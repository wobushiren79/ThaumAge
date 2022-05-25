using UnityEditor;
using UnityEngine;

public class BlockTypeCropWatermelonGrow : BlockBaseCrop
{
    public override void RefreshCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        base.RefreshCrop(chunk, localPosition, blockInfo);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //获取成长周期
        BlockMetaCrop blockCropData = FromMetaData<BlockMetaCrop>(blockData.meta);

        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle(blockInfo);
        if (blockCropData.growPro >= lifeCycle)
        {
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.CropWatermelon);
        }
    }
}