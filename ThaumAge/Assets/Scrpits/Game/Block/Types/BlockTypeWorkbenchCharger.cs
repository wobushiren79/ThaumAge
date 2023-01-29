using UnityEditor;
using UnityEngine;

public class BlockTypeWorkbenchCharger : Block
{
    public override void InitBlockColor(Color[] colorArray)
    {
        Color lightColor = new Color(1, 1, 1, 5);
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