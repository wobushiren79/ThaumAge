using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //模型列表
    public Dictionary<string, GameObject> dicModel = new Dictionary<string, GameObject>();

    //存储着世界中所有的Chunk
    public Dictionary<Vector3Int, Chunk> dicChunk = new Dictionary<Vector3Int, Chunk>();

    //世界种子
    public int worldSeed;

    protected Vector3 offset0;
    protected Vector3 offset1;
    protected Vector3 offset2;

    protected float frequency = 0.025f;
    protected float amplitude = 10;

    public int widthChunk = 16;
    public int heightChunk = 256;
    public int minHeightChunk = 50;

    /// <summary>
    /// 获取区块模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetChunkModel()
    {
        GameObject objModel = GetModel(dicModel, "block/base", "Chunk", "Assets/Prefabs/Game/Chunk.prefab");
        return objModel;
    }

    /// <summary>
    /// 获取区块
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Chunk GetChunk(Vector3Int pos)
    {
        if(dicChunk.TryGetValue(pos,out Chunk value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// 设置世界种子
    /// </summary>
    /// <param name="worldSeed"></param>
    public void SetWorldSeed(int worldSeed)
    {
        this.worldSeed = worldSeed;
        //初始化随机种子
        UnityEngine.Random.InitState(worldSeed);

        offset0 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
        offset1 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
        offset2 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
    }

    /// <summary>
    /// 增加区域
    /// </summary>
    /// <param name="chunk"></param>
    public void AddChunk(Vector3Int position, Chunk chunk)
    {
        dicChunk.Add(position, chunk);
    }
    /// <summary>
    /// 创建区域方块数据
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight">生成地形的最低高低</param>
    /// <returns></returns>
    public Dictionary<Vector3Int, Block> CreateChunkBlockData(Chunk chunk)
    {
        //初始化Map
        Dictionary<Vector3Int, Block> mapForBlock = new Dictionary<Vector3Int, Block>();
        Vector3Int chunkPosition = Vector3Int.CeilToInt(chunk.transform.position);
        int halfWidth = widthChunk / 2;
        //遍历map，生成其中每个Block的信息
        for (int x = 0; x < widthChunk; x++)
        {
            for (int y = 0; y < heightChunk; y++)
            {
                for (int z = 0; z < widthChunk; z++)
                {
                    Vector3Int position = new Vector3Int(x - halfWidth, y, z - halfWidth);
                    //获取方块类型
                    BlockTypeEnum blockType = GetBlockType(position + chunkPosition, heightChunk, minHeightChunk);
                    //生成方块
                    Block block = BlockHandler.CreateBlock(chunk, position, blockType);
                    //添加方块
                    mapForBlock.Add(position, block);
                }
            }
        }
        return mapForBlock;
    }

    public async void CreateChunkBlockDataForAsync(Chunk chunk,Action<Dictionary<Vector3Int, Block>> callBack)
    {       
        //初始化Map
        Dictionary<Vector3Int, Block> mapForBlock = new Dictionary<Vector3Int, Block>();
        Vector3Int chunkPosition = Vector3Int.CeilToInt(chunk.transform.position);
        int halfWidth = widthChunk / 2;
        await Task.Run(()=> {

            //遍历map，生成其中每个Block的信息
            for (int x = 0; x < widthChunk; x++)
            {
                for (int y = 0; y < heightChunk; y++)
                {
                    for (int z = 0; z < widthChunk; z++)
                    {
                        Vector3Int position = new Vector3Int(x - halfWidth, y, z - halfWidth);
                        //获取方块类型
                        BlockTypeEnum blockType = GetBlockType(position + chunkPosition, heightChunk, minHeightChunk);
                        //生成方块
                        Block block = BlockHandler.CreateBlock(chunk, position, blockType);
                        //添加方块
                        mapForBlock.Add(position, block);
                    }
                }
            }
        });
        callBack?.Invoke(mapForBlock);
    }

    public int CreateHeightData(Vector3 wPos, int minHeight)
    {

        //让随机种子，振幅，频率，应用于我们的噪音采样结果
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

        //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + minHeight);
    }

    public BlockTypeEnum GetBlockType(Vector3 wPos, int height, int minHeight)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height)
        {
            return BlockTypeEnum.None;
        }

        //获取当前位置方块随机生成的高度值
        float genHeight = CreateHeightData(wPos, minHeight);
        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return BlockTypeEnum.None;
        }
        //当前方块位置等于随机生成的高度值时，当前方块类型为草地
        else if (wPos.y == genHeight)
        {
            return BlockTypeEnum.Grass;
        }
        //当前方块位置小于随机生成的高度值 且 大于 genHeight - 5时，当前方块类型为泥土
        else if (wPos.y < genHeight && wPos.y > genHeight - 5)
        {
            return BlockTypeEnum.Dirt;
        }
        //其他情况，当前方块类型为碎石
        return BlockTypeEnum.Stone;
    }
}