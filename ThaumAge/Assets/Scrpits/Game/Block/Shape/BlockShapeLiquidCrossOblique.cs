using UnityEditor;
using UnityEngine;

public class BlockShapeLiquidCrossOblique : BlockShapeLiquidCross
{
    public BlockShapeLiquidCrossOblique() : base()
    {
        vertsAdd = BlockShapeCrossOblique.VertsAddCrossOblique;
    }
}