﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathFindingManager : BaseManager
{
    protected AstarPathFinding astarPathFinding;
    protected NavigationPathFinding navigationPathFinding;

    private void Awake()
    {
        switch (ProjectConfigInfo.AI_PATHFINDING)
        {
            case PathFindingEnum.Navigation:
                navigationPathFinding = CptUtil.AddCpt<NavigationPathFinding>(gameObject);
                break;
            case PathFindingEnum.Astar:
                astarPathFinding = CptUtil.AddCpt<AstarPathFinding>(gameObject);
                break;
        }
    }

    /// <summary>
    /// 初始化寻路
    /// </summary>
    public void InitPathFinding()
    {
        switch (ProjectConfigInfo.AI_PATHFINDING)
        {
            case PathFindingEnum.Navigation:
                navigationPathFinding.InitNavMesh();
                break;
            case PathFindingEnum.Astar:
                break;
        }
    }

    /// <summary>
    /// 刷新寻路
    /// </summary>
    /// <param name="chunk"></param>
    public void RefreshPathFinding(Chunk chunk,bool isAdd)
    {
        switch (ProjectConfigInfo.AI_PATHFINDING)
        {
            case PathFindingEnum.Navigation:
                navigationPathFinding.RefreshNavMeshSource(chunk, isAdd);
                break;
            case PathFindingEnum.Astar:
                //astarPathFinding.RefreshGraph();
                break;
        }
    }

    /// <summary>
    /// 移除寻路
    /// </summary>
    public void DestoryPathFinding()
    {
        switch (ProjectConfigInfo.AI_PATHFINDING)
        {
            case PathFindingEnum.Navigation:
                navigationPathFinding.DestroyNavMesh();
                break;
            case PathFindingEnum.Astar:
                break;
        }
    }
}