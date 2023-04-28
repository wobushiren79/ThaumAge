using UnityEditor;
using UnityEngine;

public class BlockTypeCropWatermelon : Block
{

    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        BlockShapeCustom blockShapeCustom = blockShape as BlockShapeCustom;
        blockShapeCustom.vertsAdd = blockShapeCustom.vertsAdd.AddY(-(1f / 16f));
    }

}