using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    protected Block _leftBlock;
    protected bool leftBlockHasChunk;
    protected Block _rightBlock;
    protected bool rightBlockHasChunk;
    protected Block _upBlock;
    protected bool upBlockHasChunk;
    protected Block _downBlock;
    protected bool downBlockHasChunk;
    protected Block _forwardBlock;
    protected bool forwardBlockHasChunk;
    protected Block _backBlock;
    protected bool backBlockHasChunk;

    public Block leftBlock
    {
        get
        {
            if (_leftBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.left), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _leftBlock, out leftBlockHasChunk);
            }
            return _leftBlock;
        }
    }
    public Block rightBlock
    {
        get
        {
            if (_rightBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.right), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _rightBlock, out rightBlockHasChunk);
            }
            return _rightBlock;
        }
    }

    public Block upBlock
    {
        get
        {
            if (_upBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.up), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _upBlock, out upBlockHasChunk);
            }
            return _upBlock;
        }
    }

    public Block downBlock
    {
        get
        {
            if (_downBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.down), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _downBlock, out downBlockHasChunk);
            }
            return _downBlock;
        }
    }

    public Block forwardBlock
    {
        get
        {
            if (_forwardBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.back), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _forwardBlock, out forwardBlockHasChunk);
            }
            return _forwardBlock;
        }
    }

    public Block backBlock
    {
        get
        {
            if (_backBlock == null)
            {
                Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition((localPosition + Vector3Int.forward), localPosition));
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out _backBlock, out  backBlockHasChunk);
            }
            return _backBlock;
        }
    }

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
        upBlock?.RefreshBlock();
        downBlock?.RefreshBlock();
        leftBlock?.RefreshBlock();
        rightBlock?.RefreshBlock();
        forwardBlock?.RefreshBlock();
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
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Vector3Int position, out Block closeBlock)
    {
        closeBlock = null;
        if (position.y < 0) return false;
        //�����ת
        Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition(position, localPosition));
        //��ȡ����
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition, out closeBlock, out bool hasChunk);
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

    /// <summary>
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(DirectionEnum direction, out Block closeBlock)
    {
        closeBlock = null;
        bool hasChunk = false;
        switch (direction)
        {
            case DirectionEnum.Left:
                closeBlock = leftBlock;
                hasChunk = leftBlockHasChunk;
                break;
            case DirectionEnum.Right:
                closeBlock = rightBlock;
                hasChunk = rightBlockHasChunk;
                break;
            case DirectionEnum.UP:
                closeBlock = upBlock;
                hasChunk = upBlockHasChunk;
                break;
            case DirectionEnum.Down:
                closeBlock = downBlock;
                hasChunk = downBlockHasChunk;
                break;
            case DirectionEnum.Forward:
                closeBlock = forwardBlock;
                hasChunk = forwardBlockHasChunk;
                break;
            case DirectionEnum.Back:
                closeBlock = backBlock;
                hasChunk = backBlockHasChunk;
                break;
        }
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
    public virtual bool CheckNeedBuildFace(DirectionEnum direction)
    {
        return CheckNeedBuildFace(direction, out Block value);
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
