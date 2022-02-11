using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSaveBean : BaseBean
{
    //世界类型
    public int workdType = 0;
    //所属账号
    public string userId;
    //chunk坐标
    public Vector3Int position;
    //保存的方块数据
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

    public BlockBean GetBlockData(int x, int y, int z)
    {
        int widthChunk = WorldCreateHandler.Instance.manager.widthChunk;
        int heightChunk = WorldCreateHandler.Instance.manager.heightChunk;
        int index = MathUtil.GetSingleIndexForThree(x, y, z, widthChunk, heightChunk);
        if (dicBlockData.TryGetValue(index, out BlockBean blockData))
        {
            return blockData;
        }
        return null;
    }

    public void ClearBlockData(int x, int y, int z)
    {
        int widthChunk = WorldCreateHandler.Instance.manager.widthChunk;
        int heightChunk = WorldCreateHandler.Instance.manager.heightChunk;
        int index = MathUtil.GetSingleIndexForThree(x, y, z, widthChunk, heightChunk);
        if(dicBlockData.ContainsKey(index))
            dicBlockData.Remove(index);
    }

    public WorldTypeEnum GetWorkType()
    {
        return (WorldTypeEnum)workdType;
    }
}