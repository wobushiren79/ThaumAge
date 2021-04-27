﻿using Pathfinding;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AstarPathManager : BaseManager
{
    protected Dictionary<Vector3Int, GridGraph> dicGridGraph = new Dictionary<Vector3Int, GridGraph>();

    /// <summary>
    /// 创建网格路面
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="depth"></param>
    /// <param name="nodeSize"></param>
    public void CreateGridGraph(Vector3Int center, int width, int depth, float nodeSize)
    {
        if (dicGridGraph.TryGetValue(center,out  GridGraph  value))
        {
            value.Scan();
        }
        else
        {
            AstarData astarData = AstarPath.active.data;
            GridGraph gridGraph = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
            gridGraph.center = center;
            gridGraph.SetDimensions(width, depth, nodeSize);
            dicGridGraph.Add(center, gridGraph);
            gridGraph.Scan();
        }
    }

    /// <summary>
    /// 刷新网格路面
    /// </summary>
    /// <param name="center"></param>
    public void RefreshGridGraph(Vector3Int center)
    {
        if (dicGridGraph.TryGetValue(center, out GridGraph value))
        {
            value.Scan();
        }
    }

    /// <summary>
    /// 删除网格路面
    /// </summary>
    /// <param name="center"></param>
    public void DestroyGridGraph(Vector3Int center)
    {
        if (dicGridGraph.TryGetValue(center, out GridGraph value))
        {
            AstarData astarData = AstarPath.active.data;
            astarData.RemoveGraph(value);
            dicGridGraph.Remove(center);
        }
    }

}