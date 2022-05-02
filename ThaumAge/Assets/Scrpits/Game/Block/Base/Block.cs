using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public BlockTypeEnum blockType;    //方块类型
    protected BlockInfoBean _blockInfo;//方块信息
    protected BlockShape _blockShape;//方块的形状

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
    /// 设置数据
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
    /// <param name="worldPosition"></param>
    /// <param name="upBlock"></param>
    /// <param name="downBlock"></param>
    /// <param name="leftBlock"></param>
    /// <param name="rightBlock"></param>
    /// <param name="forwardBlock"></param>
    /// <param name="backBlock"></param>
    public void GetRoundBlock(Vector3Int worldPosition, out Block upBlock, out Block downBlock, out Block leftBlock, out Block rightBlock, out Block forwardBlock, out Block backBlock)
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

    /// <summary>
    /// 删除方块mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, ChunkMeshIndexData meshIndexData)
    {
        //删除该条下标信息
        //chunk.chunkMeshData.dicIndexData.Remove(localPosition);

        //移除对应三角数据
        List<int> tris = chunk.chunkMeshData.dicTris[blockInfo.material_type];
        MeshTrisRemove(tris, meshIndexData.trisStartIndex, meshIndexData.trisCount);
        //如果有碰撞 还需要删除碰撞
        if (blockInfo.collider_state == 1)
        {
            //移除对应三角数据
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            MeshTrisRemove(trisCollider, meshIndexData.trisColliderStartIndex, meshIndexData.trisColliderCount);
        }
        //如果有触发 还需要删除触发
        if (blockInfo.trigger_state == 1)
        {
            //移除对应三角数据
            List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
            MeshTrisRemove(trisTrigger, meshIndexData.trisColliderStartIndex, meshIndexData.trisColliderCount);
        }
    }

    /// <summary>
    /// 三角形删除处理
    /// </summary>
    protected virtual void MeshTrisRemove(List<int> listTris, int trisStartIndex, int trisCount)
    {
        for (int i = 0; i < trisCount; i++)
        {
            listTris[trisStartIndex + i] = 0;
        }
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
    public virtual void OnCollision(GameObject user, Vector3Int worldPosition, DirectionEnum direction)
    {

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
    /// 初始化方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:创建地形 1：手动设置方块</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition,int state)
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
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
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
    /// 刷新方块
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {       
        //更新方块
        WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk,1);
    }

    /// <summary>
    /// 刷新周围方块
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
    /// 刷新靠近的方块
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
    /// 获取破坏掉落
    /// </summary>
    /// <returns></returns>
    public virtual List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        return blockInfo.GetItemsDrop();
    }

    /// <summary>
    /// 获取下标
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }

    /// <summary>
    /// 获取使用道具时的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public virtual string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        return curMeta;
    }

    /// <summary>
    /// 道具使用，用于方块的放置或者其他处理
    /// </summary>
    public virtual void ItemUse(
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk,
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk,
        BlockDirectionEnum direction , string metaData)
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
    /// 获取不同方向的方块
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(Chunk chunk, Vector3Int localPosition, DirectionEnum getDirection,
        out Block block, out Chunk blockChunk,out Vector3Int closeLocalPosition)
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
            BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
            if (blockDoorData != null)
            {
                //如果是子级 则不生成
                if (blockDoorData.level > 0)
                    return;
            }
        }
        //判断是否在指定的link坐标上有其他方块，如果有则生成道具
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
                BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
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
    public void SaveLinkBaseBlockData<T>(Vector3Int baseWorldPosition, T data) where T : BlockBaseLinkBean
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseWorldPosition, out Block baseBlock, out Chunk baseChunk);
        BlockBean baseBlockData = baseChunk.GetBlockDataForWorldPosition(baseWorldPosition);
        baseBlockData.meta = ToMetaData(data);
        baseChunk.SetBlockData(baseBlockData);
    }

    /// <summary>
    /// 获取连接的基础方块数据
    /// </summary>
    public T GetLinkBaseBlockData<T>(string meta) where T : BlockBaseLinkBean
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
