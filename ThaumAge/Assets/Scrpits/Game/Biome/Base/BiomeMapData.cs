using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeMapData
{
    public ChunkTerrainData[] arrayChunkTerrainData;

    public BiomeMapData(Chunk chunk)
    {
        BiomeHandler biomeHandler = BiomeHandler.Instance;
        BiomeManager biomeManager = biomeHandler.manager;
        //获取世界类型和种子
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();

        //世界的生态信息
        ChunkBiomeData[] arrayChunkBiomeData = biomeManager.GetBiomeDataByWorldType(worldType, chunk);
        //设置地形数据
        arrayChunkTerrainData = new ChunkTerrainData[chunk.chunkData.chunkWidth * chunk.chunkData.chunkWidth];
        for (int x = 0; x < chunk.chunkData.chunkWidth; x++)
        {
            for (int z = 0; z < chunk.chunkData.chunkWidth; z++)
            {
                ChunkTerrainData itemTerrainData = new ChunkTerrainData
                {
                    position = new Vector2(x + chunk.chunkData.positionForWorld.x, z + chunk.chunkData.positionForWorld.z)
                };
                arrayChunkTerrainData[x * chunk.chunkData.chunkWidth + z] = itemTerrainData;
            }
        }
        ComputeBuffer bufferTerrain = new ComputeBuffer(arrayChunkTerrainData.Length, 16);
        bufferTerrain.SetData(arrayChunkTerrainData);

        ComputeBuffer bufferBiome = BiomeHandler.Instance.manager.GetBiomeComputeBufferByWorldType(worldType);

        int kernelId = biomeManager.terrainCShader.FindKernel("CSMain");
        biomeManager.terrainCShader.SetInt("RandomSeed", worldSeed);
        biomeManager.terrainCShader.SetInt("BiomeNum", arrayChunkBiomeData.Length);
        biomeManager.terrainCShader.SetInt("BiomeSize", 256);
        biomeManager.terrainCShader.SetBuffer(kernelId, "BufferTerraninData", bufferTerrain);
        biomeManager.terrainCShader.SetBuffer(kernelId, "BufferBiomeData", bufferBiome);
        biomeManager.terrainCShader.Dispatch(kernelId, arrayChunkTerrainData.Length, 1, 1);
        bufferTerrain.GetData(arrayChunkTerrainData);
        bufferTerrain.Dispose();
    }
}