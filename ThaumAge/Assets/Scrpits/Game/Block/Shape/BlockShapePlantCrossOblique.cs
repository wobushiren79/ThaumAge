using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapePlantCrossOblique : BlockShapeCrossOblique , IBlockPlant
{
    public BlockShapePlantCrossOblique() : base()
    {
        this.InitPlantVert(vertsAdd);
    }
}