
using UnityEditor;
using UnityEngine;

public class BlockShapePlantWell : BlockShapeWell, IBlockPlant
{
    public BlockShapePlantWell() : base()
    {
        this.InitPlantVert(vertsAdd);
    }
}