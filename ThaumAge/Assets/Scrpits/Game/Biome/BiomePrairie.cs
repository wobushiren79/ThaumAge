using UnityEditor;
using UnityEngine;
public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass;
        }
        if (wPos.y < genHeight && wPos.y > genHeight - 5)
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