using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    protected int maxBiomeData = 1024;
    protected Dictionary<Vector3Int, BiomeMapData[,]> dicBiomeMapData;
    public FastNoise fastNoise;

    public Vector3 offset0;
    public Vector3 offset1;
    public Vector3 offset2;
    public float offsetBiome;
    public override void Awake()
    {
        base.Awake();
        dicBiomeMapData = new Dictionary<Vector3Int, BiomeMapData[,]>(maxBiomeData);
    }

    public void InitWorldBiomeData()
    {
        dicBiomeMapData.Clear();
    }

    public void InitWorldBiomeSeed()
    {
        int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        offsetBiome = Random.value * 1000;
        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        fastNoise = new FastNoise(seed);
    }

    public BiomeMapData[,] GetBiomeMapData(Chunk chunk)
    {
        if (dicBiomeMapData.TryGetValue(chunk.chunkData.positionForWorld, out BiomeMapData[,] mapData))
        {
            return mapData;
        }
        else
        {
            WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
            //获取该世界的所有生态
            Biome[] listBiome = manager.GetBiomeListByWorldType(worldType);
            //获取一定范围内的生态点
            Vector3Int[] listBiomeCenter = GetBiomeCenterPosition(chunk, 5, 10);

            //如果已经超过 最大缓存 则清理一波 已经没有使用的chunk数据
            if (dicBiomeMapData.Count > maxBiomeData)
            {
                List<Vector3Int> listClearData = new List<Vector3Int>();
                foreach (var itemKey in dicBiomeMapData.Keys)
                {
                    Chunk itemChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(itemKey);
                    if (itemChunk == null)
                    {
                        listClearData.Add(itemKey);
                    }
                }
                for (int i = 0; i < listClearData.Count; i++)
                {
                    dicBiomeMapData.Remove(listClearData[i]);
                }
            }
            //添加新的数据
            mapData = new BiomeMapData[chunk.chunkData.chunkWidth, chunk.chunkData.chunkHeight];
            for (int x = 0; x < chunk.chunkData.chunkWidth; x++)
            {
                for (int z = 0; z < chunk.chunkData.chunkWidth; z++)
                {
                    BiomeMapData biomeMap = new BiomeMapData();
                    biomeMap.InitData(fastNoise, new Vector3Int(x + chunk.chunkData.positionForWorld.x, chunk.chunkData.positionForWorld.y, chunk.chunkData.positionForWorld.z + z), listBiomeCenter, listBiome);
                    mapData[x, z] = biomeMap;
                }
            }
            dicBiomeMapData.Add(chunk.chunkData.positionForWorld, mapData);
            return mapData;
        }
    }

    /// <summary>
    /// 根据生物生态 创造方块
    /// </summary>
    /// <param name="listBiome"></param>
    /// <param name="wPos"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public BlockTypeEnum CreateBiomeBlockType(Chunk chunk, BiomeMapData biomeMapData, Vector3Int blockLocalPosition)
    {
        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (blockLocalPosition.y > biomeMapData.maxHeight)
        {
            if (blockLocalPosition.y <= biomeMapData.biome.biomeInfo.GetWaterPlaneHeight())
            {
                return BlockTypeEnum.Water;
            }
            else
            {
                return BlockTypeEnum.None;
            }
        }

        int maxHeight = biomeMapData.maxHeight;
        Biome biome = biomeMapData.biome;
        //边缘处理 逐渐减缓到最低高度
        if (blockLocalPosition.y > maxHeight// 在基础高度-4以上
            && biomeMapData.offsetDis <= 20) //在20范围以内
        {
            maxHeight = Mathf.CeilToInt((biomeMapData.maxHeight - biome.biomeInfo.min_height) / 20f) * Mathf.CeilToInt(biomeMapData.offsetDis) + maxHeight;

            //当前方块位置高于随机生成的高度值时，当前方块类型为空
            if (blockLocalPosition.y > maxHeight)
            {
                return BlockTypeEnum.None;
            }
        }
        Vector3Int wPos = blockLocalPosition + chunk.chunkData.positionForWorld;
        BlockTypeEnum blockType = biome.GetBlockType(chunk, biome.biomeInfo, maxHeight, blockLocalPosition, wPos);
        //获取方块
        return blockType;
    }

    /// <summary>
    /// 获取高度信息
    /// </summary>
    /// <param name="wPos"></param>
    /// <param name="biomeInfo"></param>
    /// <returns></returns>
    public int GetHeightData(Vector3Int wPos, BiomeInfoBean biomeInfo)
    {
        if (biomeInfo == null)
            return 10;
        return GetHeightData(wPos, biomeInfo.frequency, biomeInfo.amplitude, biomeInfo.min_height);
    }

    public int GetHeightData(Vector3Int wPos, float frequency, float amplitude, int minHeight)
    {
        ////让随机种子，振幅，频率，应用于我们的噪音采样结果
        float x0 = (wPos.x + offset0.x) * frequency;
        float y0 = (wPos.y + offset0.y) * frequency;
        float z0 = (wPos.z + offset0.z) * frequency;

        float x1 = (wPos.x + offset1.x) * frequency * 2;
        float y1 = (wPos.y + offset1.y) * frequency * 2;
        float z1 = (wPos.z + offset1.z) * frequency * 2;

        float x2 = (wPos.x + offset2.x) * frequency / 4;
        float y2 = (wPos.y + offset2.y) * frequency / 4;
        float z2 = (wPos.z + offset2.z) * frequency / 4;

        float noise0 = SimplexNoiseUtil.Generate(x0, y0, z0) * amplitude;
        float noise1 = SimplexNoiseUtil.Generate(x1, y1, z1) * amplitude / 2;
        float noise2 = SimplexNoiseUtil.Generate(x2, y2, z2) * amplitude / 4;

        ////在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + minHeight);
    }

    /// <summary>
    /// 获取生态中心点
    /// </summary>
    /// <param name="currentChunk"></param>
    /// <param name="range">范围</param>
    /// <param name="rate">倍数</param>
    /// <returns></returns>
    public Vector3Int[] GetBiomeCenterPosition(Chunk currentChunk, int range, int rate)
    {
        List<Vector3Int> listData = new List<Vector3Int>();
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        int unitSize = currentChunk.chunkData.chunkWidth * rate;
        int startX = Mathf.FloorToInt(currentChunk.chunkData.positionForWorld.x / (float)unitSize) * unitSize;
        int startZ = Mathf.FloorToInt(currentChunk.chunkData.positionForWorld.z / (float)unitSize) * unitSize;
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                Vector3Int currentPosition = new Vector3Int(x * unitSize + startX, 0, z * unitSize + startZ);
                //RandomTools random = RandomUtil.GetRandom(worldSeed, currentPosition.x, currentPosition.z);
                //int addRate = random.NextInt(50);
                int addRate = WorldRandTools.Range(50, currentPosition * worldSeed);
                if (addRate <= 1)
                {
                    listData.Add(currentPosition);
                }
            }
        }
        return listData.ToArray();
    }




}