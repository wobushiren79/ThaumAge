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
        BlockTypeEnum type = chunk.GetBlockType(position);
        switch (type)
        {
            case BlockTypeEnum.None:
                return true;
            default:
                return false;
        }
    }

    public abstract void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris);

    public abstract void BuildFace(BlockTypeEnum blockType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris);
}
