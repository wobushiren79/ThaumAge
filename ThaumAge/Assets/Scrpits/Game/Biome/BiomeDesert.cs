using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {
    }

    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;

        //    AddWeed(wPos);
        //    //AddFlower(wPos);        
        //    //增加枯木
        //    AddCactus(wPos);
        //    BiomeCreatePlantTool.AddDeadwood(102, 0.0005f, wPos);
        //}
        //if (localPos.y <= terrainData.maxHeight && localPos.y > terrainData.maxHeight - 30)
        //{
        //    return BlockTypeEnum.Sand;
        //}
        //if (localPos.y <= terrainData.maxHeight - 30 && localPos.y > terrainData.maxHeight - 35)
        //{
        //    return BlockTypeEnum.Dirt;
        //}
        //else if (localPos.y == 0)
        //{
        //    //基础
        //    return BlockTypeEnum.Foundation;
        //}
        //else
        //{
        //    return BlockTypeEnum.Stone;
        //}
    }

    public override void InitBiomeBlockForChunk(Chunk chunk)
    {
        //base.InitBiomeBlockForChunk(chunk, biomeMapData);
        ////获取地形数据
        //ChunkTerrainData startTerrainData = GetTerrainData(chunk, biomeMapData, 0, 0);

        //Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData.maxHeight, chunk.chunkData.positionForWorld.z);

        //AddFlowerFire(flowerPosition);
    }

    /// <summary>
    /// 增加元素花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerFire(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreatePlantTool.AddFlower(110, wPos, flowersData);
    }

    /// <summary>
    /// 增加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(201, wPos, weedData);
    }

    /// <summary>
    /// 增加仙人掌
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddCactus(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData cactusData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.001f,
            minHeight = 2,
            maxHeight = 5,
            treeTrunk = BlockTypeEnum.Cactus
        };
        BiomeCreateTreeTool.AddCactus(201, wPos, cactusData);
    }
}