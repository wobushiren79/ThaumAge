using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public abstract class Block
{
    public BlockTypeEnum blockType;    //��������

    protected float uvWidth = 1 / 128f;

    public Vector3 GetCenterPosition(Vector3Int localPosition)
    {
        return localPosition + new Vector3(0.5f, 0.5f, 0.5f);
    }

    protected BlockInfoBean _blockInfo;//������Ϣ

    public Chunk GetChunk(Vector3Int worldPosition)
    {
        return WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
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
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="closeDirection"></param>
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection, out Block closeBlock)
    {
        closeBlock = null;
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out closeBlock, out Chunk closeBlockChunk);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk)
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

    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        return CheckNeedBuildFace(chunk, localPosition, direction, closeDirection, out Block closeBlock);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {

    }
    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
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
    public virtual void BuildFace(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {
        AddTris(chunkMeshData);
        AddVerts(localPosition, direction, corner, chunkMeshData);
        AddUVs(chunkMeshData);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {

    }

    /// <summary>
    /// ��Ӷ���
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listVerts"></param>
    /// <param name="vert"></param>
    public virtual void AddVert(Vector3Int localPosition, DirectionEnum direction, List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(direction, vert, GetCenterPosition(localPosition)));
    }
    public virtual void AddVert(Vector3Int localPosition, DirectionEnum direction, Vector3[] arrayVerts, int indexVerts, Vector3 vert)
    {
        arrayVerts[indexVerts] = RotatePosition(direction, vert, GetCenterPosition(localPosition));
    }
    /// <summary>
    /// ���UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void AddUVs(ChunkMeshData chunkMeshData)
    {

    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void AddTris(ChunkMeshData chunkMeshData)
    {

    }

    public virtual void GetCloseRotateBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum getDirection, out Block closeBlock, out Chunk blockChunk)
    {
        if (blockInfo.rotate_state == 0)
        {
            //����ת
            GetCloseBlockByDirection(chunk, localPosition, getDirection, out closeBlock, out blockChunk);
        }
        else if (blockInfo.rotate_state == 1)
        {
            //��ת
            DirectionEnum rotateDirection = GetRotateDirection(direction, getDirection);
            GetCloseBlockByDirection(chunk, localPosition, rotateDirection, out closeBlock, out blockChunk);
        }
        else
        {
            closeBlock = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            blockChunk = null;
        }
    }

    /// <summary>
    /// ��ȡ��ͬ����ķ���
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum getDirection, out Block block, out Chunk blockChunk)
    {
        //��ȡĿ��ı�������
        block = null;
        Vector3Int targetBlockLocalPosition;
        switch (getDirection)
        {
            case DirectionEnum.UP:
                targetBlockLocalPosition = localPosition + Vector3Int.up;
                break;
            case DirectionEnum.Down:
                targetBlockLocalPosition = localPosition + Vector3Int.down;
                break;
            case DirectionEnum.Left:
                targetBlockLocalPosition = localPosition + Vector3Int.left;
                break;
            case DirectionEnum.Right:
                targetBlockLocalPosition = localPosition + Vector3Int.right;
                break;
            case DirectionEnum.Forward:
                targetBlockLocalPosition = localPosition + Vector3Int.back;
                break;
            case DirectionEnum.Back:
                targetBlockLocalPosition = localPosition + Vector3Int.forward;
                break;
            default:
                targetBlockLocalPosition = localPosition + Vector3Int.up;
                break;
        }
        if (targetBlockLocalPosition.x < 0)
        {
            blockChunk = chunk.chunkData.chunkLeft;
            if (blockChunk != null)
            {
                blockChunk.GetBlockForLocal(new Vector3Int(chunk.chunkData.chunkWidth - 1, localPosition.y, localPosition.z), out block);
            }
        }
        else if (targetBlockLocalPosition.x > chunk.chunkData.chunkWidth - 1)
        {
            blockChunk = chunk.chunkData.chunkRight;
            if (blockChunk != null)
            {
                blockChunk.GetBlockForLocal(new Vector3Int(0, localPosition.y, localPosition.z), out block);
            }
        }
        else if (targetBlockLocalPosition.z < 0)
        {
            blockChunk = chunk.chunkData.chunkForward;
            if (blockChunk != null)
            {
                blockChunk.GetBlockForLocal(new Vector3Int(localPosition.x, chunk.chunkData.chunkWidth - 1, localPosition.z), out block);
            }
        }
        else if (targetBlockLocalPosition.z > chunk.chunkData.chunkWidth - 1)
        {
            blockChunk = chunk.chunkData.chunkBack;
            if (blockChunk != null)
            {
                blockChunk.GetBlockForLocal(new Vector3Int(localPosition.x, 0, localPosition.z), out block);
            }
        }
        else if (targetBlockLocalPosition.y > chunk.chunkData.chunkHeight - 1)
        {
            blockChunk = chunk;
        }
        else
        {
            //�����ͬһ��chunk��
            chunk.GetBlockForLocal(targetBlockLocalPosition, out block);
            blockChunk = chunk;
        }
    }


    /// <summary>
    /// ���ݱ�������ѡ����
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="getDirection"></param>
    /// <returns></returns>
    public DirectionEnum GetRotateDirection(DirectionEnum direction, DirectionEnum getDirection)
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
    public virtual Vector3 RotatePosition(DirectionEnum direction, Vector3 position, Vector3 centerPosition)
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
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        CreateBlockModel(chunk, localPosition, direction);
    }

    /// <summary>
    /// �ݻٷ���
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        DestoryBlockModel(chunk, localPosition, direction);
    }

    /// <summary>
    /// �¼��������
    /// </summary>
    public virtual void EventBlockUpdate(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// ���������ģ��
    /// </summary>
    public virtual void CreateBlockModel(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        //�����ģ�͡��򴴽�ģ��
        if (!blockInfo.model_name.IsNull())
        {
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    /// <summary>
    /// ɾ�������ģ��
    /// </summary>
    public virtual void DestoryBlockModel(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        //�ݻ�ģ��
        chunk.listBlockModelDestroy.Enqueue(localPosition);
    }

    /// <summary>
    /// ˢ�·���
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// ˢ����Χ����
    /// </summary>
    public virtual void RefreshBlockRange(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;

        RefreshBlockClose(worldPosition + Vector3Int.up);
        RefreshBlockClose(worldPosition + Vector3Int.down);
        RefreshBlockClose(worldPosition + Vector3Int.left);
        RefreshBlockClose(worldPosition + Vector3Int.right);
        RefreshBlockClose(worldPosition + Vector3Int.forward);
        RefreshBlockClose(worldPosition + Vector3Int.back);
    }

    /// <summary>
    /// ˢ�¿����ķ���
    /// </summary>
    /// <param name="closeWorldPosition"></param>
    public virtual void RefreshBlockClose(Vector3Int closeWorldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out Block closeBlock, out DirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk != null)
        {
            closeBlock?.RefreshBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, closeBlockDirection);
        }
    }

    /// <summary>
    /// ��ȡ�±�
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }
}
