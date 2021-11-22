using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System.Collections.Concurrent;

public class Chunk : BaseMonoBehaviour
{
    //需要更新事件的方块（频率每秒一次）
    public List<Vector3Int> listEventUpdateFor1 = new List<Vector3Int>();
    //需要更新事件的方块（频率每秒一次）
    public List<Vector3Int> listEventUpdateFor60 = new List<Vector3Int>();

    //需要创建方块实力的列表
    public ConcurrentQueue<Vector3Int> listBlockModelUpdate = new ConcurrentQueue<Vector3Int>();
    //需要删除方块实力的列表
    public ConcurrentQueue<Vector3Int> listBlockModelDestroy = new ConcurrentQueue<Vector3Int>();
    //方块模型
    public Dictionary<int, GameObject> dicBlockModel = new Dictionary<int, GameObject>();

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
    public bool isFirstDraw = true;

    //渲染数据
    public ChunkMeshData chunkMeshData;
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
        meshRenderer.materials = BlockHandler.Instance.manager.GetAllBlockMaterial();

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
    }

    protected float eventUpdateTimeFor1 = 0;
    protected float eventUpdateTimeFor60 = 0;

    private void Update()
    {
        if (!isInit)
            return;
        eventUpdateTimeFor1 += Time.deltaTime;
        eventUpdateTimeFor60 += Time.deltaTime;
        if (eventUpdateTimeFor1 > 1)
        {
            eventUpdateTimeFor1 = 0;
            HandleForEventUpdateFor1();
        }
        if (eventUpdateTimeFor60 > 60)
        {
            eventUpdateTimeFor60 = 0;
            HandleForEventUpdateFor60();
        }
        HandleForBlockModelUpdate(out bool hasUpdate);
        //先更新完全部需要更新的东西 在进行删除
        if (!hasUpdate)
            HandleForBlockModelDestory();
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
        chunkMeshData = new ChunkMeshData();
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
        if (!isInit || chunkData.arrayBlock.Length <= 0)
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
                    chunkMeshData = new ChunkMeshData();

                    for (int x = 0; x < chunkData.chunkWidth; x++)
                    {
                        for (int y = 0; y < chunkData.chunkHeight; y++)
                        {
                            for (int z = 0; z < chunkData.chunkWidth; z++)
                            {
                                chunkData.GetBlockForLocal(x, y, z, out Block block, out DirectionEnum direction);
                                if (block == null || block.blockType == BlockTypeEnum.None)
                                    continue;
                                Vector3Int localPosition = new Vector3Int(x, y, z);
                                block.BuildBlock(this, localPosition, direction);
                                block.InitBlock(this, localPosition);
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
                LogUtil.LogError("BuildChunkForAsync:" + e.ToString());
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
            if (chunkMeshData == null
                || chunkMeshData.verts.Count <= 3)
            {
                isDrawMesh = false;
                return;
            }

            //设置顶点
            chunkMesh.SetVertices(chunkMeshData.verts);
            //设置UV
            chunkMesh.SetUVs(0, chunkMeshData.uvs);

            //设置三角（单面渲染，双面渲染,液体）
            for (int i = 0; i < chunkMeshData.dicTris.Length; i++)
            {
                List<int> trisData = chunkMeshData.dicTris[i];
                chunkMesh.SetTriangles(trisData, i);
            }


            //碰撞数据设置
            chunkMeshCollider.SetVertices(chunkMeshData.vertsCollider);
            chunkMeshCollider.SetTriangles(chunkMeshData.trisCollider, 0);

            //触发数据设置
            chunkMeshTrigger.SetVertices(chunkMeshData.vertsTrigger);
            chunkMeshTrigger.SetTriangles(chunkMeshData.trisTrigger, 0);

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
            if (chunkMesh.vertexCount >= 3) meshFilter.sharedMesh = chunkMesh;
            if (chunkMeshCollider.vertexCount >= 3) meshCollider.sharedMesh = chunkMeshCollider;
            if (chunkMeshTrigger.vertexCount >= 3) meshTrigger.sharedMesh = chunkMeshTrigger;

            //Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
            //Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);

            //初始化动画
            //AnimForInit(() =>
            //{

            //});
            //刷新寻路
            PathFindingHandler.Instance.manager.RefreshPathFinding(this);
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

    public void GetBlockForWorld(Vector3Int blockWorldPosition, out Block block, out DirectionEnum direction, out Chunk chunk)
    {
        GetBlockForLocal(blockWorldPosition - chunkData.positionForWorld, out block, out direction, out chunk);
    }
    public void GetBlockForWorld(Vector3Int blockWorldPosition, out Block block, out bool isInside)
    {
        isInside = true;
        block = chunkData.GetBlockForLocal(blockWorldPosition - chunkData.positionForWorld);
    }

    public void GetBlockForLocal(Vector3Int localPosition, out Block block, out DirectionEnum direction, out Chunk chunk)
    {
        if (localPosition.y < 0 || localPosition.y > chunkData.chunkHeight - 1)
        {
            block = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            direction = DirectionEnum.UP;
            chunk = null;
            return;
        }
        //当前位置是否在Chunk内
        if ((localPosition.x < 0) || (localPosition.z < 0) || (localPosition.x >= chunkData.chunkWidth) || (localPosition.z >= chunkData.chunkWidth))
        {
            Vector3Int blockWorldPosition = localPosition + chunkData.positionForWorld;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out block, out direction, out chunk);
            return;
        }
        else
        {
            chunk = this;
            chunkData.GetBlockForLocal(localPosition, out block, out direction);
            return;
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
    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType, DirectionEnum direction = DirectionEnum.UP)
    {
        Vector3Int blockLocalPosition = worldPosition - chunkData.positionForWorld;
        SetBlockForLocal(blockLocalPosition, blockType, direction);
    }

    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP, string meta = null)
    {
        SetBlockForLocal(localPosition, blockType, direction, meta, true, true, true);
    }

    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType, DirectionEnum direction, string meta, bool isSaveData, bool isRefreshChunkRange, bool isRefreshBlockRange)
    {
        //首先移除方块
        Block oldBlock = chunkData.GetBlockForLocal(localPosition);
        if (oldBlock != null && oldBlock.blockType != BlockTypeEnum.None)
        {
            oldBlock.DestoryBlock(this, localPosition);
        }
        //设置新方块
        Block newBlock = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
        chunkData.SetBlockForLocal(localPosition, newBlock, direction);

        newBlock.InitBlock(this, localPosition);


        //刷新六个方向的方块
        if (isRefreshBlockRange)
        {
            newBlock.RefreshBlockRange(this, localPosition, direction);
        }

        //是否实时刷新
        if (isRefreshChunkRange)
        {
            //异步构建周围chunk
            //暂时取消周围方块刷新
            //AddUpdateChunkForRange(localPosition + chunkData.positionForWorld);
        }

        if (isSaveData)
        {
            int index = chunkData.GetIndexByPosition(localPosition);
            //保存数据
            if (worldData != null && worldData.chunkData != null)
            {
                BlockBean blockData = new BlockBean(localPosition, blockType, direction, meta);
                worldData.chunkData.dicBlockData[index] = blockData;
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
        //获取地图数据
        BiomeMapData[,] mapData = BiomeHandler.Instance.GetBiomeMapData(this);
        //遍历map，生成其中每个Block的信息 
        //生成基础地形数据
        for (int x = 0; x < chunkData.chunkWidth; x++)
        {
            for (int z = 0; z < chunkData.chunkWidth; z++)
            {
                BiomeMapData biomeMapData = mapData[x, z];
                for (int y = 0; y < chunkData.chunkHeight; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);

                    //获取方块类型
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(this, biomeMapData, position);
                    //如果是空 则跳过
                    if (blockType == BlockTypeEnum.None)
                        continue;
                    Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
                    //添加方块
                    chunkData.SetBlockForLocal(x, y, z, block, DirectionEnum.UP);
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

        WorldDataBean worldData = gameDataManager.GetWorldData(userData.userId, WorldCreateHandler.Instance.manager.worldType, chunkData.positionForWorld);

        //如果没有世界数据 则创建一个
        if (worldData == null)
        {
            worldData = new WorldDataBean();
            worldData.workdType = (int)WorldCreateHandler.Instance.manager.worldType;
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
            //chunkData.GetBlockForLocal(positionBlock, out Block block, out DirectionEnum direction);

            Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockData.blockId);
            chunkData.SetBlockForLocal(positionBlock, block, blockData.direction);
        }
        SetWorldData(worldData);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    public void HandleForEventUpdateFor1()
    {
        for (int i = 0; i < listEventUpdateFor1.Count; i++)
        {
            Vector3Int localPosition = listEventUpdateFor1[i];
            Block block = chunkData.GetBlockForLocal(localPosition);
            block.EventBlockUpdateFor1(this, localPosition);
        }
    }
    public void HandleForEventUpdateFor60()
    {
        for (int i = 0; i < listEventUpdateFor60.Count; i++)
        {
            Vector3Int localPosition = listEventUpdateFor60[i];
            Block block = chunkData.GetBlockForLocal(localPosition);
            block.EventBlockUpdateFor60(this, localPosition);
        }
    }


    /// <summary>
    /// 处理实例化模型的方块
    /// </summary>
    public void HandleForBlockModelUpdate(out bool hasUpdate)
    {
        hasUpdate = false;
        if (listBlockModelUpdate.Count <= 0)
            return;
        hasUpdate = true;
        if (!listBlockModelUpdate.TryDequeue(out Vector3Int localPosition))
            return;
        //首先删除原有的模型
        int blockIndex = Block.GetIndex(localPosition, chunkData.chunkWidth, chunkData.chunkHeight);
        if (dicBlockModel.TryGetValue(blockIndex, out GameObject dataObj))
        {
            //dicBlockModel.Remove(localPosition);
            //Destroy(dataObj);
            return;
        }

        Block block = chunkData.GetBlockForLocal(localPosition);

        if (block == null || block.blockType == BlockTypeEnum.None)
            return;
        if (block.blockInfo == null || block.blockInfo.model_name.IsNull())
            return;
        //获取模型
        GameObject objBlockModel = BlockHandler.Instance.CreateBlockModel(this, (ushort)block.blockType, block.blockInfo.model_name);
        if (objBlockModel == null)
            return;
        //设置位置
        objBlockModel.transform.localPosition = localPosition + new Vector3(0.5f, 0, 0.5f);
        //添加数据记录
        dicBlockModel.Add(blockIndex, objBlockModel);
    }

    /// <summary>
    /// 处理删除模型的方块
    /// </summary>
    public void HandleForBlockModelDestory()
    {
        if (listBlockModelDestroy.Count <= 0)
            return;
        while (listBlockModelDestroy.TryDequeue(out Vector3Int localPosition))
        {
            int blockIndex = Block.GetIndex(localPosition, chunkData.chunkWidth, chunkData.chunkHeight);

            if (dicBlockModel.TryGetValue(blockIndex, out GameObject objBlockModel))
            {
                dicBlockModel.Remove(blockIndex);
                Destroy(objBlockModel);
            }
        }
    }

    #region 事件注册

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="position"></param>
    /// <param name="updateTime">1,60</param>
    public void RegisterEventUpdate(Vector3Int position, int updateTime)
    {
        if (updateTime == 1)
        {
            if (!listEventUpdateFor1.Contains(position))
                listEventUpdateFor1.Add(position);
        }
        else if (updateTime == 60)
        {
            if (!listEventUpdateFor60.Contains(position))
                listEventUpdateFor60.Add(position);
        }
    }

    /// <summary>
    /// 注销事件
    /// </summary>
    /// <param name="position"></param>
    /// <param name="updateTime"></param>
    public void UnRegisterEventUpdate(Vector3Int position, int updateTime)
    {
        if (updateTime == 1)
        {
            if (!listEventUpdateFor1.Contains(position))
                listEventUpdateFor1.Remove(position);
        }
        else if (updateTime == 60)
        {
            if (!listEventUpdateFor60.Contains(position))
                listEventUpdateFor60.Remove(position);
        }
    }
    #endregion

}