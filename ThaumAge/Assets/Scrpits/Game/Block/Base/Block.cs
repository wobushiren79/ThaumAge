using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public abstract class Block
{
    public BlockTypeEnum blockType;    //方块类型

    protected float uvWidth = 1 / 128f;

    public Vector3 GetCenterPosition(Vector3Int localPosition)
    {
        return localPosition + new Vector3(0.5f, 0.5f, 0.5f);
    }

    protected BlockInfoBean _blockInfo;//方块信息

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
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="closeDirection"></param>
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection, out BlockTypeEnum closeBlock)
    {

        closeBlock = BlockTypeEnum.None;
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk.chunkData.positionForWorld + localPosition, direction, closeDirection, out closeBlock, out bool hasChunk);
        if (closeBlock == BlockTypeEnum.None)
        {
            if (hasChunk)
            {
                //只是空气方块
                return true;
            }
            else
            {
                //还没有生成chunk
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

    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        return CheckNeedBuildFace(chunk, localPosition, direction, closeDirection, out BlockTypeEnum closeBlock);
    }

    /// <summary>
    /// 构建方块
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {

    }
    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {

    }
    /// <summary>
    /// 构建面
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildFace(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Chunk.ChunkRenderData chunkData)
    {
        AddTris(chunkData);
        AddVerts(localPosition, direction, corner, chunkData);
        AddUVs(chunkData);
    }

    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Chunk.ChunkRenderData chunkData)
    {

    }
    public virtual void AddVert(Vector3Int localPosition, DirectionEnum direction, List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(direction, vert, GetCenterPosition(localPosition)));
    }

    /// <summary>
    /// 添加UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void AddUVs(Chunk.ChunkRenderData chunkData)
    {

    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void AddTris(Chunk.ChunkRenderData chunkData)
    {

    }

    public virtual void GetCloseRotateBlockByDirection(Vector3Int worldPosition, DirectionEnum direction, DirectionEnum getDirection, out BlockTypeEnum closeBlock, out bool hasChunk)
    {
        if (blockInfo.rotate_state == 0)
        {
            //不旋转
            GetCloseBlockByDirection(worldPosition, getDirection, out closeBlock, out hasChunk);
        }
        else if (blockInfo.rotate_state == 1)
        {
            //旋转
            DirectionEnum rotateDirection = GetRotateDirection(direction, getDirection);
            GetCloseBlockByDirection(worldPosition, rotateDirection, out closeBlock, out hasChunk);
        }
        else
        {
            closeBlock = BlockTypeEnum.None;
            hasChunk = false;
        }
    }

    /// <summary>
    /// 获取不同方向的方块
    /// </summary>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="hasChunk"></param>
    public virtual void GetCloseBlockByDirection(Vector3Int worldPosition, DirectionEnum getDirection, out BlockTypeEnum blockType, out bool hasChunk)
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
    /// 根据本身坐标选择方向
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
    /// 旋转点位
    /// </summary>
    /// <param name="vert"></param>
    /// <returns></returns>
    public virtual Vector3 RotatePosition(DirectionEnum direction, Vector3 position, Vector3 centerPosition)
    {
        if (blockInfo.rotate_state == 0)
        {
            //不旋转
            return position;
        }
        else if (blockInfo.rotate_state == 1)
        {
            //已中心点旋转
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
            //旋转6面
            Vector3 rotatePosition = VectorUtil.GetRotatedPosition(centerPosition, position, angles);
            return rotatePosition;
        }
        return position;
    }

    /// <summary>
    /// 初始化方块
    /// </summary>
    public virtual void InitBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        CreateBlockModel(chunk, localPosition, direction);
    }

    /// <summary>
    /// 摧毁方块
    /// </summary>
    public virtual void DestoryBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        DestoryBlockModel(chunk, localPosition, direction);
    }

    /// <summary>
    /// 事件方块更新
    /// </summary>
    public virtual void EventBlockUpdate(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// 创建方块的模型
    /// </summary>
    public virtual void CreateBlockModel(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        //如果有模型。则创建模型
        if (!CheckUtil.StringIsNull(blockInfo.model_name))
        {
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    /// <summary>
    /// 删除方块的模型
    /// </summary>
    public virtual void DestoryBlockModel(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        //摧毁模型
        chunk.listBlockModelDestroy.Enqueue(localPosition);
    }

    /// <summary>
    /// 刷新方块
    /// </summary>
    public virtual void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// 刷新周围方块
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
    /// 刷新靠近的方块
    /// </summary>
    /// <param name="closeWorldPosition"></param>
    public virtual void RefreshBlockClose(Vector3Int closeWorldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closeWorldPosition, out BlockTypeEnum closeBlockType, out DirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk != null)
        {
            Block upBlock = BlockHandler.Instance.manager.GetRegisterBlock(closeBlockType);
            upBlock?.RefreshBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, closeBlockDirection);
        }
    }

    /// <summary>
    /// 获取下标
    /// </summary>
    public static int GetIndex(Vector3Int localPosition, int chunkWidth, int chunkHeight)
    {
        return localPosition.x * chunkWidth * chunkHeight + localPosition.y * chunkWidth + localPosition.z;
    }
}
