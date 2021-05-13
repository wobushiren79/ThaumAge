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
    public Dictionary<Vector3Int, Chunk> dicChunk = new Dictionary<Vector3Int, Chunk>();

    //存储着所有的材质
    public Material[] arrayBlockMat = new Material[16];

    //所有待修改的方块
    protected ConcurrentQueue<BlockBean> listUpdateBlock = new ConcurrentQueue<BlockBean>();
    //所有待修改的区块
    protected ConcurrentQueue<Chunk> listUpdateChunk = new ConcurrentQueue<Chunk>();
    //待绘制的区块
    protected List<Chunk> listUpdateDrawChunk = new List<Chunk>();

    //世界种子
    protected int worldSeed;

    public int widthChunk = 16;
    public int heightChunk = 256;
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

    protected void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForUpdateChunk(null);
        }
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
    /// 增加需要更新的区块
    /// </summary>
    /// <param name="chunk"></param>
    public void AddUpdateChunk(Chunk chunk)
    {
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
            listUpdateDrawChunk.Add(chunk);
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
        Vector3Int chunkPosition = GetChunkPositionForWorldPosition(pos);
        return GetChunk(chunkPosition);
    }

    public Vector3Int GetChunkPositionForWorldPosition(Vector3Int pos)
    {
        int halfWidth = widthChunk / 2;
        int posX;
        int posZ;
        if (pos.x < 0)
        {
            posX = Mathf.FloorToInt((pos.x - halfWidth + 1) / widthChunk) * widthChunk;
        }
        else
        {
            posX = Mathf.FloorToInt((pos.x + halfWidth) / widthChunk) * widthChunk;
        }
        if (pos.z < 0)
        {
            posZ = Mathf.FloorToInt((pos.z - halfWidth + 1) / widthChunk) * widthChunk;
        }
        else
        {
            posZ = Mathf.FloorToInt((pos.z + halfWidth) / widthChunk) * widthChunk;
        }
        return new Vector3Int(posX, 0, posZ);
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    /// <param name="pos">世界坐标</param>
    /// <returns></returns>
    public Block GetBlockForWorldPosition(Vector3Int pos)
    {
        Chunk chunk = GetChunkForWorldPosition(pos);
        if (chunk == null)
            return null;
        return chunk.GetBlockForWorld(pos);
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
            Block itemBlock = chunk.GetBlockForWorld(new Vector3Int(x, y, z));
            if (itemBlock == null)
                continue;
            if (itemBlock.blockData.GetBlockType() != BlockTypeEnum.None && y > maxHeight)
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
    /// 异步创建区块方块数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="callBack"></param>
    public async void CreateChunkBlockDataForAsync(Chunk chunk, Action callBack)
    {
        //初始化Map
        BiomeManager biomeManager = BiomeHandler.Instance.manager;
        BlockManager blockManager = BlockHandler.Instance.manager;
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;

        await Task.Run(() =>
        {
            try
            {
                //生成基础地形数据
                HandleForBaseBlock(chunk);
                //处理存档方块 优先使用存档方块
                HandleForLoadBlock(chunk);
            }
            catch (Exception e)
            {
                LogUtil.Log(e.ToString());
            }
        });
        callBack?.Invoke();
    }



    /// <summary>
    /// 处理 待更新区块
    /// </summary>
    /// <param name="callBack"></param>
    public void HandleForUpdateChunk(Action callBack)
    {
        if (listUpdateChunk.Count > 0)
        {
            if (listUpdateChunk.TryDequeue(out Chunk updateChunk))
            {
                if (updateChunk != null)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateDrawChunk(updateChunk);
                    //构建修改过的区块
                    updateChunk.BuildChunkForAsync(null);
                }
            }
        }
        else
        {
            if (listUpdateDrawChunk.Count > 0)
            {
                Chunk updateDrawChunk = listUpdateDrawChunk[0];
                if (updateDrawChunk != null)
                {
                    if (!updateDrawChunk.isBake)
                    {
                        //构建修改过的区块
                        updateDrawChunk.RefreshMesh();
                        listUpdateDrawChunk.RemoveAt(0);
                        //刷新寻路
                        PathFindingHandler.Instance.manager.RefreshPathFinding(updateDrawChunk);
                    }
                }
                else
                {
                    listUpdateDrawChunk.RemoveAt(0);
                }
            }
            else
            {
                callBack?.Invoke();
            }
        }
    }

    /// <summary>
    /// 处理-基础地形方块
    /// </summary>
    /// <param name="chunk"></param>
    public void HandleForBaseBlock(Chunk chunk)
    {
        int halfWidth = widthChunk / 2;
        //获取该世界的所有生态
        List<Biome> listBiome = BiomeHandler.Instance.manager.GetBiomeListByWorldType(worldType);
        //获取一定范围内的生态点
        List<Vector3Int> listBiomeCenter = BiomeHandler.Instance.GetBiomeCenterPosition(chunk, 5, 10);

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
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(chunk, listBiomeCenter, listBiome, position);
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
    }

    /// <summary>
    /// 处理 更新方块
    /// </summary>
    public async void HandleForUpdateBlock(Action callBack)
    {
        if (listUpdateBlock.Count <= 0)
        {
            callBack?.Invoke();
            return;
        }
        await Task.Run(() =>
        {

            List<BlockBean> listNoChunkBlock = new List<BlockBean>();
            //添加修改的方块信息，用于树木或建筑群等用于多个区块的数据     
            while (listUpdateBlock.TryDequeue(out BlockBean itemBlock))
            {
                if (itemBlock == null || itemBlock.worldPosition == null)
                {
                    continue;
                }
                Vector3Int positionBlockWorld = itemBlock.worldPosition.GetVector3Int();
                Chunk chunk = GetChunkForWorldPosition(positionBlockWorld);
                if (chunk != null && chunk.isInit)
                {
                    Vector3Int positionBlockLocal = itemBlock.worldPosition.GetVector3Int() - chunk.worldPosition;
                    //需要重新设置一下本地坐标 之前没有记录本地坐标
                    itemBlock.localPosition = new Vector3IntBean(positionBlockLocal);
                    //获取保存的数据
                    WorldDataBean worldData = chunk.GetWorldData();
                    if (worldData == null || worldData.chunkData == null || worldData.chunkData.GetBlockData(positionBlockLocal) == null)
                    {
                        //设置方块
                        chunk.SetBlock(itemBlock, false, false, false);
                        //添加需要更新的chunk
                        AddUpdateChunk(chunk);
                    }
                }
                else
                {
                    listNoChunkBlock.Add(itemBlock);
                }
            }
            for (int i = 0; i < listNoChunkBlock.Count; i++)
            {
                AddUpdateBlock(listNoChunkBlock[i]);
            }

        });
        callBack?.Invoke();
    }

    /// <summary>
    /// 处理 读取的方块
    /// </summary>
    public void HandleForLoadBlock(Chunk chunk)
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();

        WorldDataBean worldData = gameDataManager.GetWorldData(userData.userId, WorldTypeEnum.Main, chunk.worldPosition);

        //如果没有世界数据 则创建一个
        if (worldData == null)
        {
            worldData = new WorldDataBean();
            worldData.workdType = (int)WorldTypeEnum.Main;
            worldData.userId = userData.userId;
        }
        Dictionary<Vector3Int, BlockBean> dicBlockData = new Dictionary<Vector3Int, BlockBean>();
        //如果有数据 则读取数据
        if (worldData.chunkData != null)
        {
            worldData.chunkData.InitData();
            dicBlockData = worldData.chunkData.dicBlockData;
        }
        else
        {
            worldData.chunkData = new ChunkBean();
            worldData.chunkData.position = new Vector3IntBean(chunk.worldPosition);
        }
        foreach (var itemData in dicBlockData)
        {
            BlockBean blockData = itemData.Value;
            //生成方块
            Block block = BlockHandler.Instance.CreateBlock(chunk, blockData);
            Vector3Int positionBlock = blockData.localPosition.GetVector3Int();
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