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
    /// ��ȡ����ʵ��ģ��
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GameObject GetBlockObj(Vector3Int worldPosition)
    {
        return BlockHandler.Instance.GetBlockObj(worldPosition); ;
    }
    
    /// <summary>
    /// ��ȡ����ķ�λ
    /// </summary>
    /// <param name="blockDirection"></param>
    /// <returns></returns>
    public DirectionEnum GetDirection(BlockDirectionEnum blockDirection)
    {
        int direction = (((int)blockDirection) % 100) / 10;
        switch (direction)
        {
            case 1:
                return DirectionEnum.UP;
            case 2:
                return DirectionEnum.Down;
            case 3:
                return DirectionEnum.Left;
            case 4:
                return DirectionEnum.Right;
            case 5:
                return DirectionEnum.Forward;
            case 6:
                return DirectionEnum.Back;
        }
        return DirectionEnum.None;
    }

    /// <summary>
    /// ��ȡ��Χ�ķ���
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="upBlock"></param>
    /// <param name="downBlock"></param>
    /// <param name="leftBlock"></param>
    /// <param name="rightBlock"></param>
    /// <param name="forwardBlock"></param>
    /// <param name="backBlock"></param>
    public void GetRoundBlock(Vector3Int worldPosition, out Block upBlock, out Block downBlock, out Block leftBlock, out Block rightBlock, out Block forwardBlock, out Block backBlock)
    {
        //��ȡ��Χ�ķ��� ����������
        Vector3Int upPosition = worldPosition + Vector3Int.up;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(upPosition, out upBlock, out Chunk upChunk);
        Vector3Int downPosition = worldPosition + Vector3Int.down;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(downPosition, out downBlock, out Chunk downChunk);
        Vector3Int leftPosition = worldPosition + Vector3Int.left;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(leftPosition, out leftBlock, out Chunk leftChunk);
        Vector3Int rightPosition = worldPosition + Vector3Int.right;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(rightPosition, out rightBlock, out Chunk rightChunk);
        Vector3Int forwardPosition = worldPosition + Vector3Int.forward;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(forwardPosition, out forwardBlock, out Chunk forwardChunk);
        Vector3Int backPosition = worldPosition + Vector3Int.back;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(backPosition, out backBlock, out Chunk backChunk);
    }

    /// <summary>
    /// ɾ������mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, ChunkMeshIndexData meshIndexData)
    {
        //ɾ�������±���Ϣ
        //chunk.chunkMeshData.dicIndexData.Remove(localPosition);

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
    public virtual void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {

    }

    /// <summary>
    /// ��ײ
    /// </summary>
    /// <param name="user"></param>
    public virtual void OnCollision(GameObject user, Vector3Int worldPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        blockShape.BuildBlock(chunk, localPosition);
    }

    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition)
    {
        blockShape.BuildBlock(chunk, localPosition);
    }


    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:�������� 1���ֶ����÷���</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition,int state)
    {
        CreateBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// �ݻٷ���-�����·���֮ǰ
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
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
    /// ��������ģ�ͳɹ�
    /// </summary>
    public virtual void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {

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
        WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk,1);
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

    /// <summary>
    /// ��ȡʹ�õ���ʱ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        return curMeta;
    }

    /// <summary>
    /// ����ʹ�ã����ڷ���ķ��û�����������
    /// </summary>
    public virtual void ItemUse(
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk,
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk,
        BlockDirectionEnum direction , string metaData)
    {
        //���·��鲢 ��Ӹ�������
        switch (blockInfo.rotate_state)
        {
            case 0:
                closeChunk.SetBlockForWorld(closeWorldPosition, blockType, BlockDirectionEnum.UpForward, metaData);
                break;
            case 1:
                closeChunk.SetBlockForWorld(closeWorldPosition, blockType, direction, metaData);
                break;
            case 2:
                if ((int)direction > 20)
                {
                    direction = (BlockDirectionEnum)((int)direction % 10 + 10);
                }
                closeChunk.SetBlockForWorld(closeWorldPosition, blockType, direction, metaData);
                break;
            case 3:
                if ((int)direction > 20&& (int)direction < 30)
                {
                    direction = BlockDirectionEnum.DownForward;
                }
                else
                {
                    direction = BlockDirectionEnum.UpForward;
                }
                closeChunk.SetBlockForWorld(closeWorldPosition, blockType, direction, metaData);
                break;
        }
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
        out Block block, out Chunk blockChunk,out Vector3Int closeLocalPosition)
    {
        //��ȡĿ��ı�������
        block = null;

        localPosition = GetClosePositionByDirection(getDirection, localPosition);
        closeLocalPosition = localPosition;

        int maxWidth = chunk.chunkData.chunkWidth - 1;
        int maxHeight = chunk.chunkData.chunkHeight - 1;

        if (localPosition.x < 0)
        {
            blockChunk = chunk.chunkData.chunkLeft;
            if (blockChunk != null)
            {
                closeLocalPosition = new Vector3Int(maxWidth, localPosition.y, localPosition.z);
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
                closeLocalPosition = new Vector3Int(0, localPosition.y, localPosition.z);
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
                closeLocalPosition = new Vector3Int(localPosition.x, localPosition.y, maxWidth);
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
                closeLocalPosition = new Vector3Int(localPosition.x, localPosition.y, 0);
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

    /// <summary>
    /// �������ӵķ���
    /// </summary>
    public virtual void CreateLinkBlock(Chunk chunk, Vector3Int localPosition, List<Vector3Int> listLink)
    {
        //��ȡ����
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null)
        {
            BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
            if (blockDoorData != null)
            {
                //������Ӽ� ������
                if (blockDoorData.level > 0)
                    return;
            }
        }
        //�ж��Ƿ���ָ����link���������������飬����������ɵ���
        bool hasBlock = false;
        for (int i = 0; i < listLink.Count; i++)
        {
            Vector3Int linkPosition = listLink[i];
            Vector3Int closeWorldPosition = localPosition + chunk.chunkData.positionForWorld + linkPosition;
            chunk.GetBlockForWorld(closeWorldPosition, out Block closeBlock, out BlockDirectionEnum closeDirection, out Chunk closeChunk);
            if (closeBlock != null && closeBlock.blockType != BlockTypeEnum.None)
            {
                hasBlock = true;
                break;
            }
        }
        if (hasBlock)
        {
            //��������
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
            ItemsHandler.Instance.CreateItemCptDrop(this, chunk, localPosition + chunk.chunkData.positionForWorld);
        }
        else
        {
            //����link����
            for (int i = 0; i < listLink.Count; i++)
            {
                Vector3Int linkPosition = listLink[i];
                Vector3Int closeWorldPosition = localPosition + chunk.chunkData.positionForWorld + linkPosition;
                BlockDoorBean blockDoor = new BlockDoorBean();
                blockDoor.level = 1;
                blockDoor.linkBasePosition = new Vector3IntBean(localPosition + chunk.chunkData.positionForWorld);
                BlockDirectionEnum blockDirection = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
                chunk.SetBlockForWorld(closeWorldPosition, blockType, blockDirection, ToMetaData(blockDoor));
            }
        }
    }

    /// <summary>
    /// ɾ�����ӷ���
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listLink"></param>
    public void DestoryLinkBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, List<Vector3Int> listLink)
    {
        Vector3Int baseWorldPosition = localPosition + chunk.chunkData.positionForWorld;
        //��ȡ����
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //�ӳ�һִ֡�� �ȵ�ǰ�����Ѿ�ɾ����
        chunk.chunkComponent.WaitExecuteEndOfFrame(1, () =>
        {
            if (blockData != null)
            {
                BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
                if (blockDoorData != null)
                {
                    //������Ӽ� ������
                    if (blockDoorData.level > 0)
                    {
                        baseWorldPosition = blockDoorData.linkBasePosition.GetVector3Int();
                        //ɾ����������
                        chunk.SetBlockForWorld(baseWorldPosition, BlockTypeEnum.None);
                    }
                }
            }

            //��������Ӽ� ��˵���ǻ������� �����￪ʼɾ������
            for (int i = 0; i < listLink.Count; i++)
            {
                Vector3Int linkPosition = listLink[i];
                Vector3Int closeWorldPosition = baseWorldPosition + linkPosition;
                chunk.SetBlockForWorld(closeWorldPosition, BlockTypeEnum.None);
            }
        });
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="chunk"></param>
    /// <param name="baseWorldPosition"></param>
    /// <param name="listLinkPosition"></param>
    /// <param name="data"></param>
    public void SaveLinkBaseBlockData<T>(Vector3Int baseWorldPosition, T data) where T : BlockBaseLinkBean
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseWorldPosition, out Block baseBlock, out Chunk baseChunk);
        BlockBean baseBlockData = baseChunk.GetBlockDataForWorldPosition(baseWorldPosition);
        baseBlockData.meta = ToMetaData(data);
        baseChunk.SetBlockData(baseBlockData);
    }

    /// <summary>
    /// ��ȡ���ӵĻ�����������
    /// </summary>
    public T GetLinkBaseBlockData<T>(string meta) where T : BlockBaseLinkBean
    {
        //��ȡlink����
        T blockLink = FromMetaData<T>(meta);
        if (blockLink.level == 0)
        {
            //����Լ����ǻ������ӷ���
            return blockLink;
        }
        else
        {
            //��ȡ�������ӷ���
            Vector3Int baseWorldPosition = blockLink.linkBasePosition.GetVector3Int();
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseWorldPosition, out Block baseBlock, out BlockDirectionEnum baseBlockDirection, out Chunk baseChunk);
            BlockBean blockDataBase = baseChunk.GetBlockData(baseWorldPosition - baseChunk.chunkData.positionForWorld);
            return FromMetaData<T>(blockDataBase.meta);
        }
    }

    /// <summary>
    /// ��ȡ��ת�ĽǶ�
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        return BlockShape.GetRotateAngles(direction);
    }

    /// <summary>
    /// ��ȡmeta����
    /// </summary>
    /// <returns></returns>
    public static string ToMetaData<T>(T blockData)
    {
        return JsonUtil.ToJson(blockData);
    }

    public static T FromMetaData<T>(string data)
    {
        return JsonUtil.FromJson<T>(data);
    }
}
