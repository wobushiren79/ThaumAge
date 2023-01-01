using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseCrop : BlockBasePlant
{
    protected List<Vector2Int[]> listGrowUV;

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
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
    /// 刷新植物
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public virtual void RefreshCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        StartGrow(chunk, localPosition, blockInfo, out bool isGrowAdd);
    }

    /// <summary>
    /// 开始生长
    /// </summary>
    public virtual void StartGrow(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo,
        out bool isGrowAdd)
    {
        chunk.chunkData.GetBlockForLocal(localPosition, out Block block, out BlockDirectionEnum direction);

        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockMetaCrop;
        isGrowAdd = false;
        if (blockData == null)
        {
            blockMetaCrop = new BlockMetaCrop();
            blockMetaCrop.growPro = 0;
            blockMetaCrop.isStartGrow = false;

            string meta = ToMetaData(blockMetaCrop);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        else
        {
            blockMetaCrop = FromMetaData<BlockMetaCrop>(blockData.meta);
        }
        //如果是已经长好的 就不在监听
        if (blockMetaCrop.level > 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
            return;
        }

        //获取下方方块，如果下方方块时耕地
        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Down, out Block blockDown, out Chunk blockChunkDown, out Vector3Int blockDownLocalPosition);
        if (blockDown is BlockBasePlough)
        {
            BlockBean blockDownData = blockChunkDown.GetBlockData(blockDownLocalPosition.x, blockDownLocalPosition.y, blockDownLocalPosition.z);
            if (blockDownData != null)
            {
                BlockMetaPlough blockMetaPlough = FromMetaData<BlockMetaPlough>(blockDownData.meta);
                //并且已经浇水 才能生长
                if (blockMetaPlough != null && blockMetaPlough.waterState == 1)
                {
                    //成长周期+1
                    if (blockMetaCrop.isStartGrow)
                    {
                        //是否开始生长
                        blockMetaCrop.growPro++;
                        isGrowAdd = true;

                        //浇水的土地干了
                        blockMetaPlough.waterState = 0;
                        blockDownData.meta = ToMetaData(blockMetaPlough);
                    }
                    else
                    {
                        blockMetaCrop.isStartGrow = true;
                    }

                    //判断是否已经是最大生长周期
                    int lifeCycle = GetCropLifeCycle(blockInfo);
                    if (blockMetaCrop.growPro >= lifeCycle)
                    {
                        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
                    }

                    //设置新数据
                    string newMeta = ToMetaData(blockMetaCrop);
                    blockData.meta = newMeta;
                    chunk.SetBlockData(blockData);
                    //刷新
                    WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);
                }
            }
        }
    }


    /// <summary>
    /// 获取作物的生长周期
    /// </summary>
    /// <returns></returns>
    public virtual int GetCropLifeCycle(BlockInfoBean blockInfo)
    {
        return blockInfo.remark_int;
    }

    /// <summary>
    /// 初始化植物定点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static Vector3[] InitCropVert(Vector3[] vertsAdd)
    {
        //往下偏移的位置
        Vector3[] newVertsAdd = new Vector3[vertsAdd.Length];
        float offsetY = -(1f / 16f);
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            newVertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
        return newVertsAdd;
    }


    /// <summary>
    /// 获取生长UV
    /// </summary>
    public static Vector2 GetUVStartPosition(Block block, BlockInfoBean blockInfo, BlockMetaCrop blockCropData)
    {
        BlockBaseCrop blockCrop = block as BlockBaseCrop;
        List<Vector2Int[]> listUVData = blockCrop.GetListGrowUV();
        Vector2 uvStartPosition;
        if (listUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (blockCropData.growPro >= blockCrop.GetCropLifeCycle(blockInfo))
        {
            //如果生长周期大于UV长度 则取最后一个
            Vector2Int[] itemUVData = listUVData[listUVData.Count - 1];
            uvStartPosition = new Vector2(BlockShape.uvWidth * itemUVData[blockCropData.level].y, BlockShape.uvWidth * itemUVData[blockCropData.level].x);
        }
        else
        {
            Vector2Int[] itemUVData = listUVData[blockCropData.growPro];
            //按生长周期取UV
            uvStartPosition = new Vector2(BlockShape.uvWidth * itemUVData[blockCropData.level].y, BlockShape.uvWidth * itemUVData[blockCropData.level].x);
        }
        return uvStartPosition;
    }
}