using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block
{
    public Chunk chunk;    //所属Chunk
    public Vector3Int localPosition; //Chunk内的坐标
    public Vector3Int worldPosition; //世界坐标
    public BlockBean blockData; //方框数据

    public Vector3 centerPosition;

    protected BlockInfoBean _blockInfo;//方块信息
    public BlockInfoBean blockInfo
    {
        get
        {
            if (_blockInfo == null)
            {
                _blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.blockId);
            }
            return _blockInfo;
        }
    }
    protected float uvWidth = 1 / 128f;


    public Block()
    {

    }

    public Block(BlockTypeEnum blockType)
    {
        if (blockData == null)
            blockData = new BlockBean(blockType, Vector3Int.zero, Vector3Int.zero);
    }

    public virtual void RefreshBlock()
    {

    }

    public virtual void RefreshBlockRange()
    {
        Block upBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.up);
        upBlock?.RefreshBlock();
        Block downBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.down);
        downBlock?.RefreshBlock();
        Block leftBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.left);
        leftBlock?.RefreshBlock();
        Block rightBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.right);
        rightBlock?.RefreshBlock();
        Block forwardBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.forward);
        forwardBlock?.RefreshBlock();
        Block backBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition + Vector3Int.back);
        backBlock?.RefreshBlock();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    public virtual void SetData(Chunk chunk, Vector3Int localPosition, BlockBean blockData)
    {
        this.chunk = chunk;
        this.localPosition = localPosition;
        this.worldPosition = localPosition + chunk.worldPosition;
        this.blockData = blockData;
        this.centerPosition = localPosition + new Vector3(0.5f, 0.5f, 0.5f);
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Vector3Int position)
    {
        if (position.y < 0) return false;
        //检测旋转
        Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition(position, localPosition));
        //获取方块
        Block block = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition);
        if (block == null)
            return false;
        BlockShapeEnum blockShape = block.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            default:
                return true;
        }
    }

    /// <summary>
    /// 构建方块
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public virtual void BuildBlock(Chunk.ChunkData chunkData)
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
    public virtual void BuildFace(BlockBean blockData, Vector3 corner, Chunk.ChunkData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, chunkData);
        AddUVs(blockData, chunkData);
    }

    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void AddVerts(Vector3 corner, Chunk.ChunkData chunkData)
    {

    }
    public virtual void AddVert(List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(vert, centerPosition));
    }

    /// <summary>
    /// 添加UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void AddUVs(BlockBean blockData, Chunk.ChunkData chunkData)
    {

    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void AddTris(Chunk.ChunkData chunkData)
    {

    }

    /// <summary>
    /// 旋转点位
    /// </summary>
    /// <param name="vert"></param>
    /// <returns></returns>
    public virtual Vector3 RotatePosition(Vector3 position, Vector3 centerPosition)
    {
        if (blockInfo.rotate_state == 0)
        {
            //不旋转
            return position;
        }
        else if (blockInfo.rotate_state == 1)
        {
            //已中心点旋转
            DirectionEnum direction = blockData.GetDirection();
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
                    angles = new Vector3(90, 0, 0);
                    break;
                case DirectionEnum.Back:
                    angles = new Vector3(-90, 0, 0);
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



}
