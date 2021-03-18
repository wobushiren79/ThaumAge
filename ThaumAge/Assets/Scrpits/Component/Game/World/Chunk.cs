using System.Collections.Generic;
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
    public void SetData(Dictionary<Vector3Int, Block> mapForBlock, int width, int height)
    {
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

    public BlockTypeEnum GetBlockType(Vector3Int position)
    {
        if (position.y < 0 || position.y > height - 1)
        {
            return 0;
        }
        int halfWidth = width / 2;
        //当前位置是否在Chunk内
        if ((position.x < -halfWidth) || (position.z < -halfWidth) || (position.x >= halfWidth) || (position.z >= halfWidth))
        {
            BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(position + transform.position, width, height);
            return blockType;
        }
        Block block = mapForBlock[position];
        return block.blockData.GetBlockType();
    }
}