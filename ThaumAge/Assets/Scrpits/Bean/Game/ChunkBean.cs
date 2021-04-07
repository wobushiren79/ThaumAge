using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkBean
{
    public Vector3IntBean position;
    public List<BlockBean> listBlockData = new List<BlockBean>();

    public Dictionary<Vector3Int, BlockBean> dicBlockData = new Dictionary<Vector3Int, BlockBean>();

    public void InitData()
    {
        dicBlockData.Clear();
        for (int i = 0; i < listBlockData.Count; i++)
        {
            BlockBean blockData = listBlockData[i];
            Vector3Int localPosition = blockData.localPosition.GetVector3Int();
            if (!dicBlockData.ContainsKey(localPosition))
                dicBlockData.Add(localPosition, blockData);
        }
    }

    public void SaveData()
    {
        listBlockData.Clear();
        foreach (var itemData in dicBlockData)
        {
            listBlockData.Add(itemData.Value);
        }
    }
}