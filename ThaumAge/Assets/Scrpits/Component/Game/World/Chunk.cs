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
    public List<Vector3Int> listEventUpdateForSec = new List<Vector3Int>();
    //需要更新事件的方块（频率每秒一次）
    public List<Vector3Int> listEventUpdateForMin = new List<Vector3Int>();

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

    //是否初始化
    public bool isInit = false;
    public bool isBuildChunk = false;
    public bool isDrawMesh = false;
    public bool isFirstDraw = true;

    //包含Chunk内的所有信息
    public ChunkData chunkData;
    //渲染数据
    public ChunkMeshData chunkMeshData;
    //存储数据
    protected ChunkSaveBean chunkSaveData;

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

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();
        chunkMeshTrigger = new Mesh();

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

        //设置为动态变更，理论上可以提高效率
        chunkMesh.MarkDynamic();
        chunkMeshCollider.MarkDynamic();
        chunkMeshTrigger.MarkDynamic();
    }

    protected float eventUpdateTimeForSec = 0;
    protected float eventUpdateTimeForMin = 0;

    public bool isSaveData = false;
    protected void Update()
    {
        if (!isInit)
            return;
        eventUpdateTimeForSec += Time.deltaTime;
        eventUpdateTimeForMin += Time.deltaTime;
        if (eventUpdateTimeForSec > 1)
        {
            eventUpdateTimeForSec = 0;
            HandleForEventUpdateForSec();
        }
        if (eventUpdateTimeForMin > 60)
        {
            eventUpdateTimeForMin = 0;
            HandleForEventUpdateForMin();
        }
        HandleForBlockModelUpdate(out bool hasUpdate);
        //先更新完全部需要更新的东西 在进行删除
        if (!hasUpdate)
            HandleForBlockModelDestory();
        //数据保存
        if (isSaveData)
        {
            GameDataHandler.Instance.manager.SaveGameDataAsync(chunkSaveData);
            isSaveData = false;
        }
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
        chunkData = new ChunkData(worldPosition, width, height);
        chunkMeshData = new ChunkMeshData();
    }

    /// <summary>
    /// 设置存储数据
    /// </summary>
    /// <param name="worldData"></param>
    public void SetChunkSaveData(ChunkSaveBean chunkSaveData)
    {
        this.chunkSaveData = chunkSaveData;
    }
    /// <summary>
    /// 获取存储数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveBean GetChunkSaveData()
    {
        return chunkSaveData;
    }

    /// <summary>
    /// 获取存储的方块数据
    /// </summary>
    public BlockBean GetBlockData(int x, int y, int z)
    {
        if (chunkSaveData == null)
            return null;
        BlockBean blockData = chunkSaveData.GetBlockData(x, y, z);
        return blockData;
    }

    /// <summary>
    /// 获取存储的方块数据
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public BlockBean GetBlockData(Vector3Int localPosition)
    {
        return GetBlockData(localPosition.x, localPosition.y, localPosition.z);
    }

    /// <summary>
    /// 获取方块模型
    /// </summary>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public GameObject GetBlockObjForLocal(Vector3Int localPosition)
    {
        int blockIndex = Block.GetIndex(localPosition, chunkData.chunkWidth, chunkData.chunkHeight);
        if (dicBlockModel.TryGetValue(blockIndex, out GameObject obj))
        {
            return obj;
        }
        return null;
    }

    /// <summary>
    /// 设置存储方块数据
    /// </summary>
    /// <param name="blockData"></param>
    public void SetBlockData(BlockBean blockData, bool isSaveData = true)
    {
        int index = chunkData.GetIndexByPosition(blockData.localPosition);
        chunkSaveData.dicBlockData[index] = blockData;

        //异步保存数据
        this.isSaveData = isSaveData;
    }

    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearBlockData(int x, int y, int z)
    {
        if (chunkSaveData == null)
            return;
        chunkSaveData.ClearBlockData(x, y, z);
    }
    /// <summary>
    /// 清除数据
    /// </summary>
    public void ClearBlockData(Vector3Int localPosition)
    {
        ClearBlockData(localPosition.x, localPosition.y, localPosition.z);
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

                    chunkData.InitRoundChunk();

                    for (int i = 0; i < chunkData.arrayBlock.Length; i++)
                    {
                        Block block = chunkData.arrayBlock[i];
                        if (block == null || block.blockType == BlockTypeEnum.None)
                            continue;
                        BlockDirectionEnum blockDirection = (BlockDirectionEnum)chunkData.arrayBlockDirection[i];
                        Vector3Int localPosition = chunkData.GetPositionByIndex(i);
                        block.BuildBlock(this, localPosition, blockDirection);
                        block.InitBlock(this, localPosition, blockDirection, 0);
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
    /// 初始化mats
    /// </summary>
    protected void InitBlockMats()
    {
        Material[] allBlockMats = BlockHandler.Instance.manager.GetAllBlockMaterial();
        List<Material> newBlockMtas = new List<Material>();
        for (int i = 0; i < allBlockMats.Length; i++)
        {
            List<int> listTrisMat = chunkMeshData.dicTris[i];
            if (!listTrisMat.IsNull())
            {
                newBlockMtas.Add(allBlockMats[i]);
            }
        }
        meshRenderer.materials = newBlockMtas.ToArray();
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
            InitBlockMats();
            isDrawMesh = true;

            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //定点数判断
            if (chunkMeshData == null && chunkMeshData.verts.Count < 3)
            {
                isDrawMesh = false;
                return;
            }
            chunkMesh.Clear();
            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //设置顶点
            chunkMesh.SetVertices(chunkMeshData.verts);
            //设置UV
            chunkMesh.SetUVs(0, chunkMeshData.uvs);

            //设置三角（单面渲染，双面渲染,液体）
            int indexMat = 0;
            for (int i = 0; i < chunkMeshData.dicTris.Length; i++)
            {
                List<int> trisData = chunkMeshData.dicTris[i];
                if (trisData.IsNull())
                    continue;
                chunkMesh.SetTriangles(trisData, indexMat);
                indexMat++;
            }

            //碰撞数据设置
            if (chunkMeshData.vertsCollider.Count >= 3)
            {
                chunkMeshCollider.Clear();
                chunkMeshCollider.SetVertices(chunkMeshData.vertsCollider);
                chunkMeshCollider.SetTriangles(chunkMeshData.trisCollider, 0);
            }
            //触发数据设置
            if (chunkMeshData.vertsTrigger.Count >= 3)
            {
                chunkMeshTrigger.Clear();
                chunkMeshTrigger.SetVertices(chunkMeshData.vertsTrigger);
                chunkMeshTrigger.SetTriangles(chunkMeshData.trisTrigger, 0);
            }


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
            meshCollider.sharedMesh = chunkMeshCollider;
            meshTrigger.sharedMesh = chunkMeshTrigger;

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

    public void GetBlockForWorld(Vector3Int blockWorldPosition, out Block block, out BlockDirectionEnum direction, out Chunk chunk)
    {
        GetBlockForLocal(blockWorldPosition - chunkData.positionForWorld, out block, out direction, out chunk);
    }
    public void GetBlockForWorld(Vector3Int blockWorldPosition, out Block block, out bool isInside)
    {
        isInside = true;
        block = chunkData.GetBlockForLocal(blockWorldPosition - chunkData.positionForWorld);
    }

    public void GetBlockForLocal(Vector3Int localPosition, out Block block, out BlockDirectionEnum direction, out Chunk chunk)
    {
        if (localPosition.y < 0 || localPosition.y > chunkData.chunkHeight - 1)
        {
            block = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            direction = BlockDirectionEnum.UpForward;
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
    public void RemoveBlockForWorld(Vector3Int worldPosition, bool isSaveData = true)
    {
        SetBlockForWorld(worldPosition, BlockTypeEnum.None, isSaveData: isSaveData);
    }
    public void RemoveBlockForLocal(Vector3Int localPosition, bool isSaveData = true)
    {
        SetBlockForLocal(localPosition, BlockTypeEnum.None, isSaveData: isSaveData);
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction = BlockDirectionEnum.UpForward, string meta = null, bool isRefreshMesh = true, bool isSaveData = true, bool isRefreshBlockRange = true)
    {
        Vector3Int blockLocalPosition = worldPosition - chunkData.positionForWorld;
        SetBlockForLocal(blockLocalPosition, blockType, direction, meta, isRefreshMesh, isSaveData, isRefreshBlockRange);
    }
    public void SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType, BlockDirectionEnum direction = BlockDirectionEnum.UpForward, string meta = null, bool isRefreshMesh = true, bool isSaveData = true, bool isRefreshBlockRange = true)
    {
        if (localPosition.y > chunkData.chunkHeight)
            return;
        //首先移除方块
        Block oldBlock = chunkData.GetBlockForLocal(localPosition);
        if (oldBlock != null && oldBlock.blockType != BlockTypeEnum.None)
        {
            //先删除方块 再删除老数据 因为再删除方块时会用到老数据
            oldBlock.DestoryBlock(this, localPosition, direction);
            //删除老数据
            ClearBlockData(localPosition);
        }
        //设置新方块
        Block newBlock = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
        chunkData.SetBlockForLocal(localPosition, newBlock, direction);

        //保存数据
        if (isSaveData)
        {
            BlockBean blockData = new BlockBean(localPosition, blockType, direction, meta);
            SetBlockData(blockData);
        }
        //刷新blockMesh
        if (isRefreshMesh)
        {
            WorldCreateHandler.Instance.HandleForUpdateChunk(this, localPosition, oldBlock, newBlock, direction);
        }
        //初始化方块(放再这里是等处理完数据和mesh之后再初始化)
        newBlock.InitBlock(this, localPosition, direction, 1);
        //刷新六个方向的方块
        if (isRefreshBlockRange)
        {
            newBlock.RefreshBlockRange(this, localPosition, direction);
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
                    chunkData.SetBlockForLocal(x, y, z, block, BlockDirectionEnum.UpForward);
                }
            }
        }

        //生成洞穴 不放在每一个方块里去检测 提升效率
        BiomeCreateTool.BiomeForCaveData caveData = new BiomeCreateTool.BiomeForCaveData();
        caveData.minDepth = 100;
        caveData.maxDepth = 200;
        caveData.minSize = 3;
        caveData.maxSize = 5;
        BiomeCreateTool.AddCave(this, mapData, caveData);
    }

    /// <summary>
    /// 处理 读取的方块
    /// </summary>
    public void HandleForLoadBlock()
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();

        ChunkSaveBean chunkSaveData = gameDataManager.GetChunkSaveData(userData.userId, WorldCreateHandler.Instance.manager.worldType, chunkData.positionForWorld);
        //如果没有世界数据 则创建一个
        if (chunkSaveData == null)
        {
            chunkSaveData = new ChunkSaveBean();
            chunkSaveData.workdType = (int)WorldCreateHandler.Instance.manager.worldType;
            chunkSaveData.userId = userData.userId;
            chunkSaveData.position = chunkData.positionForWorld;
        }
        chunkSaveData.InitData();
        Dictionary<int, BlockBean> dicBlockData = chunkSaveData.dicBlockData;
        foreach (var itemData in dicBlockData)
        {
            BlockBean blockData = itemData.Value;
            Vector3Int positionBlock = blockData.localPosition;

            //添加方块 如果已经有该方块 则先删除，优先使用存档的方块
            //chunkData.GetBlockForLocal(positionBlock, out Block block, out DirectionEnum direction);

            Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockData.blockId);
            chunkData.SetBlockForLocal(positionBlock, block, blockData.direction);
        }
        SetChunkSaveData(chunkSaveData);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    public void HandleForEventUpdateForSec()
    {
        for (int i = 0; i < listEventUpdateForSec.Count; i++)
        {
            Vector3Int localPosition = listEventUpdateForSec[i];
            Block block = chunkData.GetBlockForLocal(localPosition);
            block.EventBlockUpdateForSec(this, localPosition);
        }
    }
    public void HandleForEventUpdateForMin()
    {
        for (int i = 0; i < listEventUpdateForMin.Count; i++)
        {
            Vector3Int localPosition = listEventUpdateForMin[i];
            Block block = chunkData.GetBlockForLocal(localPosition);
            block.EventBlockUpdateForMin(this, localPosition);
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

        chunkData.GetBlockForLocal(localPosition, out Block block, out BlockDirectionEnum direction);

        if (block == null || block.blockType == BlockTypeEnum.None)
            return;
        if (block.blockInfo == null || block.blockInfo.model_name.IsNull())
            return;
        //获取模型
        GameObject objBlockModel = BlockHandler.Instance.CreateBlockModel(this, (ushort)block.blockType, block.blockInfo.model_name);
        if (objBlockModel == null)
            return;
        //设置位置
        objBlockModel.transform.localPosition = localPosition + new Vector3(0.5f, 0.5f, 0.5f);
        //添加数据记录
        dicBlockModel.Add(blockIndex, objBlockModel);
        //设置方向
        Vector3 rotateAngles = BlockShape.GetRotateAngles(direction);
        objBlockModel.transform.localEulerAngles = rotateAngles;
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
    public void RegisterEventUpdate(Vector3Int position, TimeUpdateEventTypeEnum updateTimeType)
    {
        if (updateTimeType == TimeUpdateEventTypeEnum.Sec)
        {
            if (!listEventUpdateForSec.Contains(position))
                listEventUpdateForSec.Add(position);
        }
        else if (updateTimeType == TimeUpdateEventTypeEnum.Min)
        {
            if (!listEventUpdateForMin.Contains(position))
                listEventUpdateForMin.Add(position);
        }
    }

    /// <summary>
    /// 注销事件
    /// </summary>
    /// <param name="position"></param>
    /// <param name="updateTime"></param>
    public void UnRegisterEventUpdate(Vector3Int position, TimeUpdateEventTypeEnum updateTimeType)
    {
        if (updateTimeType == TimeUpdateEventTypeEnum.Sec)
        {
            if (listEventUpdateForSec.Contains(position))
                listEventUpdateForSec.Remove(position);
        }
        else if (updateTimeType == TimeUpdateEventTypeEnum.Min)
        {
            if (listEventUpdateForMin.Contains(position))
                listEventUpdateForMin.Remove(position);
        }
    }
    #endregion

}