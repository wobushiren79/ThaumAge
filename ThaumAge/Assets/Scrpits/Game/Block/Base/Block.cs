using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{

    public Chunk chunk;    //所属Chunk
    public Vector3Int position; //所属Chunk内的坐标
    public BlockBean blockData; //方框数据

    /// <summary>
    /// 设置数据
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
    /// 检测是否需要构建面
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
