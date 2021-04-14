using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Chunk : BaseMonoBehaviour
{
    public struct ChunkData
    {
        //普通使用的三角形合集
        public List<Vector3> verts;
        public List<Vector2> uvs;

        //碰撞使用的三角形合集
        public List<Vector3> vertsCollider;
        public List<int> trisCollider;

        //出发使用的三角形合集
        public List<Vector3> vertsTrigger;
        public List<int> trisTrigger;

        //所有三角形合集，根据材质球区分
        public Dictionary<BlockMaterialEnum, List<int>> dicTris;
    }

    public Action eventUpdate;

    public MeshCollider meshCollider;
    public MeshCollider meshTrigger;

    protected MeshRenderer meshRenderer;
    protected MeshFilter meshFilter;

    //存储着此Chunk内的所有Block信息
    public Dictionary<Vector3Int, Block> mapForBlock = new Dictionary<Vector3Int, Block>();
    //待更新方块
    public List<BlockBean> listUpdateBlock = new List<BlockBean>();
    //大小
    public int width = 0;
    public int height = 0;
    //是否初始化
    public bool isInit = false;
    public bool isBake = false;
    //世界坐标
    public Vector3Int worldPosition;
    //存储数据
    protected WorldDataBean worldData;

    protected Mesh chunkMesh;
    protected Mesh chunkMeshCollider;
    protected Mesh chunkMeshTrigger;

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        meshCollider.isTrigger = false;
        meshTrigger.isTrigger = true;

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();
        chunkMeshTrigger = new Mesh();

        meshRenderer.materials = WorldCreateHandler.Instance.manager.GetAllMaterial();

        //设置为动态变更，理论上可以提高效率
        chunkMesh.MarkDynamic();
        chunkMeshCollider.MarkDynamic();
        chunkMeshTrigger.MarkDynamic();

        //设置mesh的三角形上限
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshTrigger.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
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
    /// 处理-更新的方块
    /// </summary>
    public void HandleForUpdateBlock()
    {
        if (listUpdateBlock.Count > 0)
        {
            for (int i = 0; i < listUpdateBlock.Count; i++)
            {
                BlockBean itemBlock = listUpdateBlock[i];
                Vector3Int positionBlockWorld = itemBlock.worldPosition.GetVector3Int();
                Vector3Int positionBlockLocal = positionBlockWorld - worldPosition;
                //需要重新设置一下本地坐标 之前没有记录本地坐标
                itemBlock.localPosition = new Vector3IntBean(positionBlockLocal);

                //设置方块
                SetBlock(itemBlock, false, false, false);
                //从更新列表中移除
                listUpdateBlock.Remove(itemBlock);
                i--;
            }
            //异步保存数据
            GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
            //异步刷新
            BuildChunkRangeForAsync();
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
        this.worldPosition = worldPosition;
        this.width = width;
        this.height = height;
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
    /// 设置初始化状态
    /// </summary>
    public void SetInitState(bool isInit)
    {
        this.isInit = isInit;
    }

    /// <summary>
    /// 异步构建范围内的chunk
    /// </summary>
    public void BuildChunkRangeForAsync()
    {
        BuildChunkForAsync();
        Chunk leftChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(-width, 0, 0));
        Chunk rightChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(width, 0, 0));
        Chunk upChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, width));
        Chunk downChunk = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - new Vector3Int(0, 0, -width));
        if (leftChunk && leftChunk.mapForBlock.Count > 0)
            leftChunk.BuildChunkForAsync();
        if (rightChunk && rightChunk.mapForBlock.Count > 0)
            rightChunk.BuildChunkForAsync();
        if (upChunk && upChunk.mapForBlock.Count > 0)
            upChunk.BuildChunkForAsync();
        if (downChunk && downChunk.mapForBlock.Count > 0)
            downChunk.BuildChunkForAsync();
    }

    /// <summary>
    /// 异步构建chunk
    /// </summary>
    public async void BuildChunkForAsync()
    {
        //只有初始化之后的chunk才能刷新
        if (!isInit || mapForBlock.Count <= 0)
            return;
        ChunkData chunkData = new ChunkData
        {
            //普通使用的三角形合集
            verts = new List<Vector3>(),
            uvs = new List<Vector2>(),

            //碰撞使用的三角形合集
            vertsCollider = new List<Vector3>(),
            trisCollider = new List<int>(),

            //碰撞使用的三角形合集
            vertsTrigger = new List<Vector3>(),
            trisTrigger = new List<int>(),

            //三角型合集
            dicTris = new Dictionary<BlockMaterialEnum, List<int>>()
        };
        //初始化数据
        List<BlockMaterialEnum> blockMaterialsEnum = EnumUtil.GetEnumValue<BlockMaterialEnum>();
        for (int i=0;i< blockMaterialsEnum.Count;i++)
        {
            BlockMaterialEnum blockMaterial = blockMaterialsEnum[i];
            chunkData.dicTris.Add(blockMaterial, new List<int>());
        }

        bool isSuccessInit = false;
        await Task.Run(() =>
        {
            //遍历chunk, 生成其中的每一个Block
            try
            {
                foreach (Block itemData in mapForBlock.Values)
                {
                    itemData.BuildBlock(chunkData);
                }
                isSuccessInit = true;
            }
            catch (Exception)
            {
                isSuccessInit = false;
            }
        });

        if (!isSuccessInit || this == null)
            return;

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();
        chunkMeshTrigger = new Mesh();

        //设置mesh的三角形上限
        chunkMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshCollider.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshTrigger.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //设置子mesh数量
        chunkMesh.subMeshCount = meshRenderer.materials.Length;

        //定点数判断
        if (chunkData.verts.Count <= 3)
        {
            return;
        }

        //设置顶点
        chunkMesh.vertices = chunkData.verts.ToArray();
        //设置UV
        chunkMesh.uv = chunkData.uvs.ToArray();

        //设置三角（单面渲染，双面渲染,液体）
        foreach (var itemTris in chunkData.dicTris)
        {
            chunkMesh.SetTriangles(itemTris.Value.ToArray(), (int)itemTris.Key);
        }

        //刷新
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        //碰撞数据设置
        chunkMeshCollider.vertices = chunkData.vertsCollider.ToArray();
        chunkMeshCollider.triangles = chunkData.trisCollider.ToArray();
        //刷新
        chunkMeshCollider.RecalculateBounds();
        chunkMeshCollider.RecalculateNormals();

        //出发数据设置
        chunkMeshTrigger.vertices = chunkData.vertsTrigger.ToArray();
        chunkMeshTrigger.triangles= chunkData.trisTrigger.ToArray();
        //刷新
        chunkMeshTrigger.RecalculateBounds();
        chunkMeshTrigger.RecalculateNormals();

        Physics.BakeMesh(chunkMesh.GetInstanceID(), false);
        Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMeshCollider;
        meshTrigger.sharedMesh = chunkMeshTrigger;
    }

    public Block GetBlockForWorld(Vector3Int blockWorldPosition)
    {
        return GetBlockForLocal(blockWorldPosition - worldPosition);
    }

    public Block GetBlockForLocal(Vector3Int localPosition)
    {
        if (localPosition.y < 0 || localPosition.y > height - 1)
        {
            return null;
        }
        int halfWidth = width / 2;
        //当前位置是否在Chunk内
        if ((localPosition.x < -halfWidth) || (localPosition.z < -halfWidth) || (localPosition.x >= halfWidth) || (localPosition.z >= halfWidth))
        {
            Vector3Int blockWorldPosition = localPosition + worldPosition;
            Block tempBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition);
            return tempBlock;
        }
        if (mapForBlock.TryGetValue(localPosition, out Block value))
        {
            return value;
        }
        else
        {
            return null;
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
        return SetBlock(blockData,true, true, true);
    }

    public Block SetBlock(BlockBean blockData,bool isSaveData, bool isRefreshChunkRange, bool isRefreshBlockRange)
    {
        Vector3Int localPosition = blockData.localPosition.GetVector3Int();    
        //首先移除方块
        if (mapForBlock.TryGetValue(localPosition, out Block valueBlock))
        {
            mapForBlock.Remove(localPosition);
        }
        //添加数据
        Block newBlock = BlockHandler.Instance.CreateBlock(this, blockData);
        mapForBlock.Add(localPosition, newBlock);

        //保存数据
        if (worldData != null && worldData.chunkData != null)
        {
            ChunkBean chunkData = worldData.chunkData;
            if (chunkData.dicBlockData.TryGetValue(localPosition, out BlockBean valueBlockData))
            {
                chunkData.dicBlockData.Remove(localPosition);
            }
            chunkData.dicBlockData.Add(localPosition, newBlock.blockData);
        }

        //刷新六个方向的方块
        if (isRefreshBlockRange)
        {
            newBlock.RefreshBlockRange();
        }

        //是否实时刷新
        if (isRefreshChunkRange)
        {
            //异步构建chunk
            BuildChunkRangeForAsync();
        }

        if (isSaveData)
        {
            //异步保存数据
            GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
        }

        return newBlock;
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