using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public abstract class Block
{
    public Vector3[] vertsAdd;
    public Vector2[] uvsAdd;
    public int[] trisAdd;

    public BlockTypeEnum blockType;    //��������

    public float uvWidth = 1 / 128f;

    protected BlockInfoBean _blockInfo;//������Ϣ

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

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="blockType"></param>
    public virtual void SetData(BlockTypeEnum blockType)
    {
        this.blockType = blockType;
    }

    public Vector3 GetCenterPosition(Vector3Int localPosition)
    {
        return new Vector3(localPosition.x + 0.5f, localPosition.y + 0.5f, localPosition.z + 0.5f);
    }

    public Chunk GetChunk(Vector3Int worldPosition)
    {
        return WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
    }

    /// <summary>
    /// ����Ƿ���Ҫ������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
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

    /// <summary>
    /// �Ƴ�����mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshIndexData meshIndexData)
    {
        RemoveBlockMesh(chunk, localPosition, direction, meshIndexData, BlockMaterialEnum.Normal, true, false);
    }

    /// <summary>
    /// ɾ������mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshIndexData meshIndexData,
        BlockMaterialEnum blockMaterial = BlockMaterialEnum.Normal, bool hasCollider = false, bool hasTrigger = false)
    {
        //ɾ�������±���Ϣ
        chunk.chunkMeshData.dicIndexData.Remove(localPosition);
        //�Ƴ���Ӧ��������
        List<int> tris = chunk.chunkMeshData.dicTris[(int)blockMaterial];
        MeshTrisRemove(tris, meshIndexData.trisStartIndex, meshIndexData.trisCount);
        //�������ײ ����Ҫɾ����ײ
        if (hasCollider)
        {
            //�Ƴ���Ӧ��������
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            MeshTrisRemove(trisCollider, meshIndexData.trisColliderStartIndex, meshIndexData.trisColliderCount);
        }
        //����д��� ����Ҫɾ������
        if (hasTrigger)
        {
            //�Ƴ���Ӧ��������
            List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
            MeshTrisRemove(trisTrigger, meshIndexData.trisColliderStartIndex, meshIndexData.trisColliderCount);
        }
    }

    /// <summary>
    /// ������ɾ������
    /// </summary>
    protected virtual void MeshTrisRemove(List<int> listTris, int trisStartIndex, int trisCount)
    {
        for (int i = 0; i < trisCount; i++)
        {
            listTris[trisStartIndex + i] = 0;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
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
    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        BaseAddTris(chunk, localPosition, direction);
        BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        BaseAddUVs(chunk, localPosition, direction);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {

    }

    /// <summary>
    /// ���UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
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

    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, List<Vector3> listVerts, Vector3[] vertsAdd)
    {
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            listVerts.Add(RotatePosition(direction, localPosition + vertsAdd[i], GetCenterPosition(localPosition)));
        }
    }

    /// <summary>
    /// ����UV
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listUVs"></param>
    /// <param name="uvsAdd"></param>
    public virtual void AddUVs(List<Vector2> listUVs, Vector2[] uvsAdd)
    {
        for (int i = 0; i < uvsAdd.Length; i++)
        {
            listUVs.Add(uvsAdd[i]);
        }
    }

    /// <summary>
    /// ���������±�˳��
    /// </summary>
    /// <param name="listTris"></param>
    /// <param name="trisAdd"></param>
    public virtual void AddTris(int startIndex, List<int> listTris, int[] trisAdd)
    {
        for (int i = 0; i < trisAdd.Length; i++)
        {
            listTris.Add(startIndex + trisAdd[i]);
        }
    }

    /// <summary>
    /// ��ȡ��ת����
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="blockChunk"></param>
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
    public virtual void GetCloseBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum getDirection,
        out Block block, out Chunk blockChunk)
    {
        //��ȡĿ��ı�������
        block = null;
        int targetX = localPosition.x;
        int targetY = localPosition.y;
        int targetZ = localPosition.z;
        switch (getDirection)
        {
            case DirectionEnum.UP:
                targetY++;
                break;
            case DirectionEnum.Down:
                targetY--;
                break;
            case DirectionEnum.Left:
                targetX--;
                break;
            case DirectionEnum.Right:
                targetX++;
                break;
            case DirectionEnum.Forward:
                targetZ--;
                break;
            case DirectionEnum.Back:
                targetZ++;
                break;
            default:
                break;
        }
        int maxWidth = chunk.chunkData.chunkWidth - 1;
        int maxHeight = chunk.chunkData.chunkHeight - 1;
        if (targetX < 0)
        {
            blockChunk = chunk.chunkData.chunkLeft;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(maxWidth, localPosition.y, localPosition.z);
                return;
            }
            return;
        }
        else if (targetX > maxWidth)
        {
            blockChunk = chunk.chunkData.chunkRight;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(0, localPosition.y, localPosition.z);
                return;
            }
            return;
        }
        else if (targetZ < 0)
        {
            blockChunk = chunk.chunkData.chunkForward;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(localPosition.x, localPosition.y, maxWidth);
                return;
            }
            return;
        }
        else if (targetZ > maxWidth)
        {
            blockChunk = chunk.chunkData.chunkBack;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(localPosition.x, localPosition.y, 0);
                return;
            }
            return;
        }
        else if (targetY > maxHeight)
        {
            blockChunk = chunk;
            return;
        }
        else
        {
            //�����ͬһ��chunk��
            block = chunk.chunkData.GetBlockForLocal(targetX, targetY, targetZ);
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
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition)
    {
        CreateBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// �ݻٷ���
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition)
    {
        DestoryBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// �¼��������_1��
    /// </summary>
    public virtual void EventBlockUpdateFor1(Chunk chunk, Vector3Int localPosition)
    {

    }
    /// <summary>
    /// �¼��������_60��
    /// </summary>
    public virtual void EventBlockUpdateFor60(Chunk chunk, Vector3Int localPosition)
    {

    }

    /// <summary>
    /// ���������ģ��
    /// </summary>
    public virtual void CreateBlockModel(Chunk chunk, Vector3Int localPosition)
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
    public virtual void DestoryBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //�ݻ�ģ��
        chunk.listBlockModelDestroy.Enqueue(localPosition);
    }

    /// <summary>
    /// ˢ�·���
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition)
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
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out Block closeBlock, out Chunk closeChunk);
        if (closeChunk != null)
        {
            closeBlock?.RefreshBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld);
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
