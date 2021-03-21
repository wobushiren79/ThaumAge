using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{

    public Chunk chunk;    //����Chunk
    public Vector3Int position; //����Chunk�ڵ�����
    public BlockBean blockData; //��������



    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    public void SetData(Chunk chunk, Vector3Int position, BlockBean blockData)
    {
        this.chunk = chunk;
        this.position = position;
        this.blockData = blockData;
    }

    /// <summary>
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckNeedBuildFace(Vector3Int position)
    {
        if (position.y < 0) return false;
        BlockTypeEnum type = chunk.GetBlockTypeForLocal(position);
        switch (type)
        {
            case BlockTypeEnum.None:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public abstract void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris);

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public abstract void BuildFace(BlockBean blockData, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris);

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public abstract void AddVerts(Vector3 corner, Vector3 up, Vector3 right, List<Vector3> verts);

    /// <summary>
    /// ���UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public abstract void AddUVs(BlockBean blockData, List<Vector2> uvs);

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="reversed"></param>
    /// <param name="tris"></param>
    public abstract void AddTris(int index, bool reversed, List<int> tris);
}
