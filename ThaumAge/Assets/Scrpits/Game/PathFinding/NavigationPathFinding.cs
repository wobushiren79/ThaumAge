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
    public Dictionary<Vector3Int, NavMeshBuildSource> navMeshSources = new Dictionary<Vector3Int, NavMeshBuildSource>();

    public bool navMeshIsUpdating = false;
    public bool navMeshHasNewData = false;

    public Bounds worldBounds;
    private void Awake()
    {
        InitNavMesh();
    }

    private void Update()
    {
        //HandleForUpdateNavMesh();
    }

    public void InitNavMesh()
    {
        navMeshData = new NavMeshData();

        navMeshBuildSettings = NavMesh.GetSettingsByIndex(0);
        navMeshBuildSettings.agentClimb = 1.5f;
        navMeshBuildSettings.agentSlope = 60;
        navMeshBuildSettings.agentHeight = 1.8f;

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
    public void RefreshNavMeshSource(Chunk chunk)
    {
        if (navMeshSources.TryGetValue(chunk.worldPosition,out NavMeshBuildSource source))
        {
            source.size = new Vector3(chunk.width, chunk.height + 1, chunk.width);
            source.sourceObject = chunk.chunkMeshCollider;
            source.transform = chunk.transform.localToWorldMatrix;
        }
        else
        {
            NavMeshBuildSource sourceNew = new NavMeshBuildSource();
            sourceNew.shape = NavMeshBuildSourceShape.Mesh;
            sourceNew.size = new Vector3(chunk.width, chunk.height + 1, chunk.width);
            sourceNew.sourceObject = chunk.chunkMeshCollider;
            sourceNew.transform = chunk.transform.localToWorldMatrix;
            navMeshSources.Add(chunk.worldPosition, sourceNew);
        }
        worldBounds.Encapsulate(chunk.meshRenderer.bounds);
        worldBounds.Expand(0.1f);
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
                navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, navMeshBuildSettings, navMeshSources.Values.ToList(), worldBounds);
                navMeshIsUpdating = true;
            }
            catch
            {

            }
            navMeshHasNewData = false;
        }
    }
}