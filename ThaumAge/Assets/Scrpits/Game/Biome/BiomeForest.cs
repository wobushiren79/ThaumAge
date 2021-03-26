using UnityEditor;
using UnityEngine;

public class BiomeForest : Biome
{
    //森林
    public BiomeForest() : base(BiomeTypeEnum.Forest)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int wPos)
    {
        return BlockTypeEnum.LeavesOak;
    }
}