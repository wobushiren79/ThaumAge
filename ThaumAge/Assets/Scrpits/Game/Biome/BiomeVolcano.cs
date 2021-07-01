using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeVolcano : Biome
{
    //火山
    public BiomeVolcano() : base(BiomeTypeEnum.Volcano)
    {
    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk,biomeInfo, genHeight, localPos, wPos);
        float noise = (genHeight - biomeInfo.minHeight) / biomeInfo.amplitude;
        if (noise >= 0.9f)
        {
            if (localPos.y >= genHeight - 3)
            {
                return BlockTypeEnum.None;
            }
            else if (localPos.y > 20 && localPos.y < genHeight - 1)
            {
                return BlockTypeEnum.Magma;
            }
            else
            {
                return BlockTypeEnum.StoneVolcanic;
            }
        }
        if (genHeight == localPos.y)
        {
            AddDeadwood(wPos);
            AddFireFlower(wPos);
        }
        return BlockTypeEnum.StoneVolcanic;
    }

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData();
        treeData.addRate = 0.005f;
        treeData.minHeight = 1;
        treeData.maxHeight = 8;
        treeData.treeTrunk = BlockTypeEnum.TreeOak;
        BiomeCreateTool.AddDeadwood(101, startPosition, treeData);
    }

    /// <summary>
    /// 增加火焰花
    /// </summary>
    /// <param name="wPos"></param>
    public void AddFireFlower(Vector3Int wPos)
    {
        BiomeForFlowerData flowersData = new BiomeForFlowerData
        {
            addRate = 0.005f,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreateTool.AddFlower(201, wPos, flowersData);
    }
}