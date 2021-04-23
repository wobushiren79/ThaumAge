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
        if (wPos.y == genHeight)
        {
            //AddWeed(wPos);
            //AddFlower(wPos);
            //AddTree(wPos);
            //AddBigTree(wPos);
            //AddWorldTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass;
        }
        if (wPos.y < genHeight && wPos.y > genHeight - 10)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            //其他石头
            return BlockTypeEnum.Stone;
        }
    }
}