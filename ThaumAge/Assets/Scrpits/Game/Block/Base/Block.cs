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
    /// 删除方块mesh
    /// </summary>
    public virtual void RemoveBlockMesh(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, ChunkMeshIndexData meshIndexData)
    {
        //删除该条下标信息
        chunk.chunkMeshData.dicIndexData.Remove(localPosition);
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
    public virtual void Interactive(Vector3Int worldPosition)
    {

    }

    /// <summary>
    /// 构建方块
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
    /// 初始化方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="state">0:创建地形 1：手动设置方块</param>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        CreateBlockModel(chunk, localPosition);
    }

    /// <summary>
    /// 摧毁方块
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition)
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
        WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, this, this, direction, false);
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
        out Block block, out Chunk blockChunk)
    {
        //获取目标的本地坐标
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
            //如果在同一个chunk内
            block = chunk.chunkData.GetBlockForLocal(localPosition);
            blockChunk = chunk;
        }
    }
}
