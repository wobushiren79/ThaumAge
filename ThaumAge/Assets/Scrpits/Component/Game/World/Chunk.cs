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
    public Dictionary<Vector3Int, BlockBean> mapForBlock;

    public int width = 0;
    public int height = 0;
    public int minHeight = 0;

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void SetData(Dictionary<Vector3Int, BlockBean> mapForBlock, int width, int height, int minHeight)
    {
        this.mapForBlock = mapForBlock;
        this.width = width;
        this.height = height;
        this.minHeight = minHeight;
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
            BuildBlock(itemData.Key, itemData.Value, verts, uvs, tris);
        }

        chunkMesh.vertices = verts.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = tris.ToArray();
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;
    }

    void BuildBlock(Vector3Int position, BlockBean blockData, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        BlockTypeEnum blockType = blockData.blockType;
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(position + new Vector3Int(-1, 0, 0)))
                BuildFace(blockType, position, Vector3.up, Vector3.forward, false, verts, uvs, tris);
            //Right
            if (CheckNeedBuildFace(position + new Vector3Int(1, 0, 0)))
                BuildFace(blockType, position + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris);

            //Bottom
            if (CheckNeedBuildFace(position + new Vector3Int(0, -1, 0)))
                BuildFace(blockType, position, Vector3.forward, Vector3.right, false, verts, uvs, tris);
            //Top
            if (CheckNeedBuildFace(position + new Vector3Int(0, 1, 0)))
                BuildFace(blockType, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris);

            //Front
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, -1)))
                BuildFace(blockType, position, Vector3.up, Vector3.right, true, verts, uvs, tris);
            //Back
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, 1)))
                BuildFace(blockType, position + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
        }
    }

    public bool CheckNeedBuildFace(Vector3Int position)
    {
        if (position.y < 0) return false;
        var type = GetBlockType(position);
        switch (type)
        {
            case BlockTypeEnum.None:
                return true;
            default:
                return false;
        }
    }

    public BlockTypeEnum GetBlockType(Vector3Int position)
    {
        if (position.y < 0 || position.y > height - 1)
        {
            return 0;
        }
        int halfWidth = width / 2;
        //当前位置是否在Chunk内
        if ((position.x  < -halfWidth) || (position.z  < -halfWidth) || (position.x  >= halfWidth) || (position.z  >= halfWidth))
        {
            var id = WorldCreateHandler.Instance.manager.CreateBlockType(position + transform.position, height, minHeight);
            return id;
        }
        BlockBean blockData = mapForBlock[position];
        return blockData.blockType;
    }


    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    void BuildFace(BlockTypeEnum blockType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Vector2 uvWidth = new Vector2(0.25f, 0.25f);
        Vector2 uvCorner = new Vector2(0.00f, 0.75f);

        uvCorner.x += (float)(blockType - 1) / 4;
        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
        }
        else
        {
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 3);
            tris.Add(index + 2);
        }
    }
}