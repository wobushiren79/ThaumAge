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
        //如果是等级大于0的子集 则不继续往上生长
        if (blockCropData.uvIndex > 0)
        {
            return;
        }

        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle();
        if (blockCropData.growPro >= lifeCycle - 1)
        {
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.CropWatermelon);
        }
    }
}