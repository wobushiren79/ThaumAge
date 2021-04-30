using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Test : BaseMonoBehaviour
{
    public GameObject objTest;
    public GameObject objData;
    public NavMeshBuildSource source;
    public NavMeshBuildSettings navMeshBuildSettings;
    public NavMeshData navMeshData;
    public NavMeshDataInstance navMeshInstance;
    public AsyncOperation navMeshUpdateOperation;
    List<NavMeshBuildSource> navMeshSources=new List<NavMeshBuildSource>();
    bool navMeshIsUpdating, navMeshHasNewData;

    Bounds worldBounds;
    private void Start()
    {
        navMeshData = new NavMeshData();

        MeshFilter meshFilter = objData.GetComponent<MeshFilter>();
        NavMeshBuildSource source = new NavMeshBuildSource();
        source.shape = NavMeshBuildSourceShape.Mesh;
        source.size = new Vector3(100, 100, 100);
        source.sourceObject = meshFilter.mesh;
        source.transform = objData.transform.localToWorldMatrix;
        navMeshSources.Add(source);

        worldBounds = new Bounds(Vector3.zero, Vector3.one * 100);

        navMeshBuildSettings = NavMesh.GetSettingsByIndex(0);
        navMeshBuildSettings.agentClimb = 1.5f;
        navMeshBuildSettings.agentSlope = 60;
        navMeshBuildSettings.agentHeight = 1.8f;
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
        NavMeshBuilder.UpdateNavMeshData(navMeshData, navMeshBuildSettings, navMeshSources, worldBounds);
        if (navMeshInstance.valid)
        {
            NavMesh.RemoveNavMeshData(navMeshInstance);
        }
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
    }

void UpdateNavMesh()
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
            //try
            //{
            //    navMeshUpdateOperation = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, navMeshBuildSettings, navMeshSources, worldBounds);
            //    navMeshIsUpdating = true;
            //}
            //catch (Exception ex)
            //{
            //}
            //navMeshHasNewData = false;
        }
    }
}
