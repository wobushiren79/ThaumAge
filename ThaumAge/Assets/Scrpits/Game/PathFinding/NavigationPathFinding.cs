using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavigationPathFinding : BaseMonoBehaviour
{
    public NavMeshBuildSettings navMeshBuildSettings;
    public NavMeshData navMeshData;
    public NavMeshDataInstance navMeshInstance;
    public AsyncOperation navMeshUpdateOperation;
    public Dictionary<Vector3Int, NavMeshBuildSource> navMeshSources;

    public bool navMeshIsUpdating = false;
    public bool navMeshHasNewData = false;

    public Bounds worldBounds;
    public void Awake()
    {
        InitNavMesh();
    }

    public void Update()
    {
        HandleForUpdateNavMesh();
    }

    /// <summary>
    /// 初始化寻路数据
    /// </summary>
    public void InitNavMesh()
    {
        navMeshSources = new Dictionary<Vector3Int, NavMeshBuildSource>();

        navMeshData = new NavMeshData();

        navMeshBuildSettings = NavMesh.GetSettingsByIndex(0);
        navMeshBuildSettings.agentClimb = 1.5f;
        navMeshBuildSettings.agentSlope = 60;
        navMeshBuildSettings.agentHeight = 1.8f;
        //navMeshBuildSettings.voxelSize = 0.125f;
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
        worldBounds = new Bounds();
    }

    /// <summary>
    /// 移除地形
    /// </summary>
    public void DestroyNavMesh()
    {
        if (navMeshInstance.valid)
        {
            NavMesh.RemoveNavMeshData(navMeshInstance);
        }
    }

    /// <summary>
    /// 创建新的地形数据
    /// </summary>
    /// <param name="chunk"></param>
    public void RefreshNavMeshSource(Chunk chunk,bool isAdd)
    {
        if (chunk == null || chunk.chunkComponent == null)
            return;
        if (isAdd)
        {
            if (navMeshSources.TryGetValue(chunk.chunkData.positionForWorld, out NavMeshBuildSource source))
            {
                source.size = new Vector3(chunk.chunkData.chunkWidth, chunk.chunkData.chunkHeight, chunk.chunkData.chunkWidth);
                source.sourceObject = chunk.chunkComponent.chunkMeshCollider;
                source.transform = chunk.chunkComponent.transform.localToWorldMatrix;
            }
            else
            {
                NavMeshBuildSource sourceNew = new NavMeshBuildSource();
                sourceNew.shape = NavMeshBuildSourceShape.Mesh;
                sourceNew.size = new Vector3(chunk.chunkData.chunkWidth, chunk.chunkData.chunkHeight, chunk.chunkData.chunkWidth);
                sourceNew.sourceObject = chunk.chunkComponent.chunkMeshCollider;
                sourceNew.transform = chunk.chunkComponent.transform.localToWorldMatrix;
                sourceNew.area = 0;
                navMeshSources.Add(chunk.chunkData.positionForWorld, sourceNew);
            }
            worldBounds.Encapsulate(chunk.chunkComponent.meshRenderer.bounds);
            //worldBounds.Expand(0.01f);
        }
        else
        {
            navMeshSources.Remove(chunk.chunkData.positionForWorld);
        }
        navMeshHasNewData = true;
    }

    /// <summary>
    /// 地形更新处理
    /// </summary>
    public void HandleForUpdateNavMesh()
    {
        if (navMeshIsUpdating)
        {
            if (navMeshUpdateOperation.isDone)
            {
                if (navMeshInstance.valid)
                {
                    NavMesh.RemoveNavMeshData(navMeshInstance);
                }
                navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
                navMeshIsUpdating = false;
            }
        }
        else if (navMeshHasNewData)
        {
            try
            {
                navMeshIsUpdating = true;
                navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, navMeshBuildSettings, navMeshSources.Values.ToList(), worldBounds);
            }
            catch(Exception e)
            {
                LogUtil.LogError(e.ToString());
            }
            navMeshHasNewData = false;
        }
    }
}