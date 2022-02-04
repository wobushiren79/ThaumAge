using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropCrossOblique : BlockShapeCrossOblique
{
    public BlockShapeCropCrossOblique() : base()
    {
        BlockBaseCrop.InitPlantVert(vertsAdd);
    }

}