using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Chunk : BaseMonoBehaviour
{
    //材质合集
    public Material[] materials;

    //Chunk的网格
    protected Mesh chunkMesh;
    protected Mesh chunkMeshCollider;

    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    //存储着此Chunk内的所有Block信息
    public Dictionary<Vector3Int, Block> mapForBlock;

    public int width = 0;
    public int height = 0;

    public Vector3Int worldPosition;

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

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="mapForBlock"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void SetData(Vector3Int worldPosition, Dictionary<Vector3Int, Block> mapForBlock, int width, int height)
    {
        this.worldPosition = worldPosition;
        this.mapForBlock = mapForBlock;
        this.width = width;
        this.height = height;
        BuildChunkForAsync();
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
    /// 异步构建chunk
    /// </summary>
    public async void BuildChunkForAsync()
    {
        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();

        //设置mesh的三角形上限
        chunkMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshCollider.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //普通使用的三角形合集
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        //碰撞使用的三角形合集
        List<Vector3> vertsCollider = new List<Vector3>();
        List<int> trisCollider = new List<int>();

        //两面都渲染的三角形合集
        List<int> trisBothFace = new List<int>();

        await Task.Run(() =>
        {
            //遍历chunk, 生成其中的每一个Block
            foreach (var itemData in mapForBlock)
            {
                itemData.Value.BuildBlock(verts, uvs, tris, vertsCollider, trisCollider, trisBothFace);
            }
        });

        //设置顶点
        chunkMesh.vertices = verts.ToArray();
        chunkMesh.subMeshCount = materials.Length;
        //设置UV
        chunkMesh.uv = uvs.ToArray();
        //设置三角（单面渲染，双面渲染）
        chunkMesh.SetTriangles(tris.ToArray(), 0);
        chunkMesh.SetTriangles(trisBothFace.ToArray(), 1);

        //刷新
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        //碰撞数据设置
        chunkMeshCollider.vertices = vertsCollider.ToArray();
        chunkMeshCollider.triangles = trisCollider.ToArray();
        //刷新
        chunkMeshCollider.RecalculateBounds();
        chunkMeshCollider.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMeshCollider;
    }

    public BlockTypeEnum GetBlockTypeForWorld(Vector3Int worldPosition)
    {
        Vector3Int localPosition = worldPosition - Vector3Int.CeilToInt(transform.position);
        return GetBlockTypeForLocal(localPosition);
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
            return new BlockCube(BlockTypeEnum.None);
        }
        Block block = mapForBlock[localPosition];
        return block;
    }

    public BlockTypeEnum GetBlockTypeForLocal(Vector3Int localPosition)
    {
        if (localPosition.y < 0 || localPosition.y > height - 1)
        {
            return BlockTypeEnum.None;
        }
        int halfWidth = width / 2;
        //当前位置是否在Chunk内
        if ((localPosition.x < -halfWidth) || (localPosition.z < -halfWidth) || (localPosition.x >= halfWidth) || (localPosition.z >= halfWidth))
        {
            //查询旁边的方块坐标 但需要处理旁边区块方块的刷新问题
            //Chunk chunkOther = null;
            //Vector3Int otherWorldPosition = Vector3Int.zero;
            //if (localPosition.x < -halfWidth)
            //{
            //    chunkOther = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - Vector3Int.right * width);
            //    otherWorldPosition = localPosition + worldPosition - Vector3Int.right;
            //}
            //else if (localPosition.z < -halfWidth)
            //{
            //    chunkOther = WorldCreateHandler.Instance.manager.GetChunk(worldPosition - Vector3Int.forward * width);
            //    otherWorldPosition = localPosition + worldPosition - Vector3Int.forward;
            //}
            //else if (localPosition.x >= halfWidth)
            //{
            //    chunkOther = WorldCreateHandler.Instance.manager.GetChunk(worldPosition + Vector3Int.right * width);
            //    otherWorldPosition = localPosition + worldPosition + Vector3Int.right;
            //}
            //else if (localPosition.z >= halfWidth)
            //{
            //    chunkOther = WorldCreateHandler.Instance.manager.GetChunk(worldPosition + Vector3Int.forward * width);
            //    otherWorldPosition = localPosition + worldPosition - Vector3Int.forward;
            //}
            //if (chunkOther == null)
            //{
            //    return BlockTypeEnum.None;
            //}

            //BlockTypeEnum blockType = chunkOther.GetBlockTypeForWorld(otherWorldPosition);
            //return blockType;
            return BlockTypeEnum.None;
        }
        Block block = mapForBlock[localPosition];
        return block.blockData.GetBlockType();
    }

    /// <summary>
    /// 移除方块
    /// </summary>
    /// <param name="position"></param>
    public void RemoveBlock(Vector3Int position)
    {
        SetBlock(position, BlockTypeEnum.None);
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="blockType"></param>
    public void SetBlock(Vector3Int worldPosition, BlockTypeEnum blockType)
    {
        Vector3Int blockLocalPosition = worldPosition - this.worldPosition;
        ChunkBean chunkData = worldData.chunkData;
        //首先移除方块
        if (mapForBlock.TryGetValue(blockLocalPosition, out Block block))
        {
            mapForBlock.Remove(blockLocalPosition);
        }
        if (chunkData.dicBlockData.TryGetValue(blockLocalPosition, out BlockBean blockData))
        {
            chunkData.dicBlockData.Remove(blockLocalPosition);
        }

        //再添加新方块
        Block newBlock = BlockHandler.Instance.CreateBlock(this, blockLocalPosition, blockType);
        mapForBlock.Add(blockLocalPosition, newBlock);
        chunkData.dicBlockData.Add(blockLocalPosition, newBlock.blockData);

        //异步构建chunk
        BuildChunkForAsync();
        //异步保存数据
        GameDataHandler.Instance.manager.SaveGameDataAsync(worldData);
    }

}