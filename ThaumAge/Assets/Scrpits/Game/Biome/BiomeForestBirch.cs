using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeForestBirch : Biome
{
    //森林
    public BiomeForestBirch() : base(BiomeTypeEnum.ForestBirch)
    {

    }

    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            int waterHeight = biomeInfo.GetWaterPlaneHeight();
         
            if (localPos.y < waterHeight)
            {
                return BlockTypeEnum.Dirt;
            }
            else if (localPos.y == waterHeight)
            {

            }
            AddFlowerAndDeadWood(wPos);
            AddTreeForFallDown(wPos);
            AddTreeForTall(wPos);
            AddWeed(wPos);
            // 地表，使用草
            return BlockTypeEnum.GrassWild;
        }
        if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 10)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (localPos.y == 0)
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

    public override void InitBiomeBlockForChunk(Chunk chunk, BiomeMapData biomeMapData)
    {
        base.InitBiomeBlockForChunk(chunk, biomeMapData);
        //获取地形数据
        ChunkTerrainData startTerrainData = GetTerrainData(chunk, biomeMapData, 0, 0);

        int waterHeight = biomeInfo.GetWaterPlaneHeight();
        Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData.maxHeight, chunk.chunkData.positionForWorld.z);
        if (startTerrainData.maxHeight == waterHeight - 1)
        {

        }
        else if (startTerrainData.maxHeight < waterHeight - 1)
        {
            AddFlowerWater(flowerPosition);
        }
        else
        {
            AddFlowerWood(flowerPosition);
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
    protected void AddFlowerWater(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWater }
        };
        BiomeCreatePlantTool.AddFlower(111, wPos, flowersData);
    }


    /// <summary>
    /// 添加花花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerAndDeadWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.01f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
        //增加枯木
        BiomeCreatePlantTool.AddDeadwood(102, 0.001f, wPos);
    }

    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTreeForTall(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.008f,
            minHeight = 10,
            maxHeight = 15,
            treeTrunk = BlockTypeEnum.TreeBirch,
            treeLeaves = BlockTypeEnum.LeavesBirch,
            leavesRange = 3,
        };
        BiomeCreateTreeTool.AddTreeForTall(201, wPos, treeData);
    }

    /// <summary>
    /// 增加倒下的树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTreeForFallDown(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.002f,
            minHeight = 4,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeBirch,
        };
        BiomeCreateTreeTool.AddTreeForFallDown(211, wPos + new Vector3Int(0, 1, 0), treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedWildLong, BlockTypeEnum.WeedWildNormal, BlockTypeEnum.WeedWildShort, BlockTypeEnum.WeedWildStart }
        };
        BiomeCreatePlantTool.AddPlant(222, wPos, weedData);
    }
}