using UnityEditor;
using UnityEngine;

public class BiomeMountain : Biome
{
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {
    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(genHeight, localPos, wPos);
        return BlockTypeEnum.Oak;
    }
}