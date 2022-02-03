
using UnityEditor;
using UnityEngine;

public class BlockShapePlantWell : BlockShapeWell
{
    public BlockShapePlantWell() : base()
    {
        BlockBasePlant.InitPlantVert(vertsAdd);
    }


}