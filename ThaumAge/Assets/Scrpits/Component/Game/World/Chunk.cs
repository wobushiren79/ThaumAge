using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class Chunk : BaseMonoBehaviour
{
    public class ChunkRenderData
    {
        //普通使用的三角形合集
        public List<Vector3> verts = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();

        //碰撞使用的三角形合集
        public List<Vector3> vertsCollider = new List<Vector3>();
        public List<int> trisCollider = new List<int>();

        //出发使用的三角形合集
        public List<Vector3> vertsTrigger = new List<Vector3>();
        public List<int> trisTrigger = new List<int>();

        //所有三角形合集，根据材质球区分
        public Dictionary<BlockMaterialEnum, List<int>> dicTris = new Dictionary<BlockMaterialEnum, List<int>>();
    }

    public Action eventUpdate;

    public MeshCollider meshCollider;
    public MeshCollider meshTrigger;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    //存储着此Chunk内的所有Block信息
    public Block[] mapForBlock;
    //待更新方块
    public Queue<BlockBean> listUpdateBlock = new Queue<BlockBean>();
    //大小
    public int width = 0;
    public int height = 0;
    //是否初始化
    public bool isInit = false;
    public bool isBuildChunk = false;
    public bool isDrawMesh = false;
    protected bool isFirstDraw = true;
    protected bool isAnimForInit = false;
    //世界坐标
    public Vector3Int worldPosition;

    //渲染数据
    protected ChunkRenderData chunkRenderData;
    //存储数据
    protected WorldDataBean worldData;

    public Mesh chunkMesh;
    public Mesh chunkMeshCollider;
    public Mesh chunkMeshTrigger;

    protected static object lockForUpdateBlcok = new object();

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        chunkRenderData = new ChunkRenderData();
        meshRenderer.materials = WorldCreateHandler.Instance.manager.GetAllMaterial();

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();
        chunkMeshTrigger = new Mesh();

        //设置为动态变更，理论上可以提高效率
        chunkMesh.MarkDynamic();
        chunkMeshCollider.MarkDynamic();
        chunkMeshTrigger.MarkDynamic();

        //设置mesh的三角形上限
        chunkMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshCollider.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshTrigger.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        meshFilter.sharedMesh = chunkMesh;
        meshCollider.sharedMesh = chunkMeshCollider;
        meshTrigger.sharedMesh = chunkMeshTrigger;

        //设置mesh的三角形上限
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshTrigger.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
        Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);
    }

    protected float eventUpdateTime = 0;

    private void Update()
    {
        if (!isInit)
            return;
        eventUpdateTime -= Time.deltaTime;
        if (eventUpdateTime < 0)
        {
            eventUpdateTime = 1;
            eventUpdate?.Invoke();
            HandleForUpdateBlock();
        }
    }

    /// <summary>
    /// 增加待更新方块
    /// </summary>
    /// <param name="blockData"></param>
    public void AddUpdateBlock(BlockBean blockData)
    {
        listUpdateBlock.Enqueue(blockData);
    }

    /// <summary>
    /// 处理-更新的方块
    /// </summary>
    public void HandleForUpdateBlock()
    {
        if (listUpdateBlock.Count > 0)
        {
            while (listUpdateBlock.Count > 0)
            {
                BlockBean itemBlock = listUpdateBlock.Dequeue();
                Vector3Int positionBlockWorld = itemBlock.worldPosition.GetVector3Int();
                Vector3Int positionBlockLocal = positionBlockWorld - worldPosition;
                //需要重新设置一下本地坐标 之前没有记录本地坐标
                itemBlock.localPosition = new Vector3IntBean(positionBlockLocal);
                //设置方块
                SetBlock(itemBlock, false, false, false);
            }
            //异步保存数据
            GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
            //异步刷新
            AddUpdateChunkForRange();
            WorldCreateHandler.Instance.HandleForUpdateChunk(false,null);
        }
    }

    /// <summary>
    /// 获取存储数据
    /// </summary>
    /// <returns></returns>
    public WorldDataBean GetWorldData()
    {
        return worldData;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="mapForBlock"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void SetData(Vector3Int worldPosition, int width, int height)
    {
        this.worldPosition = worldPosition;
        this.width = width;
        this.height = height;
        mapForBlock = new Block[width * height * width];
    }

    /// <summary>
    /// 设置世界数据 用于保存
    /// </summary>
    /// <param name="worldData"></param>
    public void SetWorldData(WorldDataBean worldData)
    {
        this.worldData = worldData;
    }


    /// <summary>
    /// 增加四周待更新的区块
    /// </summary>
    public void AddUpdateChunkForRange()
    {
        Chunk leftChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(-width, 0, 0));
        Chunk rightChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(width, 0, 0));
        Chunk upChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, width));
        Chunk downChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, -width));
        WorldCreateHandler.Instance.manager.AddUpdateChunk(leftChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(rightChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(upChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(downChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(this);
    }

    public void AddUpdateChunkForRange(Block changeBlock)
    {
        Chunk leftChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(changeBlock.worldPosition - new Vector3Int(-1, 0, 0));
        Chunk rightChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(changeBlock.worldPosition - new Vector3Int(1, 0, 0));
        Chunk upChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(changeBlock.worldPosition - new Vector3Int(0, 0, 1));
        Chunk downChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(changeBlock.worldPosition - new Vector3Int(0, 0, -1));
        if (leftChunk != this)
            WorldCreateHandler.Instance.manager.AddUpdateChunk(leftChunk);
        if (rightChunk != this)
            WorldCreateHandler.Instance.manager.AddUpdateChunk(rightChunk);
        if (upChunk != this)
            WorldCreateHandler.Instance.manager.AddUpdateChunk(upChunk);
        if (downChunk != this)
            WorldCreateHandler.Instance.manager.AddUpdateChunk(downChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(this);
    }

    /// <summary>
    /// 异步创建区块方块数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="callBack"></param>
    public async void BuildChunkBlockDataForAsync(Action callBackForComplete)
    {
        //初始化Map
        BiomeManager biomeManager = BiomeHandler.Instance.manager;
        BlockManager blockManager = BlockHandler.Instance.manager;
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;

        await Task.Run(() =>
        {
            try
            {
                lock (lockForUpdateBlcok)
                {
#if UNITY_EDITOR
                    Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
#endif
                    //生成基础地形数据      
                    HandleForBaseBlock();
                    //处理存档方块 优先使用存档方块
                    HandleForLoadBlock();
                    //设置数据
                    AddUpdateChunkForRange();
                    //初始化完成
                    isInit = true;
#if UNITY_EDITOR
                    TimeUtil.GetMethodTimeEnd("Time_BuildChunkBlockDataForAsync:", stopwatch);
#endif
                }
            }
            catch (Exception e)
            {
                LogUtil.Log("CreateChunkBlockDataForAsync:" + e.ToString());
            }
        });
        callBackForComplete?.Invoke();
    }


    /// <summary>
    /// 异步构建chunk
    /// </summary>
    public async void BuildChunkForAsync(Action callBackForComplete)
    {
        //只有初始化之后的chunk才能刷新
        if (!isInit || mapForBlock.Length <= 0)
        {
            isBuildChunk = false;
            callBackForComplete?.Invoke();
            return;
        }
        if (isBuildChunk)
            return;
        isBuildChunk = true;
        await Task.Run(() =>
        {
            //遍历chunk, 生成其中的每一个Block
            try
            {
                lock (lockForUpdateBlcok)
                {
#if UNITY_EDITOR
                    Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
#endif
                    chunkRenderData = new ChunkRenderData();
                    //初始化数据
                    List<BlockMaterialEnum> blockMaterialsEnum = EnumUtil.GetEnumValue<BlockMaterialEnum>();
                    for (int i = 0; i < blockMaterialsEnum.Count; i++)
                    {
                        BlockMaterialEnum blockMaterial = blockMaterialsEnum[i];
                        chunkRenderData.dicTris.Add(blockMaterial, new List<int>());
                    }
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int z = 0; z < width; z++)
                            {
                                Block block = mapForBlock[GetIndexByPosition(x, y, z)];
                                if (block == null || block.blockType == BlockTypeEnum.None)
                                    continue;
                                block.BuildBlock(chunkRenderData);
                            }
                        }
                    }
#if UNITY_EDITOR
                    TimeUtil.GetMethodTimeEnd("Time_BuildChunkForAsync:", stopwatch);
#endif
                }

            }
            catch (Exception e)
            {
                LogUtil.Log("BuildChunkForAsync:" + e.ToString());
            }
            finally
            {

            }
        });
        isBuildChunk = false;
        callBackForComplete?.Invoke();
    }

    /// <summary>
    /// 刷新网格
    /// </summary>
    public void DrawMesh()
    {
        if (isBuildChunk)
            return;
        try
        {
            isDrawMesh = true;

            chunkMesh.Clear();
            chunkMeshCollider.Clear();
            chunkMeshTrigger.Clear();

            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //定点数判断
            if (chunkRenderData == null || chunkRenderData.verts.Count <= 3)
            {
                isDrawMesh = false;
                return;
            }

            //设置顶点
            chunkMesh.SetVertices(chunkRenderData.verts);
            //设置UV
            chunkMesh.SetUVs(0, chunkRenderData.uvs);
            //设置三角（单面渲染，双面渲染,液体）
            foreach (var itemTris in chunkRenderData.dicTris)
            {
                chunkMesh.SetTriangles(itemTris.Value.ToArray(), (int)itemTris.Key);
            }

            //碰撞数据设置
            chunkMeshCollider.SetVertices(chunkRenderData.vertsCollider);
            chunkMeshCollider.SetTriangles(chunkRenderData.trisCollider, 0);

            //出发数据设置
            chunkMeshTrigger.SetVertices(chunkRenderData.vertsTrigger);
            chunkMeshTrigger.SetTriangles(chunkRenderData.trisTrigger, 0);

            //刷新
            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();
            //刷新
            //chunkMeshCollider.RecalculateBounds();
            //chunkMeshCollider.RecalculateNormals();
            //刷新
            //chunkMeshTrigger.RecalculateBounds();
            //chunkMeshTrigger.RecalculateNormals();

            //meshFilter.mesh.Optimize();
            meshFilter.sharedMesh = chunkMesh;
            meshCollider.sharedMesh = chunkMeshCollider;
            meshTrigger.sharedMesh = chunkMeshTrigger;

            //Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
            //Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);

            //初始化动画
            AnimForInit(() =>
            {
                //刷新寻路
                PathFindingHandler.Instance.manager.RefreshPathFinding(this);
            });
        }
        catch (Exception e)
        {
            LogUtil.Log("绘制出错_" + e.ToString());
            isDrawMesh = false;
        }
        finally
        {
            isDrawMesh = false;
        }

    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit(Action callBack)
    {
        if (isFirstDraw)
        {
            isFirstDraw = false;
            //动画
            transform.DOLocalMoveY(-50, 1).From().OnComplete(() =>
            {
                callBack?.Invoke();
                isAnimForInit = true;
            });
        }
        else
        {
            if (isAnimForInit)
                callBack?.Invoke();
        }
    }

    public void GetBlockForWorld(Vector3Int blockWorldPosition, out Block block, out bool isInside)
    {
        GetBlockForLocal(blockWorldPosition - worldPosition, out block, out isInside);
    }

    public void GetBlockForLocal(Vector3Int localPosition, out Block block, out bool isInside)
    {
        if (localPosition.y < 0 || localPosition.y > height - 1)
        {
            block = null;
            isInside = false;
            return;
        }
        //当前位置是否在Chunk内
        if ((localPosition.x < 0) || (localPosition.z < 0) || (localPosition.x >= width) || (localPosition.z >= width))
        {
            Vector3Int blockWorldPosition = localPosition + worldPosition;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out block, out bool hasChunk);
            isInside = false;
            return;
        }
        else
        {
            isInside = true;
            int index = GetIndexByPosition(localPosition);
            block = mapForBlock[index];
        }
    }


    /// <summary>
    /// 移除方块
    /// </summary>
    /// <param name="position"></param>
    public void RemoveBlockForWorld(Vector3Int worldPosition)
    {
        SetBlockForWorld(worldPosition, BlockTypeEnum.None);
    }
    public void RemoveBlockForLocal(Vector3Int localPosition)
    {
        SetBlockForLocal(localPosition, BlockTypeEnum.None);
    }


    /// <summary>
    /// 设置方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public Block SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType)
    {
        Vector3Int blockLocalPosition = worldPosition - this.worldPosition;
        return SetBlockForLocal(blockLocalPosition, blockType);
    }

    public Block SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        Vector3Int blockLocalPosition = worldPosition - this.worldPosition;
        return SetBlockForLocal(blockLocalPosition, blockType, direction);
    }

    public Block SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType)
    {
        BlockBean blockData = new BlockBean(blockType, localPosition, localPosition + worldPosition);
        return SetBlock(blockData, true, true, true);
    }

    public Block SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        BlockBean blockData = new BlockBean(blockType, localPosition, localPosition + worldPosition, direction);
        return SetBlock(blockData, true, true, true);
    }

    public Block SetBlock(BlockBean blockData, bool isSaveData, bool isRefreshChunkRange, bool isRefreshBlockRange)
    {
        Vector3Int localPosition = blockData.localPosition.GetVector3Int();
        //首先移除方块

        //添加数据
        Block newBlock = BlockHandler.Instance.CreateBlock(this, blockData);
        if (localPosition.x >= width || localPosition.x < 0
            || localPosition.y >= height || localPosition.y < 0
            || localPosition.z >= width || localPosition.z < 0)
        {
            return null;
        }
        mapForBlock[GetIndexByPosition(localPosition)] = newBlock;

        //刷新六个方向的方块
        if (isRefreshBlockRange)
        {
            newBlock.RefreshBlockRange();
        }

        //是否实时刷新
        if (isRefreshChunkRange)
        {
            //异步构建chunk
            AddUpdateChunkForRange(newBlock);
        }

        if (isSaveData)
        {
            //保存数据
            if (worldData != null && worldData.chunkData != null)
            {
                ChunkBean chunkData = worldData.chunkData;
                int index = GetIndexByPosition(localPosition);
                if (chunkData.dicBlockData.ContainsKey(index))
                {
                    chunkData.dicBlockData[index] = newBlock.blockData;
                }
                else
                {
                    chunkData.dicBlockData.Add(index, newBlock.blockData);
                }
            }
            //异步保存数据
            GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
        }

        return newBlock;
    }

    /// <summary>
    /// 处理 读取的方块
    /// </summary>
    public void HandleForLoadBlock()
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();

        WorldDataBean worldData = gameDataManager.GetWorldData(userData.userId, WorldTypeEnum.Main, worldPosition);

        //如果没有世界数据 则创建一个
        if (worldData == null)
        {
            worldData = new WorldDataBean();
            worldData.workdType = (int)WorldTypeEnum.Main;
            worldData.userId = userData.userId;
        }
        Dictionary<int, BlockBean> dicBlockData = new Dictionary<int, BlockBean>();
        //如果有数据 则读取数据
        if (worldData.chunkData != null)
        {
            worldData.chunkData.InitData();
            dicBlockData = worldData.chunkData.dicBlockData;
        }
        else
        {
            worldData.chunkData = new ChunkBean();
            worldData.chunkData.position = new Vector3IntBean(worldPosition);
        }
        foreach (var itemData in dicBlockData)
        {
            BlockBean blockData = itemData.Value;
            //生成方块
            Block block = BlockHandler.Instance.CreateBlock(this, blockData);
            Vector3Int positionBlock = blockData.localPosition.GetVector3Int();
            //添加方块 如果已经有该方块 则先删除，优先使用存档的方块
            mapForBlock[GetIndexByPosition(positionBlock)] = block;
        }
        SetWorldData(worldData);
    }

    /// <summary>
    /// 处理-基础地形方块
    /// </summary>
    /// <param name="chunk"></param>
    public void HandleForBaseBlock()
    {
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        //获取该世界的所有生态
        List<Biome> listBiome = BiomeHandler.Instance.manager.GetBiomeListByWorldType(worldType);
        //获取一定范围内的生态点
        List<Vector3Int> listBiomeCenter = BiomeHandler.Instance.GetBiomeCenterPosition(this, 5, 10);
        //遍历map，生成其中每个Block的信息 
        //生成基础地形数据
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);

                    //获取方块类型
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(this, listBiomeCenter, listBiome, position);

                    //如果是空 则跳过
                    if (blockType == BlockTypeEnum.None)
                        continue;

                    //生成方块
                    Block block = BlockHandler.Instance.CreateBlock(this, blockType, position);

                    //添加方块
                    mapForBlock[GetIndexByPosition(x, y, z)] = block;
                }
            }
        }
    }

    public int GetIndexByPosition(Vector3Int position)
    {
        return GetIndexByPosition(position.x, position.y, position.z);
    }
    public int GetIndexByPosition(int x, int y, int z)
    {
        return x * width * height + y * width + z;
    }


    #region 事件注册
    public void RegisterEventUpdate(Action action)
    {
        eventUpdate += action;
    }

    public void UnRegisterEventUpdate(Action action)
    {
        eventUpdate -= action;
    }
    #endregion
}