using UnityEditor;
using UnityEngine;

public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {
    }

    public override BlockTypeEnum GetBlockType(int genHeight,  Vector3Int wPos)
    {
        return  BlockTypeEnum.Sand;
    }
}