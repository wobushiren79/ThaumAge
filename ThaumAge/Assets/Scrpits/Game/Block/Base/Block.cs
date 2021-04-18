using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
    public Chunk chunk;    //����Chunk
    public Vector3Int localPosition; //Chunk�ڵ�����
    public Vector3Int worldPosition; //��������
    public BlockBean blockData; //��������

    public Vector3 centerPosition;

    protected BlockInfoBean _blockInfo;//������Ϣ
    protected float uvWidth;

    public BlockInfoBean blockInfo
    {
        get
        {
            if (_blockInfo == null)
            {
                _blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.blockId);
            }
            return _blockInfo;
        }
    }

    public Block()
    {
        if (blockData == null)
            blockData = new BlockBean();
    }

    public Block(BlockTypeEnum blockType)
    {
        if (blockData == null)
            blockData = new BlockBean(blockType, Vector3Int.zero, Vector3Int.zero);
    }

    public virtual void RefreshBlock()
    {

    }

    public virtual void RefreshBlockRange()
    {
        Block upBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.up);
        upBlock?.RefreshBlock();
        Block downBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.down);
        downBlock?.RefreshBlock();
        Block leftBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.left);
        leftBlock?.RefreshBlock();
        Block rightBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.right);
        rightBlock?.RefreshBlock();
        Block forwardBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.forward);
        forwardBlock?.RefreshBlock();
        Block backBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.back);
        backBlock?.RefreshBlock();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    public virtual void SetData(Chunk chunk, Vector3Int localPosition, BlockBean blockData)
    {
        this.chunk = chunk;
        this.localPosition = localPosition;
        this.worldPosition = localPosition + chunk.worldPosition;
        this.blockData = blockData;
        this.centerPosition = localPosition + new Vector3(0.5f, 0.5f, 0.5f);
        uvWidth = 1 / 128f;
    }

    /// <summary>
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Vector3Int position, out Block closeBlock)
    {
        closeBlock = null;
        if (position.y < 0) return false;
        //�����ת
        Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition(position, localPosition));
        //��ȡ����
        closeBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition);
        //if (closeBlock.chunk != chunk)
        //    return true;
        if (closeBlock == null)
            return false;
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            default:
                return true;
        }
    }

    public virtual bool CheckNeedBuildFace(Vector3Int position)
    {
        return CheckNeedBuildFace(position,out Block value);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk.ChunkRenderData chunkData)
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
    public virtual void BuildFace(Vector3 corner, Chunk.ChunkRenderData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, chunkData);
        AddUVs(chunkData);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void AddVerts(Vector3 corner, Chunk.ChunkRenderData chunkData)
    {

    }
    public virtual void AddVert(List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(vert, centerPosition));
    }

    /// <summary>
    /// ���UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void AddUVs(Chunk.ChunkRenderData chunkData)
    {

    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void AddTris(Chunk.ChunkRenderData chunkData)
    {

    }

    /// <summary>
    /// ��ת��λ
    /// </summary>
    /// <param name="vert"></param>
    /// <returns></returns>
    public virtual Vector3 RotatePosition(Vector3 position, Vector3 centerPosition)
    {
        if (blockInfo.rotate_state == 0)
        {
            //����ת
            return position;
        }
        else if (blockInfo.rotate_state == 1)
        {
            //�����ĵ���ת
            DirectionEnum direction = blockData.GetDirection();
            Vector3 angles;
            switch (direction)
            {
                case DirectionEnum.UP:
                    angles = new Vector3(0, 0, 0);
                    break;
                case DirectionEnum.Down:
                    angles = new Vector3(0, 0, 180);
                    break;
                case DirectionEnum.Left:
                    angles = new Vector3(0, 0, 90);
                    break;
                case DirectionEnum.Right:
                    angles = new Vector3(0, 0, -90);
                    break;
                case DirectionEnum.Forward:
                    angles = new Vector3(90, 0, 0);
                    break;
                case DirectionEnum.Back:
                    angles = new Vector3(-90, 0, 0);
                    break;
                default:
                    angles = new Vector3(0, 0, 0);
                    break;
            }
            //��ת6��
            Vector3 rotatePosition = VectorUtil.GetRotatedPosition(centerPosition, position, angles);
            return rotatePosition;
        }
        return position;
    }



}
