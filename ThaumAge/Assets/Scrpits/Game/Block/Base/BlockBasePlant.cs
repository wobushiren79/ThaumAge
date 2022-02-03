using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBasePlant : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.InitBlock(chunk, localPosition);
        this.InitPlantData(chunk, localPosition);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForMin(chunk, localPosition);
        this.RefreshPlant(chunk, localPosition, blockInfo);
    }

    /// <summary>
    /// 获取种植收获
    /// </summary>
    public override List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        List<ItemsBean> listData = new List<ItemsBean>();
        if (blockData == null || blockData.meta.IsNull())
        {
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockInfo.GetBlockType());
            listData.Add(new ItemsBean(itemsInfo.id, 1));
            return listData;
        }
        FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);
        Vector2Int[] uvPosition = blockInfo.GetUVPosition();
        if (growPro >= uvPosition.Length - 1)
        {
            //已经成熟
            listData = base.GetDropItems(blockData);
        }
        else
        {
            //没有成熟
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockInfo.GetBlockType());
            listData.Add(new ItemsBean(itemsInfo.id, 1));
        }
        return listData;
    }

    /// <summary>
    /// 初始化植物数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void InitPlantData(Chunk chunk, Vector3Int localPosition)
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// 刷新植物
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void RefreshPlant(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        chunk.chunkData.GetBlockForLocal(localPosition, out Block block, out DirectionEnum direction);

        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
        {
            string meta = ToMetaData(0, false);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        //获取成长周期
        FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);
        //成长周期+1
        if (isStartGrow)
        {
            //是否开始生长
            growPro++;
        }
        else
        {
            isStartGrow = true;
        }

        //设置新数据
        string newMeta = ToMetaData(growPro, isStartGrow);
        blockData.meta = newMeta;
        chunk.SetBlockData(blockData);
        //刷新
        WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, block, block, direction);

        //判断是否已经是最大生长周期
        Vector2Int[] arrayUVData = blockInfo.GetUVPosition();
        if (growPro >= arrayUVData.Length - 1)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
        }
    }


    /// <summary>
    /// 初始化植物定点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void InitPlantVert(Vector3[] vertsAdd)
    {
        //往下偏移的位置
        float offsetY = -1f / 16f;
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            vertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
    }

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <returns></returns>
    public static string ToMetaData(int growPro, bool isStartGrow)
    {
        return $"{growPro}:{isStartGrow}";
    }

    public static void FromMetaData(string data, out int growPro, out bool isStartGrow)
    {
        string[] dataList = data.SplitForArrayStr(':');
        growPro = int.Parse(dataList[0]);
        isStartGrow = bool.Parse(dataList[1]);
    }
}