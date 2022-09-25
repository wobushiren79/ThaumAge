using UnityEditor;
using UnityEngine;

public class BlockTypeCropFlax : BlockBaseCrop
{
    public override void RefreshCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        base.RefreshCrop(chunk, localPosition, blockInfo);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //获取成长周期
        BlockMetaCrop blockCropData = FromMetaData<BlockMetaCrop>(blockData.meta);
        //判断是否是衍生植物
        if (blockCropData.level > 0)
            return;
        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle(blockInfo);

        if (blockCropData.growPro >= lifeCycle)
        {
            //在亚麻的上1格再生成同样的方格

            //更新方块并 添加更新区块
            GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block blockUp, out Chunk chunkUp, out Vector3Int localPositionUp);

            if (chunkUp != null && blockUp != null && blockUp.blockType != BlockTypeEnum.None)
            {
                return;
            }
            BlockMetaCrop blockCropDataUp = FromMetaData<BlockMetaCrop>(blockData.meta);
            blockCropDataUp.level = 1;
            chunk.SetBlockForLocal(localPosition + Vector3Int.up, BlockTypeEnum.CropFlax, BlockDirectionEnum.UpForward, ToMetaData(blockCropDataUp), false);
        }
    }
}