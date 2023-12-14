using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTreeTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override void CreateBlockStructureForNormalTree(int blockId, Vector3Int baseWorldPosition)
    {
        BlockTypeEnum blockType = (BlockTypeEnum)blockId;
        if (blockType == BlockTypeEnum.TreeCherry)
        {
            BiomeCreateTreeTool.CreateNormalTree(baseWorldPosition, blockId, 6, 10, 2, (int)BlockTypeEnum.LeavesCherry);
        }
    }

    /// <summary>
    /// 增加元素花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWood }
        };
        BiomeCreatePlantTool.AddFlower(110, wPos, flowersData);
    }
    protected void AddFlowerEarth(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerEarth }
        };
        BiomeCreatePlantTool.AddFlower(111, wPos, flowersData);
    }
    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedWildLong, BlockTypeEnum.WeedWildNormal, BlockTypeEnum.WeedWildShort, BlockTypeEnum.WeedWildStart }
        };
        BiomeCreatePlantTool.AddPlant(222, wPos, weedData);
    }



}