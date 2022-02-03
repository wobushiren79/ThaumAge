using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapePlantCrossOblique : BlockShapeCrossOblique
{
    public BlockShapePlantCrossOblique() : base()
    {
        BlockBasePlant.InitPlantVert(vertsAdd);
    }

}