using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Chunk : BaseMonoBehaviour
{
    //Chunk的网格
    protected Mesh chunkMesh;

    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    //存储着此Chunk内的所有Block信息
    public Dictionary<Vector3Int, Block> mapForBlock;

    public int width = 0;
    public int height = 0;

    public Vector3Int worldPosition;

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="mapForBlock"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void SetData(Vector3Int worldPosition,Dictionary<Vector3Int, Block> mapForBlock, int width, int height)
    {
        this.worldPosition = worldPosition;
        this.mapForBlock = mapForBlock;
        this.width = width;
        this.height = height;
        BuildChunk();
    }

    public void BuildChunk()
    {
        chunkMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        //遍历chunk, 生成其中的每一个Block
        foreach (var itemData in mapForBlock)
        {
            itemData.Value.BuildBlock(verts, uvs, tris);
        }

        chunkMesh.vertices = verts.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = tris.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;
    }


    public async void BuildChunkForAsync()
    {
        chunkMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        await Task.Run(() => {

            //遍历chunk, 生成其中的每一个Block
            foreach (var itemData in mapForBlock)
            {
                itemData.Value.BuildBlock(verts, uvs, tris);
            }

        });

        chunkMesh.vertices = verts.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = tris.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;

    }

    public BlockTypeEnum GetBlockTypeForWorld(Vector3Int worldPosition)
    {
        Vector3Int localPosition = worldPosition - Vector3Int.CeilToInt(transform.position);
        return GetBlockTypeForLocal(localPosition);
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
        if (mapForBlock.TryGetValue(worldPosition - this.worldPosition, out Block block))
        {
            block.blockData.SetBlockType(blockType);
        }
        BuildChunkForAsync();
       // BuildChunk();
    }

}