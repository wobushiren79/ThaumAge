using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public BlockTypeEnum blockType;//方块类型

    public BlockShape blockShape;//方块的形状
    public BlockInfoBean blockInfo;//方块信息

    public Block()
    {

    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    public virtual void SetData(BlockTypeEnum blockType)
    {
        this.blockType = blockType;
        //获取方块数据
        blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockType);
        //获取方块形状
        BlockShapeEnum blockShapeType = blockInfo.GetBlockShape();
        //获取形状数据
        blockShape = BlockHandler.Instance.manager.GetRegisterBlockShape(this, blockShapeType);
    }

    /// <summary>
    /// 获取区块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Chunk GetChunk(Vector3Int worldPosition)
    {
        return WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
    }

    /// <summary>
    /// 获取方块实例模型
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GameObject GetBlockObj(Vector3Int worldPosition)
    {
        return BlockHandler.Instance.GetBlockObj(worldPosition); ;
    }

    /// <summary>
    /// 获取方块的方位
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
    /// 获取周围的方块
    /// </summary>
    public void GetRoundBlock(Vector3Int worldPosition,
        out Block upBlock, out Block downBlock, out Block leftBlock, out Block rightBlock, out Block forwardBlock, out Block backBlock)
    {
        //获取周围的方块 并触发互动
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
    /// 互动
    /// </summary>
    public virtual void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {

    }

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="user"></param>
    public virtual void OnCollision(CreatureTypeEnum creatureType, GameObject targetObj, Vector3Int worldPosition, DirectionEnum direction)
    {
        if (creatureType == CreatureTypeEnum.Player)
            GameControlHandler.Instance.manager.controlForPlayer.ChangeGroundType(0);
    }

    /// <summary>
    /// 正前方碰撞
    /// </summary>
    /// <param name="user"></param>
    /// <param name="worldPosition"></param>
    /// <param name="direction"></param>
    /// <param name="raycastHit"></param>
    public virtual void OnCollisionForward(GameObject user, Vector3Int worldPosition, RaycastHit raycastHit)
    {

    }

    /// <summary>
    /// 角色的视角摄像头
    /// </summary>
    public virtual void OnCollisionForPlayerCamera(Camera camera, Vector3Int worldPosition)
    {
        CameraHandler.Instance.SetCameraUnderLiquid(0);
    }

    /// <summary>
    /// 构建方块
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
    /// 初始化方块 异步的
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:创建地形 1：手动设置方块</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        CreateBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// 摧毁方块-设置新方块之前
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        DestoryBlockModel(chunk, localPosition);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.SecTiny);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// 事件方块更新_0.2秒
    /// </summary>
    public virtual void EventBlockUpdateForSecTiny(Chunk chunk, Vector3Int localPosition)
    {

    }
    /// <summary>
    /// 事件方块更新_1秒
    /// </summary>
    public virtual void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {

    }
    /// <summary>
    /// 事件方块更新_60秒
    /// </summary>
    public virtual void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {

    }

    /// <summary>
    /// 创建方块的模型
    /// </summary>
    public virtual void CreateBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //如果有模型。则创建模型
        if (!blockInfo.model_name.IsNull())
        {
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    /// <summary>
    /// 创建方块模型成功
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
    /// 删除方块的模型
    /// </summary>
    public virtual void DestoryBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //摧毁模型
        chunk.listBlockModelDestroy.Enqueue(localPosition);
    }

    /// <summary>
    /// 刷新方块模型
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition)
    {

    }

    /// <summary>
    /// 刷新方块
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction,int updateChunkType = 1)
    {
        //更新方块
        WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, updateChunkType);
    }

    /// <summary>
    /// 刷新周围方块
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
    /// 刷新靠近的方块
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
    /// 获取破坏掉落
    /// </summary>
    /// <returns></returns>
    public virtual List<ItemsBean> GetDropItems(BlockBean blockData = null)
    {
        return ItemsHandler.Instance.GetItemsDrop(blockInfo.items_drop);
    }

    /// <summary>
    /// 获取下标
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }

    /// <summary>
    /// 该方块被当成道具使用时 获取使用道具时的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        return curMeta;
    }

    /// <summary>
    /// 被使用
    /// </summary>
    public virtual void TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {

    }

    /// <summary>
    /// 被破坏
    /// </summary>
    public virtual void TargetBreakBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int worldPosition)
    {

    }

    /// <summary>
    /// 该方块被当成道具使用时，用于方块的放置或者其他处理
    /// </summary>
    public virtual void ItemUse(Item useItem, ItemsBean itemsData,
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk,
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk,
        BlockDirectionEnum direction, string metaData)
    {
        //更新方块并 添加更新区块
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
    /// 道具-指向
    /// </summary>
    public virtual void ItemUseForSightTarget(Vector3Int targetWorldPosition)
    {
        //展示目标位置
        GameHandler.Instance.manager.playerTargetBlock.Show(targetWorldPosition, this, blockInfo.interactive_state == 1);
    }

    /// <summary>
    /// 获取靠近坐标
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
    /// 获取不同方向的方块
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum getDirection,
        out Block block, out Chunk blockChunk, out Vector3Int closeLocalPosition)
    {
        //获取目标的本地坐标
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
            //如果在同一个chunk内
            block = chunk.chunkData.GetBlockForLocal(localPosition);
            blockChunk = chunk;
        }
    }

    /// <summary>
    /// 创建链接的方块
    /// </summary>
    public virtual void CreateLinkBlock(Chunk chunk, Vector3Int localPosition, List<Vector3Int> listLink)
    {
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null)
        {
            BlockMetaBaseLink blockMetaLinkData = FromMetaData<BlockMetaBaseLink>(blockData.meta);
            if (blockMetaLinkData != null)
            {
                //如果是子级 则不生成
                if (blockMetaLinkData.level > 0)
                    return;
            }
        }
        //判断是否在指定的link坐标上有其他方块，如果有则生成道具
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
            //创建道具
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
            ItemsHandler.Instance.CreateItemCptDrop(this, chunk, localPosition + chunk.chunkData.positionForWorld);
        }
        else
        {
            //创建link方块
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
    /// 删除链接方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listLink"></param>
    public void DestoryLinkBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, List<Vector3Int> listLink)
    {
        Vector3Int baseWorldPosition = localPosition + chunk.chunkData.positionForWorld;
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //延迟一帧执行 等当前方块已经删除了
        chunk.chunkComponent.WaitExecuteEndOfFrame(1, () =>
        {
            if (blockData != null)
            {
                BlockMetaDoor blockDoorData = FromMetaData<BlockMetaDoor>(blockData.meta);
                if (blockDoorData != null)
                {
                    //如果是子级 则不生成
                    if (blockDoorData.level > 0)
                    {
                        baseWorldPosition = blockDoorData.linkBasePosition.GetVector3Int();
                        //删除基础方块
                        chunk.SetBlockForWorld(baseWorldPosition, BlockTypeEnum.None);
                    }
                }
            }

            //如果不是子级 则说明是基础方块 从这里开始删除方块
            for (int i = 0; i < listLink.Count; i++)
            {
                Vector3Int linkPosition = listLink[i];
                Vector3Int closeWorldPosition = baseWorldPosition + linkPosition;
                chunk.SetBlockForWorld(closeWorldPosition, BlockTypeEnum.None);
            }
        });
    }

    /// <summary>
    /// 保存链接数据
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
    /// 获取连接的基础方块数据
    /// </summary>
    public T GetLinkBaseBlockData<T>(string meta) where T : BlockMetaBaseLink
    {
        //获取link数据
        T blockLink = FromMetaData<T>(meta);
        if (blockLink.level == 0)
        {
            //如果自己就是基础连接方块
            return blockLink;
        }
        else
        {
            //获取基础连接方块
            Vector3Int baseWorldPosition = blockLink.linkBasePosition.GetVector3Int();
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseWorldPosition, out Block baseBlock, out BlockDirectionEnum baseBlockDirection, out Chunk baseChunk);
            BlockBean blockDataBase = baseChunk.GetBlockData(baseWorldPosition - baseChunk.chunkData.positionForWorld);
            return FromMetaData<T>(blockDataBase.meta);
        }
    }

    /// <summary>
    /// 获取旋转的角度
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        return BlockShape.GetRotateAngles(direction);
    }

    /// <summary>
    /// 放置物品
    /// </summary>
    /// <returns></returns>
    public virtual bool SetItems(Chunk targetChunk, Block targetBlock, BlockDirectionEnum targetBlockDirection, Vector3Int blockWorldPosition, ItemsBean itemData)
    {
        return false;
    }

    /// <summary>
    /// 获取meta数据
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
