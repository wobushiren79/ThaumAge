using UnityEditor;
using UnityEngine;

public class BiomeMapData
{

    public ChunkTerrainData[] arrayChunkTerrainData;
    //生态
    public Biome biome;


    public BiomeMapData(Chunk chunk)
    {
        BiomeHandler biomeHandler = BiomeHandler.Instance;
        BiomeManager biomeManager = biomeHandler.manager;
        //获取世界类型和种子
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        //获取该世界的所有生态
        Biome[] listBiome = BiomeHandler.Instance.manager.GetBiomeListByWorldType(worldType);
        //获取一定范围内的生态点
        Vector3Int[] listBiomeCenter = biomeHandler.GetBiomeCenterPosition(chunk, 5, 10);

        //距离该方块最近的生态点距离
        float minBiomeDis = float.MaxValue;
        //距离该方块第二近的生态点距离
        float secondMinBiomeDis = float.MaxValue;
        //最靠近的生态点
        Vector3Int minBiomePosition = Vector3Int.zero;
        //便利中心点，寻找最靠近的生态点（维诺图）
        for (int i = 0; i < listBiomeCenter.Length; i++)
        {
            Vector3Int itemCenterPosition = listBiomeCenter[i];
            float tempDis = Vector3Int.Distance(itemCenterPosition, chunk.chunkData.positionForWorld);

            //如果小于最小距离
            if (tempDis <= minBiomeDis)
            {
                minBiomePosition = itemCenterPosition;
                minBiomeDis = tempDis;
            }
            //如果大于最小距离 并且小于第二小距离
            else if (tempDis > minBiomeDis && tempDis <= secondMinBiomeDis)
            {
                secondMinBiomeDis = tempDis;
            }
        }
        //查询生态
        int biomeIndex = WorldRandTools.Range(listBiome.Length, minBiomePosition);
        biome = listBiome[biomeIndex];
        arrayChunkTerrainData = new ChunkTerrainData[chunk.chunkData.chunkWidth * chunk.chunkData.chunkWidth];

        for (int x = 0; x < chunk.chunkData.chunkWidth; x++)
        {
            for (int z = 0; z < chunk.chunkData.chunkWidth; z++)
            {
                ChunkTerrainData itemTerrainData = new ChunkTerrainData
                {
                    position = new Vector2(x, z),
                    perlinFrequency = biome.biomeInfo.frequency,
                    perlinAmplitude = biome.biomeInfo.amplitude,
                    perlinSize = biome.biomeInfo.scale,
                    perlinIterateNumber = biome.biomeInfo.iterate_number,
                    minHeight = biome.biomeInfo.min_height,
                    biomeIndex = biomeIndex,
                    minBiomeDis = minBiomeDis,
                    secondMinBiomeDis = secondMinBiomeDis
                };
                arrayChunkTerrainData[x * chunk.chunkData.chunkWidth + z] = itemTerrainData;
            }
        }
        ComputeBuffer buffer = new ComputeBuffer(arrayChunkTerrainData.Length, 44);
        buffer.SetData(arrayChunkTerrainData);
        int kernelId = biomeManager.terrainCShader.FindKernel("CSMain");
        biomeManager.terrainCShader.SetFloats("RandomOffset", worldSeed, worldSeed);
        biomeManager.terrainCShader.SetBuffer(kernelId, "BufferTerraninData", buffer);
        biomeManager.terrainCShader.Dispatch(kernelId, arrayChunkTerrainData.Length, 1, 1);
        buffer.GetData(arrayChunkTerrainData);
        buffer.Dispose();

    }
}