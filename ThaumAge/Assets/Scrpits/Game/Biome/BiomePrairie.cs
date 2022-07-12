using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            AddWeed(wPos);
            AddFlower(wPos);
            AddTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.GrassWild;
        }
        if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 5)
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

    public override void GetBlockTypeForChunk(Chunk chunk, BiomeMapData biomeMapData)
    {
        base.GetBlockTypeForChunk(chunk, biomeMapData);
        //获取地形数据
        ChunkTerrainData startTerrainData1 = GetTerrainData(chunk, biomeMapData, 0, 0);
        Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData1.maxHeight, chunk.chunkData.positionForWorld.z);
        AddFlowerEarth(flowerPosition);

        ChunkTerrainData startTerrainData2 = GetTerrainData(chunk, biomeMapData, 4, 4);
        flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData2.maxHeight, chunk.chunkData.positionForWorld.z);
        AddFlowerWood(flowerPosition);
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

    protected void AddTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.0001f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeCherry,
            treeLeaves = BlockTypeEnum.LeavesCherry,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(111, wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedWildLong, BlockTypeEnum.WeedWildNormal, BlockTypeEnum.WeedWildShort, BlockTypeEnum.WeedWildStart }
        };
        BiomeCreatePlantTool.AddPlant(222,wPos, weedData);
    }



}