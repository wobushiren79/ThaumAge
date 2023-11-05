using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BiomeMapData
{
    public ChunkTerrainData[] arrayChunkTerrainData;
    //最近的3个生态点（用于数据保存）
    public Vector3Int[] arrayCloseBiome;

    public void InitData(Chunk chunk,Action<BiomeMapData> callBackForComplete)
    {
        BiomeHandler biomeHandler = BiomeHandler.Instance;
        BiomeManager biomeManager = biomeHandler.manager;
        //获取世界类型和种子
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        //读取附近生态的数据（检测是否已经生成附近生态）
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        BiomeSaveBean biomeSaveData = GameDataHandler.Instance.manager.GetBiomeSaveData(userData.userId, worldType);
        Vector4[] closeBiomeSave = biomeSaveData.GetCloseBiomeSave(chunk);

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
        ComputeBuffer bufferTerrain = new ComputeBuffer(arrayChunkTerrainData.Length, 24);
        bufferTerrain.SetData(arrayChunkTerrainData);

        ComputeBuffer bufferBiome = BiomeHandler.Instance.manager.GetBiomeComputeBufferByWorldType(worldType);

        int kernelId = biomeManager.terrainCShader.FindKernel("CSMain");
        biomeManager.terrainCShader.SetInt("RandomSeed", worldSeed);
        biomeManager.terrainCShader.SetInt("BiomeNum", arrayChunkBiomeData.Length);
        biomeManager.terrainCShader.SetInt("BiomeSize", 256);
        biomeManager.terrainCShader.SetVectorArray("BiomeSave", closeBiomeSave);
        biomeManager.terrainCShader.SetBuffer(kernelId, "BufferTerraninData", bufferTerrain);
        biomeManager.terrainCShader.SetBuffer(kernelId, "BufferBiomeData", bufferBiome);
        biomeManager.terrainCShader.Dispatch(kernelId, arrayChunkTerrainData.Length, 1, 1);

        AsyncGPUReadback.Request(bufferTerrain, (callbackGPU) =>
        {
            bufferTerrain.GetData(arrayChunkTerrainData);
            bufferTerrain.Dispose();

            //保存数据
            ChunkTerrainData tempTerrainData = arrayChunkTerrainData[0];
            bool isSetSuccess = biomeSaveData.SetData((int)tempTerrainData.biomePosition.x, (int)tempTerrainData.biomePosition.y, tempTerrainData.biomeIndex);
            if (isSetSuccess)
            {
                GameDataHandler.Instance.manager.SaveBiomeData();
            }

            callBackForComplete?.Invoke(this);
        });
    }

}