using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Pathfinding;

public class WorldCreateManager : BaseManager
{
    //模型列表
    public Dictionary<string, GameObject> dicModel = new Dictionary<string, GameObject>();

    //存储着世界中所有的Chunk
    public Dictionary<int, Chunk> dicChunk = new Dictionary<int, Chunk>();

    //存储着所有的材质
    public Material[] arrayBlockMat = new Material[16];

    //所有待修改的方块
    public ConcurrentQueue<BlockBean> listUpdateBlock = new ConcurrentQueue<BlockBean>();
    //所有待修改的区块
    public ConcurrentQueue<Chunk> listUpdateChunk = new ConcurrentQueue<Chunk>();
    //待绘制的区块
    public Queue<Chunk> listUpdateDrawChunk = new Queue<Chunk>();

    //世界种子
    protected int worldSeed;

    public int widthChunk = 16;
    public int heightChunk = 256;
    //世界大小
    public int worldSize = 10000;
    //世界范围
    public int worldRefreshRange = 1;

    public WorldTypeEnum worldType = WorldTypeEnum.Main;

    public float time;

    protected static object lockForUpdateBlock = new object();

    protected void Awake()
    {
        List<Material> listData = GetAllModel<Material>("block/mats", "Assets/Prefabs/Mats");
        for (int i = 0; i < listData.Count; i++)
        {
            //按照名字中的下标 确认每个材质球的顺序
            Material itemMat = listData[i];
            string[] nameList = StringUtil.SplitBySubstringForArrayStr(itemMat.name, '_');
            int indexMat = int.Parse(nameList[1]);
            arrayBlockMat[indexMat] = itemMat;
        }
    }

    /// <summary>
    /// 增加区域
    /// </summary>
    /// <param name="chunk"></param>
    public void AddChunk(Vector3Int position, Chunk chunk)
    {
        int index = MathUtil.GetSingleIndexForTwo(position.x/ widthChunk, position.z/ widthChunk, worldSize);
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
    public void AddUpdateDrawChunk(Chunk chunk)
    {
        if (!listUpdateDrawChunk.Contains(chunk))
        {
            listUpdateDrawChunk.Enqueue(chunk);
        }
    }

    /// <summary>
    /// 增加需要更新的方块
    /// </summary>
    /// <param name="blockData"></param>
    public void AddUpdateBlock(BlockBean blockData)
    {
        listUpdateBlock.Enqueue(blockData);
    }

    /// <summary>
    /// 获取所有材质
    /// </summary>
    /// <returns></returns>
    public Material[] GetAllMaterial()
    {
        return arrayBlockMat;
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
    /// <param name="position"></param>
    /// <returns></returns>
    public Chunk GetChunk(Vector3Int position)
    {
        int index = MathUtil.GetSingleIndexForTwo(position.x / widthChunk, position.z / widthChunk, worldSize);
        if (dicChunk.TryGetValue(index, out Chunk value))
        {
            return value;
        }
        return null;
    }

    public Chunk GetChunkForWorldPosition(Vector3Int pos)
    {
        Vector3Int chunkPosition = GetChunkPositionForWorldPosition(pos);
        return GetChunk(chunkPosition);
    }

    public Vector3Int GetChunkPositionForWorldPosition(Vector3Int pos)
    {
        int posX;
        int posZ;
        posX = Mathf.FloorToInt((float)pos.x / widthChunk) * widthChunk;
        posZ = Mathf.FloorToInt((float)pos.z / widthChunk) * widthChunk;
        return new Vector3Int(posX, 0, posZ);
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    /// <param name="pos">世界坐标</param>
    /// <returns></returns>
    public void GetBlockForWorldPosition(Vector3Int pos,out Block block ,out Chunk chunk)
    {
        chunk = GetChunkForWorldPosition(pos);
        if (chunk == null)
        {
            block = null;
            return;
        }
        chunk.GetBlockForWorld(pos, out  block,out bool isInside);
    }
    public void GetBlockForWorldPosition(Vector3Int pos, out Block block, out bool hasChunk)
    {
        GetBlockForWorldPosition(pos, out block, out Chunk chunk);
        if (chunk == null)
        {
            hasChunk = false;
        }
        else
        {
            hasChunk = true;
        }
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
        int maxHeight = 100;
        Chunk chunk = GetChunkForWorldPosition(new Vector3Int(x, 0, z));
        if (chunk == null)
            return maxHeight;
        maxHeight = int.MinValue;
        for (int y = 0; y < heightChunk; y++)
        {
            chunk.GetBlockForWorld(new Vector3Int(x, y, z),out Block itemBlock,out bool isInside);
            if (itemBlock == null)
                continue;
            if (itemBlock.blockType != BlockTypeEnum.None && y > maxHeight)
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
        UnityEngine.Random.InitState(worldSeed);
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