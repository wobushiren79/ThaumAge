using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //模型列表
    public Dictionary<string, GameObject> dicModel = new Dictionary<string, GameObject>();

    //存储着世界中所有的Chunk
    public Dictionary<long, Chunk> dicChunk = new Dictionary<long, Chunk>();

    //所有待修改的方块
    public ConcurrentQueue<BlockTempBean> listUpdateBlock = new ConcurrentQueue<BlockTempBean>();
    //所有待修改的区块
    public ConcurrentQueue<Chunk> listUpdateChunk = new ConcurrentQueue<Chunk>();

    //待绘制的区块 用于角色修改
    public Queue<Chunk> listUpdateDrawChunkEditor = new Queue<Chunk>();
    //待绘制的区块 用于场景初始化
    public Queue<Chunk> listUpdateDrawChunkInit = new Queue<Chunk>();

    //世界种子
    protected int worldSeed;

    public int widthChunk = 16;
    public int heightChunk = 256;
    //世界大小
    public int worldSize = 10000;
    //世界范围
    public int worldRefreshRange = 1;

    public WorldTypeEnum worldType = WorldTypeEnum.Main;

    protected static object lockForUpdateBlock = new object();

    public static string pathForChunk = "Assets/Prefabs/Game/Chunk.prefab";


    /// <summary>
    /// 加载资源
    /// </summary>
    public void LoadResources(Action callBack)
    {
        //加载区块模型
        GetModelForAddressables(dicModel, pathForChunk, (data) =>
        {
            callBack?.Invoke();
        });
    }

    /// <summary>
    /// 获取区块模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetChunkModel()
    {
        if (dicModel.TryGetValue(pathForChunk, out GameObject objChunkModel)) 
        {
            return objChunkModel;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 清除所有区块
    /// </summary>
    public void ClearAllChunk()
    {
        foreach (var itemChunk in dicChunk)
        {
            Destroy(itemChunk.Value.gameObject);
        }
        dicChunk.Clear();
    }

    /// <summary>
    /// 移除区块
    /// </summary>
    /// <param name="position"></param>
    /// <param name="chunk"></param>
    public void RemoveChunk(Vector3Int position, Chunk chunk)
    {
        if (chunk.isInit)
        {
            int index = MathUtil.GetSingleIndexForTwo(position.x / widthChunk, position.z / widthChunk, worldSize);
            dicChunk.Remove(index);
            Destroy(chunk.gameObject);
        }
    }

    /// <summary>
    /// 增加区块
    /// </summary>
    /// <param name="chunk"></param>
    public void AddChunk(Vector3Int position, Chunk chunk)
    {
        int index = MathUtil.GetSingleIndexForTwo(position.x / widthChunk, position.z / widthChunk, worldSize);
        dicChunk.Add(index, chunk);
    }

    /// <summary>
    /// 增加需要更新的区块
    /// </summary>
    /// <param name="chunk"></param>
    public void AddUpdateChunk(Chunk chunk)
    {
        if (chunk == null)
            return;
        if (!listUpdateChunk.Contains(chunk))
        {
            listUpdateChunk.Enqueue(chunk);
        }
    }

    /// <summary>
    /// 增加待绘制区块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="type">0场景创建 1创景编辑</param>
    public void AddUpdateDrawChunk(Chunk chunk, int type)
    {
        if (type == 0)
        {
            if (!listUpdateDrawChunkInit.Contains(chunk))
            {
                listUpdateDrawChunkInit.Enqueue(chunk);
            }
        }
        else if (type == 1)
        {
            if (!listUpdateDrawChunkEditor.Contains(chunk))
            {
                listUpdateDrawChunkEditor.Enqueue(chunk);
            }
        }
    }

    /// <summary>
    /// 增加需要更新的方块
    /// </summary>
    /// <param name="blockData"></param>
    public void AddUpdateBlock(BlockTempBean blockData)
    {
        listUpdateBlock.Enqueue(blockData);
    }


    /// <summary>
    /// 获取区块
    /// </summary>
    /// <param name="position">区块的坐标</param>
    /// <returns></returns>
    public Chunk GetChunk(Vector3Int position)
    {
        return GetChunk(position.x, position.z);
    }

    public Chunk GetChunk(int x, int z)
    {
        int index = MathUtil.GetSingleIndexForTwo(x / widthChunk, z / widthChunk, worldSize);
        if (dicChunk.TryGetValue(index, out Chunk value))
        {
            return value;
        }
        return null;
    }


    /// <summary>
    /// 通过随意一个世界坐标 获取chunk
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Chunk GetChunkForWorldPosition(Vector3Int pos)
    {
        Vector3Int chunkPosition = GetChunkPositionForWorldPosition(pos);
        return GetChunk(chunkPosition);
    }

    public Chunk GetChunkForWorldPosition(int x, int z)
    {
        GetChunkPositionForWorldPosition(x, z, out int outX, out int outZ);
        return GetChunk(outX, outZ);
    }

    /// <summary>
    /// 通过随意一个世界坐标 获取chunk的世界坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3Int GetChunkPositionForWorldPosition(Vector3Int pos)
    {
        int posX;
        int posZ;
        posX = Mathf.FloorToInt((float)pos.x / widthChunk) * widthChunk;
        posZ = Mathf.FloorToInt((float)pos.z / widthChunk) * widthChunk;
        return new Vector3Int(posX, 0, posZ);
    }

    public void GetChunkPositionForWorldPosition(int x, int z, out int outX, out int outZ)
    {
        outX = Mathf.FloorToInt((float)x / widthChunk) * widthChunk;
        outZ = Mathf.FloorToInt((float)z / widthChunk) * widthChunk;
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    /// <param name="pos">世界坐标</param>
    /// <returns></returns>
    public void GetBlockForWorldPosition(Vector3Int pos, out Block block, out DirectionEnum direction, out Chunk chunk)
    {
        chunk = GetChunkForWorldPosition(pos);
        if (chunk == null)
        {
            block = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            direction = DirectionEnum.UP;
            return;
        }
        chunk.chunkData.GetBlockForLocal(pos - chunk.chunkData.positionForWorld, out block, out direction);
    }

    public void GetBlockForWorldPosition(Vector3Int pos, out Block block, out Chunk chunk)
    {
        chunk = GetChunkForWorldPosition(pos);
        if (chunk == null)
        {
            block = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            return;
        }
        chunk.GetBlockForWorld(pos, out block, out bool isInside);
    }

    /// <summary>
    /// 获取世界种子
    /// </summary>
    /// <returns></returns>
    public int GetWorldSeed()
    {
        return worldSeed;
    }

    /// <summary>
    /// 获取某一点的最高坐标
    /// </summary>
    /// <param name="pos"></param>
    public int GetMaxHeightForWorldPosition(int x, int z)
    {
        Chunk chunk = GetChunkForWorldPosition(new Vector3Int(x, 0, z));
        if (chunk == null)
            return 0;
        int maxHeight = int.MinValue;
        for (int y = 0; y < heightChunk; y++)
        {
            chunk.chunkData.GetBlockForLocal(new Vector3Int(x, y, z) - chunk.chunkData.positionForWorld, out Block block, out DirectionEnum direction);
            if (block == null || block.blockType == BlockTypeEnum.None)
                continue;
            if (block.blockType != BlockTypeEnum.None && y > maxHeight)
            {
                maxHeight = y;
            }
        }
        return maxHeight;
    }

    /// <summary>
    /// 设置世界种子
    /// </summary>
    /// <param name="worldSeed"></param>
    public void SetWorldSeed(int worldSeed)
    {
        this.worldSeed = worldSeed;
        //初始化随机种子
        //UnityEngine.Random.InitState(worldSeed);
        //初始化生态种子
        BiomeHandler.Instance.InitWorldBiomeSeed();
        //初始化随机种子
        WorldRandTools.Randomize(worldSeed);
    }

    /// <summary>
    /// 刷新所有chunk
    /// </summary>
    public void RefreshAllChunk()
    {
        foreach (var itemChunk in dicChunk.Values)
        {
            itemChunk.BuildChunkForAsync(null);
        }
    }
}