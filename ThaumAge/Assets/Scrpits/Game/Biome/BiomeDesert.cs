using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {
    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y == genHeight)
        {
            AddCactus(wPos);
        }
        if (wPos.y <= genHeight&& wPos.y > genHeight - 5)
        {
            return BlockTypeEnum.Sand;
        }
        if (wPos.y <= genHeight - 5 && wPos.y > genHeight - 10)
        {
            return BlockTypeEnum.Dirt;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            return BlockTypeEnum.Stone;
        }
    }

    public void AddCactus(Vector3Int startPosition)
    {
        CactusData cactusData = new CactusData(); 
        cactusData.addRateMin = 1;
        cactusData.addRateMax = 100;
        cactusData.minHeight = 1;
        cactusData.minHeight = 5;
        cactusData.cactusType = BlockTypeEnum.Cactus;
        BiomeCreateTool.AddCactus(startPosition, cactusData);
    }
}