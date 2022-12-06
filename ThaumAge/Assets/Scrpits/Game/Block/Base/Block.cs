using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public BlockTypeEnum blockType;//��������

    public BlockShape blockShape;//�������״
    public BlockInfoBean blockInfo;//������Ϣ

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
        //��ȡ��������
        blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockType);
        //��ȡ������״
        BlockShapeEnum blockShapeType = blockInfo.GetBlockShape();
        //��ȡ��״����
        blockShape = BlockHandler.Instance.manager.GetRegisterBlockShape(this, blockShapeType);
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
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
    public void GetRoundBlock(Vector3Int worldPosition,
        out Block upBlock, out Block downBlock, out Block leftBlock, out Block rightBlock, out Block forwardBlock, out Block backBlock)
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

    public List<Block> GetRoundBlock(Vector3Int worldPosition, int range = 1)
    {
        List<Block> listBlock = new List<Block>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                for (int z = -range; z <= range; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;
                    Vector3Int targetPosition = worldPosition + new Vector3Int(x, y, z);
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out Chunk targetChunk);
                    if (targetChunk != null && targetBlock != null)
                        listBlock.Add(targetBlock);
                }
            }
        }
        return listBlock;
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
    public virtual void OnCollision(CreatureTypeEnum creatureType, GameObject targetObj, Vector3Int worldPosition, DirectionEnum direction)
    {
        if (creatureType == CreatureTypeEnum.Player)
            GameControlHandler.Instance.manager.controlForPlayer.ChangeGroundType(0);
    }

    /// <summary>
    /// ��ǰ����ײ
    /// </summary>
    /// <param name="user"></param>
    /// <param name="worldPosition"></param>
    /// <param name="direction"></param>
    /// <param name="raycastHit"></param>
    public virtual void OnCollisionForward(GameObject user, Vector3Int worldPosition, RaycastHit raycastHit)
    {

    }

    /// <summary>
    /// ��ɫ���ӽ�����ͷ
    /// </summary>
    public virtual void OnCollisionForPlayerCamera(Camera camera, Vector3Int worldPosition)
    {
        CameraHandler.Instance.SetCameraUnderLiquid(0);
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
    /// ��ʼ������ �첽��
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:�������� 1���ֶ����÷���</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
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
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.SecTiny);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// �¼��������_0.2��
    /// </summary>
    public virtual void EventBlockUpdateForSecTiny(Chunk chunk, Vector3Int localPosition)
    {

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
        BlockTypeComponent blockTypeComponent = obj.GetComponent<BlockTypeComponent>();
        if (blockTypeComponent != null)
        {
            blockTypeComponent.SetBlockWorldPosition(chunk.chunkData.positionForWorld + localPosition);
        }
        RefreshObjModel(chunk, localPosition);
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
    /// ˢ�·���ģ��
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition)
    {

    }

    /// <summary>
    /// ˢ�·���
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction,int updateChunkType = 1)
    {
        //���·���
        WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, updateChunkType);
    }

    /// <summary>
    /// ˢ����Χ����
    /// </summary>
    public virtual void RefreshBlockRange(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int updateChunkType = 1)
    {
        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;

        RefreshBlockClose(worldPosition + Vector3Int.up, updateChunkType);
        RefreshBlockClose(worldPosition + Vector3Int.down, updateChunkType);
        RefreshBlockClose(worldPosition + Vector3Int.left, updateChunkType);
        RefreshBlockClose(worldPosition + Vector3Int.right, updateChunkType);
        RefreshBlockClose(worldPosition + Vector3Int.forward, updateChunkType);
        RefreshBlockClose(worldPosition + Vector3Int.back, updateChunkType);
    }

    /// <summary>
    /// ˢ�¿����ķ���
    /// </summary>
    /// <param name="closeWorldPosition"></param>
    public virtual void RefreshBlockClose(Vector3Int closeWorldPosition, int updateChunkType = 1)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out Block closeBlock, out BlockDirectionEnum direction, out Chunk closeChunk);
        if (closeChunk != null && closeBlock != null && closeBlock.blockType != BlockTypeEnum.None)
        {
            closeBlock?.RefreshBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, direction, updateChunkType);
        }
    }

    /// <summary>
    /// ��ȡ�ƻ�����
    /// </summary>
    /// <returns></returns>
    public virtual List<ItemsBean> GetDropItems(BlockBean blockData = null)
    {
        return ItemsHandler.Instance.GetItemsDrop(blockInfo.items_drop);
    }

    /// <summary>
    /// ��ȡ�±�
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }

    /// <summary>
    /// �÷��鱻���ɵ���ʹ��ʱ ��ȡʹ�õ���ʱ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        return curMeta;
    }

    /// <summary>
    /// ��ʹ��
    /// </summary>
    public virtual void TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {

    }

    /// <summary>
    /// ���ƻ�
    /// </summary>
    public virtual void TargetBreakBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int worldPosition)
    {

    }

    /// <summary>
    /// �÷��鱻���ɵ���ʹ��ʱ�����ڷ���ķ��û�����������
    /// </summary>
    public virtual void ItemUse(Item useItem, ItemsBean itemsData,
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk,
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk,
        BlockDirectionEnum direction, string metaData)
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
                if ((int)direction > 20 && (int)direction < 30)
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

    /// <summary>
    /// ����-ָ��
    /// </summary>
    public virtual void ItemUseForSightTarget(Vector3Int targetWorldPosition)
    {
        //չʾĿ��λ��
        GameHandler.Instance.manager.playerTargetBlock.Show(targetWorldPosition, this, blockInfo.interactive_state == 1);
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    public Vector3Int GetClosePositionByDirection(DirectionEnum getDirection, Vector3Int position)
    {
        switch (getDirection)
        {
            case DirectionEnum.UP:
                return position.AddY(1);
            case DirectionEnum.Down:
                return position.AddY(-1);
            case DirectionEnum.Left:
                return position.AddX(-1);
            case DirectionEnum.Right:
                return position.AddX(1);
            case DirectionEnum.Forward:
                return position.AddZ(-1);
            case DirectionEnum.Back:
                return position.AddZ(1);
            default:
                return position;
        }
    }

    public Vector3Int GetCloseOffsetByDirection(DirectionEnum getDirection)
    {
        switch (getDirection)
        {
            case DirectionEnum.UP:
                return Vector3Int.up;
            case DirectionEnum.Down:
                return Vector3Int.down;
            case DirectionEnum.Left:
                return Vector3Int.left;
            case DirectionEnum.Right:
                return Vector3Int.right;
            case DirectionEnum.Forward:
                return Vector3Int.back;
            case DirectionEnum.Back:
                return Vector3Int.forward;
            default:
                return Vector3Int.zero;
        }
    }

    /// <summary>
    /// ��ȡ��ͬ����ķ���
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum getDirection,
        out Block block, out Chunk blockChunk, out Vector3Int closeLocalPosition)
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
            BlockMetaBaseLink blockMetaLinkData = FromMetaData<BlockMetaBaseLink>(blockData.meta);
            if (blockMetaLinkData != null)
            {
                //������Ӽ� ������
                if (blockMetaLinkData.level > 0)
                    return;
            }
        }
        //�ж��Ƿ���ָ����link���������������飬����������ɵ���
        bool hasBlock = false;
        BlockDirectionEnum blockDirection = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
        Vector3 blockAngleRotate = GetRotateAngles(blockDirection);
        for (int i = 0; i < listLink.Count; i++)
        {
            Vector3Int linkPosition = listLink[i];
            Vector3 linkPositionRotate = VectorUtil.GetRotatedPosition(Vector3.zero, linkPosition, blockAngleRotate);
            Vector3Int closeWorldPosition = localPosition + chunk.chunkData.positionForWorld + Vector3Int.RoundToInt(linkPositionRotate);

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
                Vector3 linkPositionRotate = VectorUtil.GetRotatedPosition(Vector3.zero, linkPosition, blockAngleRotate);
                Vector3Int closeWorldPosition = localPosition + chunk.chunkData.positionForWorld + Vector3Int.RoundToInt(linkPositionRotate);
                BlockMetaBaseLink blockMetaLinkData = new BlockMetaBaseLink();
                blockMetaLinkData.level = 1;
                blockMetaLinkData.linkBasePosition = new Vector3IntBean(localPosition + chunk.chunkData.positionForWorld);
                chunk.SetBlockForWorld(closeWorldPosition, BlockTypeEnum.LinkChild, blockDirection, ToMetaData(blockMetaLinkData));
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
                BlockMetaDoor blockDoorData = FromMetaData<BlockMetaDoor>(blockData.meta);
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
    public void SaveLinkBaseBlockData<T>(Vector3Int baseWorldPosition, T data) where T : BlockMetaBaseLink
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseWorldPosition, out Block baseBlock, out Chunk baseChunk);
        BlockBean baseBlockData = baseChunk.GetBlockDataForWorldPosition(baseWorldPosition);
        baseBlockData.meta = ToMetaData(data);
        baseChunk.SetBlockData(baseBlockData);
    }

    /// <summary>
    /// ��ȡ���ӵĻ�����������
    /// </summary>
    public T GetLinkBaseBlockData<T>(string meta) where T : BlockMetaBaseLink
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
    /// ������Ʒ
    /// </summary>
    /// <returns></returns>
    public virtual bool SetItems(Chunk targetChunk, Block targetBlock, BlockDirectionEnum targetBlockDirection, Vector3Int blockWorldPosition, ItemsBean itemData)
    {
        return false;
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
