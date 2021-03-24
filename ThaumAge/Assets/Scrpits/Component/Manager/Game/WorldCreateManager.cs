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
        if (dicChunk.TryGetValue(pos, out Chunk value))
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

    public async void CreateChunkBlockDataForAsync(Chunk chunk, Action<Dictionary<Vector3Int, Block>> callBack)
    {
        //初始化Map
        Dictionary<Vector3Int, Block> mapForBlock = new Dictionary<Vector3Int, Block>();
        Vector3Int chunkPosition = Vector3Int.CeilToInt(chunk.transform.position);
        int halfWidth = widthChunk / 2;
        BiomeManager biomeManager = BiomeHandler.Instance.manager;
        BlockManager blockManager = BlockHandler.Instance.manager;
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;

        List<Biome> listBiome = new List<Biome>();
        listBiome.Add(new BiomeDesert());
        listBiome.Add(new BiomeForest());
        listBiome.Add(new BiomePrairie());

        await Task.Run(() =>
        {
            //获取数据中的chunk
            WorldDataBean worldData = gameDataManager.GetWorldData();
            ChunkBean chunkData = null;
            for (int i = 0; i < worldData.listChunkData.Count; i++)
            {
                ChunkBean tempChunkData = worldData.listChunkData[i];
                if (tempChunkData.position.GetVector3Int() == chunkPosition)
                {
                    chunkData = tempChunkData;
                    break;
                }
            }
            Dictionary<Vector3Int, BlockBean> dicBlockData = new Dictionary<Vector3Int, BlockBean>();
            //如果有数据 则读取数据
            if (chunkData != null)
            {
                for (int i = 0; i < chunkData.listBlockData.Count; i++)
                {
                    BlockBean blockData = chunkData.listBlockData[i];
                    dicBlockData.Add(blockData.position.GetVector3Int(), blockData);
                }
            }
            if (chunkData == null)
                chunkData = new ChunkBean();
            //遍历map，生成其中每个Block的信息
            for (int x = 0; x < widthChunk; x++)
            {
                for (int y = 0; y < heightChunk; y++)
                {
                    for (int z = 0; z < widthChunk; z++)
                    {
                        Vector3Int position = new Vector3Int(x - halfWidth, y, z - halfWidth);
                        Block block;
                        if (dicBlockData.TryGetValue(position, out BlockBean value))
                        {
                            //生成方块
                            block = BlockHandler.Instance.CreateBlock(chunk, value);
                        }
                        else
                        {
                            //获取方块类型
                            BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(listBiome, position + chunkPosition, widthChunk, heightChunk);
                            //生成方块
                            block = BlockHandler.Instance.CreateBlock(chunk, position, blockType);
                        }
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
            itemChunk.Value.BuildChunkForAsync();
        }
    }
}