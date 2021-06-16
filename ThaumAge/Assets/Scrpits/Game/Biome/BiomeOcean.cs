using UnityEditor;
using UnityEngine;
public class BiomeOcean : Biome
{
    //海洋
    public BiomeOcean() : base(BiomeTypeEnum.Ocean)
    {

    }

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(biomeInfo, genHeight, localPos, wPos);
        return BlockTypeEnum.TreeOak;
    }
}