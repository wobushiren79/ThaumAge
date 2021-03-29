﻿using System;
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

    //所有待修改的方块
    public List<BlockBean> listUpdateBlock = new List<BlockBean>();

    //世界种子
    public int worldSeed;

    public int widthChunk = 16;
    public int heightChunk = 256;


    /// <summary>
    /// 处理 更新方块
    /// </summary>
    public void HandleForUpdateBlock()
    {
        if (listUpdateBlock.Count <= 0)
            return;
        //添加修改的方块信息，用于树木或建筑群等用于多个区块的数据
        List<Chunk> listUpdateChunk = new List<Chunk>();
        for (int i = 0; i < listUpdateBlock.Count; i++)
        {
            BlockBean itemBlock = listUpdateBlock[i];
            Vector3Int positionBlock = itemBlock.worldPosition.GetVector3Int();
            Chunk chunk = GetChunkForWorldPosition(positionBlock);
            if (chunk != null)
            {
                //生成方块
                Block block = BlockHandler.Instance.CreateBlock(chunk, itemBlock);
                if (!chunk.mapForBlock.ContainsKey(positionBlock))
                {
                    chunk.mapForBlock.Add(positionBlock, block);
                    listUpdateChunk.Add(chunk);
                }
                //从更新列表中移除
                listUpdateBlock.Remove(itemBlock);
                i--;
            }
        }
        //构建修改过的区块
        for (int i = 0; i < listUpdateBlock.Count; i++)
        {
            Chunk itemChunk = listUpdateChunk[i];
            itemChunk.BuildChunkForAsync();
        }
    }

    /// <summary>
    /// 处理 读取的方块
    /// </summary>
    public void HandleForLoadBlock(Chunk chunk)
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        Vector3Int chunkPosition = Vector3Int.CeilToInt(chunk.transform.position);
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();
        WorldDataBean worldData = gameDataManager.GetWorldData(userData.userId, WorldTypeEnum.Main, chunkPosition);
        //如果没有世界数据 则创建一个
        if (worldData == null)
        {
            worldData = new WorldDataBean();
            worldData.workdType = (int)WorldTypeEnum.Main;
            worldData.userId = userData.userId;
        }
        Dictionary<string, BlockBean> dicBlockData = new Dictionary<string, BlockBean>();
        //如果有数据 则读取数据
        if (worldData.chunkData != null)
        {
            dicBlockData = worldData.chunkData.dicBlockData;
        }
        else
        {
            worldData.chunkData = new ChunkBean();
            worldData.chunkData.position = new Vector3IntBean(chunkPosition);
        }
        foreach (var itemData in dicBlockData)
        {
            BlockBean blockData = itemData.Value;
            //生成方块
            Block block = BlockHandler.Instance.CreateBlock(chunk, blockData);
            Vector3Int positionBlock = blockData.position.GetVector3Int();
            //添加方块 如果已经有该方块 则先删除，优先使用存档的方块
            if (chunk.mapForBlock.ContainsKey(positionBlock))
            {
                chunk.mapForBlock.Remove(positionBlock);
            }
            chunk.mapForBlock.Add(positionBlock, block);
        }
        chunk.SetWorldData(worldData);
    }

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

    public Chunk GetChunkForWorldPosition(Vector3Int pos)
    {
        int halfWidth = widthChunk / 2;

        int posX = (int)Mathf.Floor((pos.x - halfWidth) / (float)widthChunk) * widthChunk;
        int posZ = (int)Mathf.Floor((pos.z - halfWidth) / (float)widthChunk) * widthChunk;

        return GetChunk(new Vector3Int(posX, 0, posZ));
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
    /// 异步创建区块方块数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="callBack"></param>
    public async void CreateChunkBlockDataForAsync(Chunk chunk, Action callBack)
    {
        //初始化Map
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
            //chunk.mapForBlock.Clear();
            //遍历map，生成其中每个Block的信息 
            //生成基础地形数据
            for (int x = 0; x < widthChunk; x++)
            {
                for (int y = 0; y < heightChunk; y++)
                {
                    for (int z = 0; z < widthChunk; z++)
                    {
                        Vector3Int position = new Vector3Int(x - halfWidth, y, z - halfWidth);

                        //获取方块类型
                        BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(chunk, listBiome, position);
                        //生成方块
                        Block block = BlockHandler.Instance.CreateBlock(chunk, position, blockType);
                        //TODO 还可以检测方块的优先级
                        if (!chunk.mapForBlock.ContainsKey(position))
                        {
                            //添加方块
                            chunk.mapForBlock.Add(position, block);
                        }
                    }
                }
            }
            //处理更新方块
            HandleForUpdateBlock();
            //处理存档方块 优先使用存档方块
            HandleForLoadBlock(chunk);
        });

        callBack?.Invoke();
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