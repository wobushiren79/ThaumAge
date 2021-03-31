using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
    public Chunk chunk;    //����Chunk
    public Vector3Int position; //����Chunk�ڵ�����
    public BlockBean blockData; //��������

    protected float uvWidth = 1 / 128f;

    public Block()
    {

    }

    public Block(BlockTypeEnum blockType)
    {
        if (blockData == null)
            blockData = new BlockBean(blockType, Vector3Int.zero, Vector3Int.zero);
    }

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
        Block block = chunk.GetBlockForLocal(position);
        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(block.blockData.blockId);
        BlockShapeEnum blockShape = blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            default:
                return true;
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk.ChunkData chunkData)
    {

    }

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
    public virtual void BuildFace(BlockBean blockData, Vector3 corner, Chunk.ChunkData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, chunkData);
        AddUVs(blockData, chunkData);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void AddVerts(Vector3 corner, Chunk.ChunkData chunkData)
    {

    }

    /// <summary>
    /// ���UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void AddUVs(BlockBean blockData, Chunk.ChunkData chunkData)
    {

    }


    /// <summary>
    /// �������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void AddTris(Chunk.ChunkData chunkData)
    {

    }
}
