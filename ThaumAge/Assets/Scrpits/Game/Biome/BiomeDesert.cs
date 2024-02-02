using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {

    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
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

    public void InitBiomeBlockForChunk(Chunk chunk)
    {
        //base.InitBiomeBlockForChunk(chunk, biomeMapData);
        ////获取地形数据
        //ChunkTerrainData startTerrainData = GetTerrainData(chunk, biomeMapData, 0, 0);

        //Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData.maxHeight, chunk.chunkData.positionForWorld.z);

        //AddFlowerFire(flowerPosition);
    }

}