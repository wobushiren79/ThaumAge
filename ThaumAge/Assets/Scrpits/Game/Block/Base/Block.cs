using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Block
{
    public Chunk chunk;    //����Chunk

    public BlockTypeEnum blockType;    //��������

    public Vector3Int localPosition; //Chunk�ڵ�����
    public Vector3Int worldPosition; //��������                             
    public Vector3 centerPosition;

    public int contactLevel;    //������ϵ�ȼ�
    public DirectionEnum direction;    //����
    public string meta;    //��������

    protected BlockBean _blockData; //��������
    protected BlockInfoBean _blockInfo;//������Ϣ
    protected float uvWidth;

    public BlockBean blockData
    {
        set
        {
            _blockData = value;
        }
        get
        {
            if (_blockData == null)
            {
                _blockData = new BlockBean(blockType, localPosition, worldPosition, direction);
            }
            return _blockData;
        }
    }

    public BlockInfoBean blockInfo
    {
        get
        {
            if (_blockInfo == null)
            {
                _blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockType);
            }
            return _blockInfo;
        }
    }

    public Block()
    {

    }

    public Block(BlockTypeEnum blockType)
    {
        this.blockType = blockType;
    }

    public virtual void RefreshBlock()
    {

    }

    public virtual void RefreshBlockRange()
    {
        bool hasChunk;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.up,out Block upBlock ,out hasChunk);
        upBlock?.RefreshBlock();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.down, out Block downBlock, out hasChunk);
        downBlock?.RefreshBlock();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.left, out Block leftBlock, out hasChunk);
        leftBlock?.RefreshBlock();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.right, out Block rightBlock, out hasChunk);
        rightBlock?.RefreshBlock();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.forward, out Block forwardBlock, out hasChunk);
        forwardBlock?.RefreshBlock();
         WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.back, out Block backBlock, out hasChunk);
        backBlock?.RefreshBlock();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    public virtual void SetData(Chunk chunk, BlockTypeEnum blockType, Vector3Int localPosition, DirectionEnum direction)
    {
        this.chunk = chunk;
        this.blockType = blockType;
        this.localPosition = localPosition;
        if (chunk != null)
            this.worldPosition = localPosition + chunk.worldPosition;
        this.centerPosition = localPosition + new Vector3(0.5f, 0.5f, 0.5f);
        uvWidth = 1 / 128f;
    }
    public virtual void SetData(Chunk chunk, BlockTypeEnum blockType, Vector3Int localPosition)
    {
        SetData(chunk, blockType, localPosition, DirectionEnum.UP);
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
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition,out closeBlock,out bool hasChunk);
        if (closeBlock == null)
        {
            if (hasChunk)
            {       
                //ֻ�ǿ�������
                return true;
            }
            else
            {
                //��û������chunk
                return false;
            }
        }
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
        return CheckNeedBuildFace(position, out Block value);
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

    /// <summary>
    /// �ݻٷ���
    /// </summary>
    public virtual void DestoryBlock()
    {

    }

}
