using UnityEditor;
using UnityEngine;

public class BlockTypeInfernalFurnace : BlockBaseLinkLarge
{
    public override BuildingTypeEnum GetBuildingType()
    {
        return BuildingTypeEnum.InfernalFurnace;
    }

    public override void InitBlockColor(Color[] colorArray)
    {
        Color lightColor = new Color(0.63f, 0.023f, 0, 2);
        BlockShapeCustom blockShapeCustom = blockShape as BlockShapeCustom;
        for (int i = 0; i < colorArray.Length; i++)
        {
            MeshDataCustom meshDataCustom = blockShapeCustom.GetBlockMeshData();
            Color texColor = meshDataCustom.mainMeshData.texColor[i];
            if (texColor.r > 0.5f)
            {
                colorArray[i] = lightColor;
            }
            else
            {
                colorArray[i] = Color.white;
            }
        }
    }
}