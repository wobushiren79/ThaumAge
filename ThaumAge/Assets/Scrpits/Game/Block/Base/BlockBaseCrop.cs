using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseCrop : BlockBasePlant
{
    protected List<Vector2Int[]> listGrowUV;

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        this.InitCropData(chunk, localPosition);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForMin(chunk, localPosition);
        this.RefreshCrop(chunk, localPosition, blockInfo);
    }

    /// <summary>
    /// 获取生长UV
    /// </summary>
    /// <returns></returns>
    public virtual List<Vector2Int[]> GetListGrowUV()
    {
        if (listGrowUV == null)
        {
            listGrowUV = new List<Vector2Int[]>();
            string uvs = blockInfo.uv_position;
            string[] arrayUV = uvs.SplitForArrayStr('|');
            for (int i = 0; i < arrayUV.Length; i++)
            {
                string itemUVData = arrayUV[i];
                string[] arrayUVDetails = itemUVData.SplitForArrayStr('&');
                Vector2Int[] uvData = new Vector2Int[arrayUVDetails.Length];
                for (int f = 0; f < arrayUVDetails.Length; f++)
                {
                    string itemUVDetails = arrayUVDetails[f];
                    int[] uvPosition = itemUVDetails.SplitForArrayInt(',');
                    uvData[f] = new Vector2Int(uvPosition[0], uvPosition[1]);
                }
                listGrowUV.Add(uvData);
            }
        }
        return listGrowUV;
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
        BlockMetaCrop blockCrop = FromMetaData<BlockMetaCrop>(blockData.meta);
        Vector2Int[] uvPosition = blockInfo.GetUVPosition();
        if (blockCrop.growPro >= uvPosition.Length - 1)
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
    public void InitCropData(Chunk chunk, Vector3Int localPosition)
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// 刷新植物
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public virtual void RefreshCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        chunk.chunkData.GetBlockForLocal(localPosition, out Block block, out BlockDirectionEnum direction);

        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockCrop;
        if (blockData == null)
        {
            blockCrop = new BlockMetaCrop();
            blockCrop.growPro = 0;
            blockCrop.isStartGrow = false;

            string meta = ToMetaData(blockCrop);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        //获取成长周期
        blockCrop = FromMetaData<BlockMetaCrop>(blockData.meta);
        //成长周期+1
        if (blockCrop.isStartGrow)
        {
            //是否开始生长
            blockCrop.growPro++;
        }
        else
        {
            blockCrop.isStartGrow = true;
        }

        //设置新数据
        string newMeta = ToMetaData(blockCrop);
        blockData.meta = newMeta;
        chunk.SetBlockData(blockData);
        //刷新
        WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);

        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle();
        if (blockCrop.growPro >= lifeCycle - 1)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
        }
    }

    /// <summary>
    /// 获取作物的生长周期
    /// </summary>
    /// <returns></returns>
    public virtual int GetCropLifeCycle()
    {
        List<Vector2Int[]> listGrowUV = GetListGrowUV();
        return listGrowUV.Count;
    }

    /// <summary>
    /// 初始化植物定点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void InitCropVert(Vector3[] vertsAdd)
    {
        //往下偏移的位置
        float offsetY = -1f / 16f;
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            vertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
    }
}