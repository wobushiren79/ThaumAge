using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSaveBean : BaseBean
{
    //世界类型
    public int worldType = 0;
    //所属账号
    public string userId;
    //chunk坐标
    public Vector3Int position;
    //保存的方块数据
    public BlockBean[] arrayBlockData = new BlockBean[0];
    //生态数据
    public int biomeType = -1;

    //方块数据 改变
    public Dictionary<int, BlockBean> dicBlockData = new Dictionary<int, BlockBean>();


    public void InitData()
    {
        dicBlockData.Clear();
        int widthChunk = WorldCreateHandler.Instance.manager.widthChunk;
        int heightChunk = WorldCreateHandler.Instance.manager.heightChunk;
        for (int i = 0; i < arrayBlockData.Length; i++)
        {
            BlockBean blockData = arrayBlockData[i];
            Vector3Int localPosition = blockData.localPosition;
            int index = MathUtil.GetSingleIndexForThree(localPosition, widthChunk, heightChunk);
            if (!dicBlockData.ContainsKey(index))
                dicBlockData.Add(index, blockData);
        }
    }

    public void SaveData()
    {
        arrayBlockData = new BlockBean[dicBlockData.Count];
        int i = 0;
        foreach (var itemData in dicBlockData)
        {
            arrayBlockData[i] = itemData.Value;
            i++;
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

    public WorldTypeEnum GetWorldType()
    {
        return (WorldTypeEnum)worldType;
    }
}