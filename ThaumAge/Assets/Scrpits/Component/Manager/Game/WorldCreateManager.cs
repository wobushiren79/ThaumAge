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

    public int widthChunk = 16;
    public int heightChunk = 256;

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
        //初始化生态种子
        BiomeHandler.Instance.InitWorldBiomeSeed();
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
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(position + chunkPosition,widthChunk,heightChunk);
                    //生成方块
                    Block block = BlockHandler.Instance.CreateBlock(chunk, position, blockType);
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
        BiomeManager biomeManager = BiomeHandler.Instance.manager;
        BlockManager blockManager = BlockHandler.Instance.manager;
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
                        BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(position + chunkPosition, widthChunk, heightChunk);
                        //生成方块
                        Block block = BlockHandler.Instance.CreateBlock(chunk, position, blockType);
                        //添加方块
                        mapForBlock.Add(position, block);
                    }
                }
            }
        });
        callBack?.Invoke(mapForBlock);
    }

    /// <summary>
    /// 刷新所有chunk
    /// </summary>
    public void RefreshAllChunk()
    {
        foreach (var itemChunk in dicChunk)
        {
            itemChunk.Value.BuildChunk();
        }
    }
}