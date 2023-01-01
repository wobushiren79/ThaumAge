using UnityEditor;
using UnityEngine;
using static BiomeCreateTreeTool;

public class BlockBaseSapling : Block
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForMin(chunk, localPosition);
        this.RefreshSapling(chunk, localPosition, blockInfo);
    }

    /// <summary>
    /// 刷新树苗
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="blockInfo"></param>
    public virtual void RefreshSapling(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        //开始生长
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
        BlockMetaSapling blockMetaSapling;
        isGrowAdd = false;
        if (blockData == null)
        {
            blockMetaSapling = new BlockMetaSapling();
            blockMetaSapling.growPro = 0;
            blockMetaSapling.isStartGrow = false;

            string meta = ToMetaData(blockMetaSapling);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        else
        {
            blockMetaSapling = FromMetaData<BlockMetaSapling>(blockData.meta);
            if (blockMetaSapling == null)
            {
                blockMetaSapling = new BlockMetaSapling();
            }
        }
        //成长周期+1
        if (blockMetaSapling.isStartGrow)
        {
            //是否开始生长
            blockMetaSapling.growPro++;
            isGrowAdd = true;
        }
        else
        {
            blockMetaSapling.isStartGrow = true;
        }

        //判断是否已经是最大生长周期
        if (blockMetaSapling.growPro >= blockInfo.remark_int)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
            CreateTree(localPosition + chunk.chunkData.positionForWorld);
            //刷新
            WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);
            return;
        }
        else
        {
            //设置新数据
            string newMeta = ToMetaData(blockMetaSapling);
            blockData.meta = newMeta;
            chunk.SetBlockData(blockData);
        }
    }

    /// <summary>
    /// 创建树
    /// </summary>
    public virtual void CreateTree(Vector3Int worldPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTreeEditor(worldPosition - Vector3Int.up, treeData);
    }
}