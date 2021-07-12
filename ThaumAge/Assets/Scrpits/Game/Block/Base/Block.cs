using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public abstract class Block
{
    public BlockTypeEnum blockType;    //��������
    public DirectionEnum direction;    //����

    public Vector3Int localPosition;
    public Vector3Int worldPosition;

    protected float uvWidth = 1 / 128f;

    public Vector3 centerPosition
    {
        get
        {
            return localPosition + new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    protected BlockInfoBean _blockInfo;//������Ϣ

    public Chunk chunk
    {
        get
        {
            return WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
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
        set
        {
            _blockInfo = value;
        }
    }

    public Block()
    {

    }

    public Block(BlockTypeEnum blockType)
    {
        this.blockType = blockType;

    }

    /// <summary>
    /// ��ȡ�����ķ���
    /// </summary>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    protected Block GetCloseBlock(Vector3Int closeDirection)
    {
        Vector3Int closeWorldPosition = worldPosition + closeDirection;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out BlockTypeEnum closeBlockType, out DirectionEnum closeBlockDirection, out Chunk chunk);
        if (chunk != null)
        {
            Block closeBlock = BlockHandler.Instance.manager.GetRegisterBlock(closeBlockType);
            closeBlock.SetData(closeWorldPosition - chunk.chunkData.positionForWorld, closeWorldPosition, closeBlockDirection);
            return closeBlock;
        }
        return null;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    public virtual void SetData(Vector3Int localPosition, Vector3Int worldPosition, DirectionEnum direction)
    {
        this.localPosition = localPosition;
        this.worldPosition = worldPosition;

        this.direction = direction;
    }

    public virtual void SetData(Vector3Int localPosition, Vector3Int worldPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        SetData(localPosition, worldPosition, direction);
        this.blockType = blockType;
    }

    /// <summary>
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(DirectionEnum direction, out BlockTypeEnum closeBlock)
    {

        closeBlock = BlockTypeEnum.None;
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(direction, out closeBlock, out bool hasChunk);
        if (closeBlock == BlockTypeEnum.None)
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
        Block closeRegisterBlock = BlockHandler.Instance.manager.GetRegisterBlock(closeBlock);
        BlockShapeEnum blockShape = closeRegisterBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            default:
                return true;
        }
    }

    public virtual bool CheckNeedBuildFace(DirectionEnum direction)
    {
        return CheckNeedBuildFace(direction, out BlockTypeEnum closeBlock);
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
    public virtual void BuildBlockNoCheck(Chunk.ChunkRenderData chunkData)
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

    public virtual void GetCloseRotateBlockByDirection(DirectionEnum getDirection, out BlockTypeEnum closeBlock, out bool hasChunk)
    {
        if (blockInfo.rotate_state == 0)
        {
            //����ת
            GetCloseBlockByDirection(getDirection, out closeBlock, out hasChunk);
        }
        else if (blockInfo.rotate_state == 1)
        {
            //��ת
            DirectionEnum rotateDirection = GetRotateDirection(getDirection);
            GetCloseBlockByDirection(rotateDirection, out closeBlock, out hasChunk);
        }
        else
        {
            closeBlock = BlockTypeEnum.None;
            direction = DirectionEnum.UP;
            hasChunk = false;
        }
    }

    /// <summary>
    /// ��ȡ��ͬ����ķ���
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(DirectionEnum getDirection, out BlockTypeEnum blockType, out bool hasChunk)
    {
        Vector3Int targetBlockWorldPosition;
        switch (getDirection)
        {
            case DirectionEnum.UP:
                targetBlockWorldPosition = worldPosition + Vector3Int.up;
                break;
            case DirectionEnum.Down:
                targetBlockWorldPosition = worldPosition + Vector3Int.down;
                break;
            case DirectionEnum.Left:
                targetBlockWorldPosition = worldPosition + Vector3Int.left;
                break;
            case DirectionEnum.Right:
                targetBlockWorldPosition = worldPosition + Vector3Int.right;
                break;
            case DirectionEnum.Forward:
                targetBlockWorldPosition = worldPosition + Vector3Int.back;
                break;
            case DirectionEnum.Back:
                targetBlockWorldPosition = worldPosition + Vector3Int.forward;
                break;
            default:
                targetBlockWorldPosition = worldPosition + Vector3Int.up;
                break;
        }
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetBlockWorldPosition, out blockType, out DirectionEnum direction, out Chunk chunk);
        if (chunk == null)
        {
            hasChunk = false;
        }
        else
        {
            hasChunk = true;
        }
    }

    /// <summary>
    /// ���ݱ�������ѡ����
    /// </summary>
    /// <param name="getDirection"></param>
    /// <returns></returns>
    public DirectionEnum GetRotateDirection(DirectionEnum getDirection)
    {
        DirectionEnum targetDirection = DirectionEnum.UP;
        switch (direction)
        {
            case DirectionEnum.UP:
                targetDirection = getDirection;
                break;
            case DirectionEnum.Down:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Forward;
                        break;
                }
                break;
            case DirectionEnum.Left:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = getDirection;
                        break;
                }
                break;
            case DirectionEnum.Right:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = getDirection;
                        break;
                }
                break;
            case DirectionEnum.Forward:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.UP;
                        break;
                }
                break;
            case DirectionEnum.Back:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Down;
                        break;
                }
                break;
        }
        return targetDirection;
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
                    angles = new Vector3(-90, 0, 0);
                    break;
                case DirectionEnum.Back:
                    angles = new Vector3(90, 0, 0);
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
    /// ��ʼ������
    /// </summary>
    public virtual void InitBlock(Chunk chunk)
    {
        CreateBlockModel(chunk,localPosition);
    }

    /// <summary>
    /// �ݻٷ���
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk)
    {
        DestoryBlockModel(chunk,localPosition);
    }

    /// <summary>
    /// �¼��������
    /// </summary>
    public virtual void EventBlockUpdate(Chunk chunk, Vector3Int localPosition,DirectionEnum direction)
    {

    }

    /// <summary>
    /// ���������ģ��
    /// </summary>
    public virtual void CreateBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //�����ģ�͡��򴴽�ģ��
        if (!CheckUtil.StringIsNull(blockInfo.model_name))
        {
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    /// <summary>
    /// ɾ�������ģ��
    /// </summary>
    public virtual void DestoryBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //�ݻ�ģ��
        chunk.listBlockModelDestroy.Enqueue(localPosition);
    }

    /// <summary>
    /// ˢ�·���
    /// </summary>
    public virtual void RefreshBlock()
    {

    }

    /// <summary>
    /// ˢ����Χ����
    /// </summary>
    public virtual void RefreshBlockRange()
    {
        GetCloseBlock(Vector3Int.up)?.RefreshBlock();
        GetCloseBlock(Vector3Int.down)?.RefreshBlock();
        GetCloseBlock(Vector3Int.left)?.RefreshBlock();
        GetCloseBlock(Vector3Int.right)?.RefreshBlock();
        GetCloseBlock(Vector3Int.forward)?.RefreshBlock();
        GetCloseBlock(Vector3Int.back)?.RefreshBlock();
    }

    /// <summary>
    /// ��ȡ�±�
    /// </summary>
    public int GetIndex(int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }

}
