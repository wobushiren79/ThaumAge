using UnityEditor;
using UnityEngine;

public class BiomeTest : Biome
{
    //测试
    public BiomeTest() : base(BiomeTypeEnum.Test)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
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
        else if (wPos.y < genHeight&& wPos.y >= genHeight-5)
        {
            //其他土
            return BlockTypeEnum.Dirt;
        }
        else
        {
            BiomeCreateTool.AddOre(900, 0.00001f, wPos);
            //其他土
            return BlockTypeEnum.Stone;
        }
    }
}