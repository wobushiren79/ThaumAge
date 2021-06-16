using UnityEditor;
using UnityEngine;

public class BiomeMountain : Biome
{
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {
    }

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(biomeInfo, genHeight, localPos, wPos);
        return BlockTypeEnum.TreeOak;
    }
}