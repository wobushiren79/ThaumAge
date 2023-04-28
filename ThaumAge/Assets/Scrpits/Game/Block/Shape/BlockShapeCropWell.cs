using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropWell : BlockShapeWell
{
    public BlockShapeCropWell(Block block) : base(block)
    {
        vertsAdd = vertsAdd.AddY(-(1f / 16f));
    }
    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public static Vector2[] GetUVsAddForCrop(Chunk chunk, Block block, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockCropData = Block.FromMetaData<BlockMetaCrop>(blockData.meta);

        Vector2 uvStart = BlockBaseCrop.GetUVStartPosition(block, blockInfo, blockCropData);
        Vector2[] uvsAdd = new Vector2[]
        {
            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),

            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y)
        };
        return uvsAdd;
    }

    public override void BaseAddVertsUVsColors(
        Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd,
        Vector3[] vertsColliderAdd, Vector3[] vertsTriggerAdd)
    {
        Vector2[] uvsAddNew = GetUVsAddForCrop(chunk, block, localPosition, block.blockInfo);
        base.BaseAddVertsUVsColors(chunk, localPosition, blockDirection, vertsAdd, uvsAddNew, colorsAdd, vertsColliderAdd, vertsTriggerAdd);
    }
}