using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkBean
{
    public Vector3Int position;

    public List<BlockBean> listBlockData = new List<BlockBean>();

    public Dictionary<int, BlockBean> dicBlockData = new Dictionary<int, BlockBean>();

    public void InitData()
    {
        dicBlockData.Clear();
        int widthChunk = WorldCreateHandler.Instance.manager.widthChunk;
        int heightChunk = WorldCreateHandler.Instance.manager.heightChunk;
        for (int i = 0; i < listBlockData.Count; i++)
        {
            BlockBean blockData = listBlockData[i];
            Vector3Int localPosition = blockData.localPosition;
            int index = MathUtil.GetSingleIndexForThree(localPosition, widthChunk, heightChunk);
            if (!dicBlockData.ContainsKey(index))
                dicBlockData.Add(index, blockData);
        }
    }

    public void SaveData()
    {
        try
        {
            listBlockData.Clear();
            foreach (var itemData in dicBlockData)
            {
                listBlockData.Add(itemData.Value);
            }
        }
        catch
        {

        }
    }

    public bool GetBlockData(Vector3Int localPosition,out BlockBean blockData)
    {
        int widthChunk = WorldCreateHandler.Instance.manager.widthChunk;
        int heightChunk = WorldCreateHandler.Instance.manager.heightChunk;
        int index = MathUtil.GetSingleIndexForThree(localPosition, widthChunk, heightChunk);
        if (dicBlockData.TryGetValue(index, out  blockData))
        {
            return true;     
        }
        return false;
    }
}