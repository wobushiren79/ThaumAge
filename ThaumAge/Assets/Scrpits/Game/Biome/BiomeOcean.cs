using UnityEditor;
using UnityEngine;
public class BiomeOcean : Biome
{
    //海洋
    public BiomeOcean() : base(BiomeTypeEnum.Ocean)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        return BlockTypeEnum.Oak;
    }
}