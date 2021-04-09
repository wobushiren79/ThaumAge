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
        public List<int> tris;

        //碰撞使用的三角形合集
        public List<Vector3> vertsCollider;
        public List<int> trisCollider;

        //两面都渲染的三角形合集
        public List<int> trisBothFace;

        //液体三角型合集
        public List<int> trisLiquid;
    }

    public Action eventUpdate;

    //材质合集
    public Material[] materials;

    //Chunk的网格
    protected Mesh chunkMesh;
    protected Mesh chunkMeshCollider;

    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    //存储着此Chunk内的所有Block信息
    public Dictionary<Vector3Int, Block> mapForBlock = new Dictionary<Vector3Int, Block>();
    //待更新方块
    public List<Block> listUpdateBlock = new List<Block>();

    //大小
    public int width = 0;
    public int height = 0;
    //是否初始化
    public bool isInit = false;
    //世界坐标
    public Vector3Int worldPosition;
    //存储数据
    protected WorldDataBean worldData;

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        meshRenderer.materials = materials;
        //设置mesh的三角形上限
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        //设置碰撞和去除多余的顶点
        meshCollider.cookingOptions = MeshColliderCookingOptions.WeldColocatedVertices;
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
            if (listUpdateBlock.Count > 0)
            {
                BuildChunkForAsync();
                listUpdateBlock.Clear();
            }
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
            tris = new List<int>(),

            //碰撞使用的三角形合集
            vertsCollider = new List<Vector3>(),
            trisCollider = new List<int>(),

            //两面都渲染的三角形合集
            trisBothFace = new List<int>(),

            //液体三角型合集
            trisLiquid = new List<int>()
        };

        await Task.Run(() =>
        {
            lock (this)
            {
                //遍历chunk, 生成其中的每一个Block
                try
                {
                    foreach (Block itemData in mapForBlock.Values)
                    {
                        itemData.BuildBlock(chunkData);
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        });

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();

        //设置mesh的三角形上限
        chunkMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshCollider.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //设置为动态变更，理论上可以提高效率
        chunkMesh.MarkDynamic();
        chunkMeshCollider.MarkDynamic();

        //定点数判断
        if (chunkData.verts.Count <= 3)
        {
            return;
        }

        //设置顶点
        chunkMesh.vertices = chunkData.verts.ToArray();

        //设置子mesh数量
        chunkMesh.subMeshCount = materials.Length;
        //设置UV
        chunkMesh.uv = chunkData.uvs.ToArray();

        //设置三角（单面渲染，双面渲染,液体）
        chunkMesh.SetTriangles(chunkData.tris.ToArray(), 0);
        chunkMesh.SetTriangles(chunkData.trisBothFace.ToArray(), 1);
        chunkMesh.SetTriangles(chunkData.trisLiquid.ToArray(), 2);

        //刷新
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        //碰撞数据设置
        chunkMeshCollider.vertices = chunkData.vertsCollider.ToArray();
        chunkMeshCollider.triangles = chunkData.trisCollider.ToArray();

        //刷新
        chunkMeshCollider.RecalculateBounds();
        chunkMeshCollider.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMeshCollider;
    }

    public Block GetBlockForWorld(Vector3Int blockWorldPosition)
    {
        return GetBlockForLocal(blockWorldPosition - worldPosition);
    }

    public Block GetBlockForLocal(Vector3Int localPosition)
    {
        if (localPosition.y < 0 || localPosition.y > height - 1)
        {
            return new BlockCube(BlockTypeEnum.None);
        }
        int halfWidth = width / 2;
        //当前位置是否在Chunk内
        if ((localPosition.x < -halfWidth) || (localPosition.z < -halfWidth) || (localPosition.x >= halfWidth) || (localPosition.z >= halfWidth))
        {
            Vector3Int blockWorldPosition = localPosition + worldPosition;
            Block tempBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition);
            if (tempBlock == null)
            {
                return new BlockCube(BlockTypeEnum.None);
            }
            else
            {
                return tempBlock;
            }
        }
        if (mapForBlock.TryGetValue(localPosition, out Block value))
        {
            return value;
        }
        else
        {
            return new BlockCube(BlockTypeEnum.None);
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
    public Block SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType)
    {
        Vector3Int blockLocalPosition = worldPosition - this.worldPosition;
        return SetBlockForLocal(blockLocalPosition, blockType);
    }

    public Block SetBlockForLocal(Vector3Int localPosition, BlockTypeEnum blockType)
    {
        BlockBean blockData = new BlockBean(blockType, localPosition, localPosition + worldPosition);
        return SetBlock(blockData,true);
    }

    public Block SetBlock(BlockBean blockData,bool isRefresh)
    {
        ChunkBean chunkData = worldData.chunkData;
        Vector3Int localPosition = blockData.localPosition.GetVector3Int();
        //首先移除方块
        if (mapForBlock.TryGetValue(localPosition, out Block valueBlock))
        {
            mapForBlock.Remove(localPosition);
        }
        if (chunkData.dicBlockData.TryGetValue(localPosition, out BlockBean valueBlockData))
        {
            chunkData.dicBlockData.Remove(localPosition);
        }

        //再添加新方块
        Block newBlock = BlockHandler.Instance.CreateBlock(this, blockData);

        mapForBlock.Add(localPosition, newBlock);
        chunkData.dicBlockData.Add(localPosition, newBlock.blockData);

        //刷新六个方向的方块
        newBlock.RefreshBlockRange();

        //是否实时刷新
        if (isRefresh)
        {
            //异步构建chunk
            BuildChunkRangeForAsync();
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