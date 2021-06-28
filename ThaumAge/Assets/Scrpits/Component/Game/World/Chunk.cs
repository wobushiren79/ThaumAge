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

    public List<Action<Chunk>> listEventUpdate = new List<Action<Chunk>>();

    //需要创建方块实力的列表
    public Queue<Vector3Int> listBlockModelUpdate = new Queue<Vector3Int>();
    public Dictionary<Vector3Int, GameObject> dicBlockModel = new Dictionary<Vector3Int, GameObject>();

    public MeshCollider meshCollider;
    public MeshCollider meshTrigger;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    //存储着此Chunk内的所有信息
    public ChunkDataBean chunkData;

    //是否初始化
    public bool isInit = false;
    public bool isBuildChunk = false;
    public bool isDrawMesh = false;
    protected bool isFirstDraw = true;
    protected bool isAnimForInit = false;

    //渲染数据
    protected ChunkRenderData chunkRenderData;
    //存储数据
    protected WorldDataBean worldData;

    public Mesh chunkMesh;
    public Mesh chunkMeshCollider;
    public Mesh chunkMeshTrigger;

    public GameObject objBlockContainer;

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
            HandleForEventUpdate();
        }
        HandleForBlockModelUpdate();
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
        chunkData = new ChunkDataBean(worldPosition, width, height);
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
        Vector3Int worldPosition = chunkData.positionForWorld;
        int chunkWidth = chunkData.chunkWidth;
        Chunk leftChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(-chunkWidth, 0, 0));
        Chunk rightChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(chunkWidth, 0, 0));
        Chunk upChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, chunkWidth));
        Chunk downChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, -chunkWidth));
        WorldCreateHandler.Instance.manager.AddUpdateChunk(leftChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(rightChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(upChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(downChunk);
        WorldCreateHandler.Instance.manager.AddUpdateChunk(this);
    }

    public void AddUpdateChunkForRange(Vector3Int worldPosition)
    {
        Chunk leftChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition - new Vector3Int(-1, 0, 0));
        Chunk rightChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition - new Vector3Int(1, 0, 0));
        Chunk upChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition - new Vector3Int(0, 0, 1));
        Chunk downChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition - new Vector3Int(0, 0, -1));
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
        if (!isInit || chunkData.arrayBlockIds.Length <= 0)
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
                    for (int x = 0; x < chunkData.chunkWidth; x++)
                    {
                        for (int y = 0; y < chunkData.chunkHeight; y++)
                        {
                            for (int z = 0; z < chunkData.chunkWidth; z++)
                            {
                                chunkData.GetBlockForLocal(x, y, z, out BlockTypeEnum blockType, out DirectionEnum direction);
                                if (blockType == BlockTypeEnum.None)
                                    continue;
                                Vector3Int localPosition = new Vector3Int(x, y, z);
                                Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
                                block.SetData(localPosition, localPosition + chunkData.positionForWorld, direction);
                                block.BuildBlock(chunkRenderData);
                                block.InitBlock(this);
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

    public void GetBlockForWorld(Vector3Int blockWorldPosition, out BlockTypeEnum block, out DirectionEnum direction, out bool isInside)
    {
        GetBlockForLocal(blockWorldPosition - chunkData.positionForWorld, out block, out direction, out isInside);
    }


    public void GetBlockForLocal(Vector3Int localPosition, out BlockTypeEnum block, out DirectionEnum direction, out bool isInside)
    {
        if (localPosition.y < 0 || localPosition.y > chunkData.chunkHeight - 1)
        {
            block = BlockTypeEnum.None;
            direction = DirectionEnum.UP;
            isInside = false;
            return;
        }
        //当前位置是否在Chunk内
        if ((localPosition.x < 0) || (localPosition.z < 0) || (localPosition.x >= chunkData.chunkWidth) || (localPosition.z >= chunkData.chunkWidth))
        {
            Vector3Int blockWorldPosition = localPosition + chunkData.positionForWorld;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out block, out direction, out Chunk chunk);
            isInside = false;
            return;
        }
        else
        {
            isInside = true;
            chunkData.GetBlockForLocal(localPosition, out block, out direction);
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
    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType)
    {
        SetBlockForWorld(worldPosition, blockType, DirectionEnum.UP);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        Vector3Int blockLocalPosition = worldPosition - chunkData.positionForWorld;
        SetBlockForLocal(blockLocalPosition, blockType, direction);
    }

    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType)
    {
        SetBlockForLocal(localPosition, blockType, DirectionEnum.UP);
    }

    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        SetBlockForLocal(localPosition, blockType, direction, true, true, true);
    }

    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType, DirectionEnum direction, bool isSaveData, bool isRefreshChunkRange, bool isRefreshBlockRange)
    {
        //首先移除方块
        chunkData.GetBlockForLocal(localPosition, out BlockTypeEnum oldBlockType, out DirectionEnum oldDirection);
        if (oldBlockType != BlockTypeEnum.None)
        {
            Block oldBlock = BlockHandler.Instance.manager.GetRegisterBlock(oldBlockType);
            oldBlock.SetData(localPosition, localPosition + this.chunkData.positionForWorld, oldDirection);
            oldBlock.DestoryBlock(this);
        }
        //设置新方块
        chunkData.SetBlockForLocal(localPosition, blockType, direction);

        Block newBlock = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
        newBlock.SetData(localPosition, localPosition + this.chunkData.positionForWorld, direction);
        newBlock.InitBlock(this);


        //刷新六个方向的方块
        if (isRefreshBlockRange)
        {
            newBlock.SetData(localPosition, chunkData.positionForWorld + localPosition, direction);
            newBlock.RefreshBlockRange();
        }

        //是否实时刷新
        if (isRefreshChunkRange)
        {
            //异步构建chunk
            AddUpdateChunkForRange(localPosition + chunkData.positionForWorld);
        }

        if (isSaveData)
        {
            int index = chunkData.GetIndexByPosition(localPosition);
            //保存数据
            if (worldData != null && worldData.chunkData != null)
            {
                BlockBean blockData = new BlockBean(localPosition, localPosition + chunkData.positionForWorld, blockType, direction);
                if (worldData.chunkData.dicBlockData.ContainsKey(index))
                {
                    worldData.chunkData.dicBlockData[index] = blockData;
                }
                else
                {
                    worldData.chunkData.dicBlockData.Add(index, blockData);
                }
            }
            //异步保存数据
            GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
        }
    }

    /// <summary>
    /// 处理-基础地形方块
    /// </summary>
    /// <param name="chunk"></param>
    public void HandleForBaseBlock()
    {
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        //获取该世界的所有生态
        Biome[] listBiome = BiomeHandler.Instance.manager.GetBiomeListByWorldType(worldType);
        //获取一定范围内的生态点
        Vector3Int[] listBiomeCenter = BiomeHandler.Instance.GetBiomeCenterPosition(this, 5, 10);
        //遍历map，生成其中每个Block的信息 
        //生成基础地形数据
        for (int x = 0; x < chunkData.chunkWidth; x++)
        {
            for (int y = 0; y < chunkData.chunkHeight; y++)
            {
                for (int z = 0; z < chunkData.chunkWidth; z++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);

                    //获取方块类型
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(this, listBiomeCenter, listBiome, position);

                    //如果是空 则跳过
                    if (blockType == BlockTypeEnum.None)
                        continue;

                    //添加方块
                    chunkData.SetBlockForLocal(x, y, z, blockType, DirectionEnum.UP);
                }
            }
        }
    }

    /// <summary>
    /// 处理 读取的方块
    /// </summary>
    public void HandleForLoadBlock()
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();

        WorldDataBean worldData = gameDataManager.GetWorldData(userData.userId, WorldTypeEnum.Main, chunkData.positionForWorld);

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
            worldData.chunkData.position = chunkData.positionForWorld;
        }
        foreach (var itemData in dicBlockData)
        {
            BlockBean blockData = itemData.Value;
            Vector3Int positionBlock = blockData.localPosition;

            //添加方块 如果已经有该方块 则先删除，优先使用存档的方块
            chunkData.GetBlockForLocal(positionBlock, out BlockTypeEnum blockType, out DirectionEnum direction);

            chunkData.SetBlockForLocal(positionBlock, blockData.blockId, blockData.direction);
        }
        SetWorldData(worldData);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    public void HandleForEventUpdate()
    {
        for (int i = 0; i < listEventUpdate.Count; i++)
        {
            Action<Chunk> actionItem = listEventUpdate[i];
            actionItem?.Invoke(this);
        }
    }

    /// <summary>
    /// 处理实例化模型的方块
    /// </summary>
    public void HandleForBlockModelUpdate()
    {
        if (listBlockModelUpdate.Count <= 0)
            return;
        Vector3Int localPosition = listBlockModelUpdate.Dequeue();
        //首先删除原有的模型
        if (dicBlockModel.TryGetValue(localPosition, out GameObject dataObj))
        {
            dicBlockModel.Remove(localPosition);
            Destroy(dataObj);
        }
        GetBlockForLocal(localPosition, out BlockTypeEnum block, out DirectionEnum direction, out bool isInside);
        if (!isInside || block == BlockTypeEnum.None)
            return;
        //获取数据
        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(block);
        if (blockInfo == null || CheckUtil.StringIsNull(blockInfo.model_name))
            return;
        //获取模型
        GameObject objBlockModel = BlockHandler.Instance.CreateBlockModel(this, (ushort)block, blockInfo.model_name);
        if (objBlockModel == null)
            return;
        dicBlockModel.Add(localPosition, objBlockModel);
        //设置位置
        objBlockModel.transform.position = localPosition + chunkData.positionForWorld + new Vector3(0.5f, 0, 0.5f);
    }

    #region 事件注册
    public void RegisterEventUpdate(Action<Chunk> action)
    {
        listEventUpdate.Add(action);
    }

    public void UnRegisterEventUpdate(Action<Chunk> action)
    {
        listEventUpdate.Remove(action);
    }
    #endregion
}