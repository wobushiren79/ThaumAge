using UnityEditor;
using UnityEngine;

public class BiomeTest : Biome
{
    //测试
    public BiomeTest() : base(BiomeTypeEnum.Test)
    {

    }

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y == genHeight)
        {
            //草
            return BlockTypeEnum.Grass;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            //其他土
            return BlockTypeEnum.Dirt;
        }
    }
}