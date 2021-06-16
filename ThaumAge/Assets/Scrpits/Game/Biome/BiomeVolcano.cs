using UnityEditor;
using UnityEngine;

public class BiomeVolcano : Biome
{
    //火山
    public BiomeVolcano() : base(BiomeTypeEnum.Volcano)
    {
    }

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(biomeInfo,genHeight, localPos, wPos);
        return BlockTypeEnum.Stone;
    }
}