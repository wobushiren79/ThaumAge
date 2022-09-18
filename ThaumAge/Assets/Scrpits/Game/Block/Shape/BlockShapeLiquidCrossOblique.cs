using UnityEditor;
using UnityEngine;

public class BlockShapeLiquidCrossOblique : BlockShapeLiquidCross
{
    public BlockShapeLiquidCrossOblique(Block block) : base(block)
    {
        vertsAdd = BlockShapeCrossOblique.VertsAddCrossOblique;
    }
}