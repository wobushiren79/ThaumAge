using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public BlockTypeEnum blockType;    //��������
    protected BlockInfoBean _blockInfo;//������Ϣ
    protected BlockShape _blockShape;//�������״

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

    public BlockShape blockShape
    {
        get
        {
            if (_blockShape == null)
            {
                BlockShapeEnum blockShapeType = blockInfo.GetBlockShape();
                _blockShape = BlockHandler.Instance.manager.GetRegisterBlockShape(blockShapeType);
            }
            return _blockShape;
        }
        set
        {
            _blockShape = value;
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
        blockShape.InitData(this);
    }

    public Chunk GetChunk(Vector3Int worldPosition)
    {
        return WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
    }

    /// <summary>
    /// ɾ������mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, ChunkMeshIndexData meshIndexData)
    {
        //ɾ�������±���Ϣ
        chunk.chunkMeshData.dicIndexData.Remove(localPosition);
        //�Ƴ���Ӧ��������
        List<int> tris = chunk.chunkMeshData.dicTris[blockInfo.material_type];
        MeshTrisRemove(tris, meshIndexData.trisStartIndex, meshIndexData.trisCount);
        //�������ײ ����Ҫɾ����ײ
        if (blockInfo.collider_state == 1)
        {
            //�Ƴ���Ӧ��������
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            MeshTrisRemove(trisCollider, meshIndexData.trisColliderStartIndex, meshIndexData.trisColliderCount);
        }
        //����д��� ����Ҫɾ������
        if (blockInfo.trigger_state == 1)
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
    /// ����
    /// </summary>
    public virtual void Interactive(Vector3Int worldPosition)
    {

    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        blockShape.BuildBlock(chunk, localPosition, direction);
    }

    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        blockShape.BuildBlock(chunk, localPosition, direction);
    }


    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:�������� 1���ֶ����÷���</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        CreateBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// �ݻٷ���
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition)
    {
        DestoryBlockModel(chunk, localPosition);
        //ȡ��ע��
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// �¼��������_1��
    /// </summary>
    public virtual void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {

    }
    /// <summary>
    /// �¼��������_60��
    /// </summary>
    public virtual void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
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
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        //���·���
        WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, this, this, direction, false);
    }

    /// <summary>
    /// ˢ����Χ����
    /// </summary>
    public virtual void RefreshBlockRange(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
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
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out Block closeBlock, out BlockDirectionEnum direction, out Chunk closeChunk);
        if (closeChunk != null && closeBlock != null && closeBlock.blockType != BlockTypeEnum.None)
        {
            closeBlock?.RefreshBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, direction);
        }
    }

    /// <summary>
    /// ��ȡ�ƻ�����
    /// </summary>
    /// <returns></returns>
    public virtual List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        return blockInfo.GetItemsDrop();
    }

    /// <summary>
    /// ��ȡ�±�
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }


    public Vector3Int GetClosePositionByDirection(DirectionEnum getDirection, Vector3Int localPosition)
    {
        switch (getDirection)
        {
            case DirectionEnum.UP:
                return localPosition.AddY(1);
            case DirectionEnum.Down:
                return localPosition.AddY(-1);
            case DirectionEnum.Left:
                return localPosition.AddX(-1);
            case DirectionEnum.Right:
                return localPosition.AddX(1);
            case DirectionEnum.Forward:
                return localPosition.AddZ(-1);
            case DirectionEnum.Back:
                return localPosition.AddZ(1);
            default:
                return localPosition;
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

        localPosition = GetClosePositionByDirection(getDirection, localPosition);

        int maxWidth = chunk.chunkData.chunkWidth - 1;
        int maxHeight = chunk.chunkData.chunkHeight - 1;
        if (localPosition.x < 0)
        {
            blockChunk = chunk.chunkData.chunkLeft;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(maxWidth, localPosition.y, localPosition.z);
                return;
            }
            return;
        }
        else if (localPosition.x > maxWidth)
        {
            blockChunk = chunk.chunkData.chunkRight;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(0, localPosition.y, localPosition.z);
                return;
            }
            return;
        }
        else if (localPosition.z < 0)
        {
            blockChunk = chunk.chunkData.chunkForward;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(localPosition.x, localPosition.y, maxWidth);
                return;
            }
            return;
        }
        else if (localPosition.z > maxWidth)
        {
            blockChunk = chunk.chunkData.chunkBack;
            if (blockChunk != null)
            {
                block = blockChunk.chunkData.GetBlockForLocal(localPosition.x, localPosition.y, 0);
                return;
            }
            return;
        }
        else if (localPosition.y > maxHeight)
        {
            blockChunk = chunk;
            return;
        }
        else
        {
            //�����ͬһ��chunk��
            block = chunk.chunkData.GetBlockForLocal(localPosition);
            blockChunk = chunk;
        }
    }
}
