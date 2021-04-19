using UnityEditor;
using UnityEngine;

public class BiomeMagicForest : Biome
{
    //魔法深林
    public BiomeMagicForest() : base(BiomeTypeEnum.MagicForest)
    {
    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(genHeight, localPos, wPos);
        return BlockTypeEnum.Dirt;
    }
}