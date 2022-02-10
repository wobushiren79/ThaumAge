using UnityEditor;
using UnityEngine;

public class BlockTypeCropCorn : BlockBaseCrop
{
    public override void RefreshCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        base.RefreshCrop(chunk, localPosition, blockInfo);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //获取成长周期
        BlockCropBean blockCropData = FromMetaData<BlockCropBean>(blockData.meta);
        //如果是等级大于0的子集 则不继续往上生长
        if (blockCropData.uvIndex > 0)
        {
            return;
        }

        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle();
        if (blockCropData.growPro >= lifeCycle - 1)
        {
            //在玉米的上2格再生成同样的方格

            //更新方块并 添加更新区块
            GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block blockUp, out Chunk chunkUp);
            GetCloseBlockByDirection(chunk, localPosition + Vector3Int.up, DirectionEnum.UP, out Block blockUpUp, out Chunk chunkUpUp);

            if (chunkUp != null && blockUp != null && blockUp.blockType != BlockTypeEnum.None)
            {
                return;
            }
            BlockCropBean blockCropDataUp = FromMetaData<BlockCropBean>(blockData.meta);
            if (chunkUpUp != null && blockUpUp != null && blockUpUp.blockType != BlockTypeEnum.None)
            {
                blockCropDataUp.uvIndex = 2;
            }
            else
            {
                blockCropDataUp.uvIndex = 1;
            }
            chunk.SetBlockForLocal(localPosition + Vector3Int.up, BlockTypeEnum.CropCorn, BlockDirectionEnum.UpForward, ToMetaData(blockCropDataUp), false);

            //继续往上
            if (chunkUpUp != null && blockUpUp != null && blockUpUp.blockType != BlockTypeEnum.None)
            {
                return;
            }
            blockCropDataUp.uvIndex = 2;
            chunk.SetBlockForLocal(localPosition + Vector3Int.up * 2, BlockTypeEnum.CropCorn, BlockDirectionEnum.UpForward, ToMetaData(blockCropDataUp), false);
        }
    }
}