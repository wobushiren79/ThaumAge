﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShape
{
    public Block block;

    public Vector3[] vertsAdd;
    public Vector2[] uvsAdd;
    public int[] trisAdd;

    public static Vector3[] vertsColliderAdd = new Vector3[]
    {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(0,0,1),

            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
            new Vector3(1,0,1),

            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0),

            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0),

            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
            new Vector3(1,0,0),

            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
    };

    public static int[] trisColliderAdd = new int[]
    {
            0,2,1, 0,3,2,

            4,5,6, 4,6,7,

            8,10,9, 8,11,10,

            12,13,14, 12,14,15,

            16,17,18, 16,18,19,

            20,22,21, 20,23,22
    };

    public static float uvWidth = 1 / 128f;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    public virtual void InitData(Block block)
    {
        this.block = block;
    }

    /// <summary>
    /// 构建方块
    /// </summary>
    /// <param name="block"></param>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public virtual void BuildBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {

    }
    public virtual void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
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
    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd)
    {
        BaseAddTris(chunk, localPosition, direction);
        BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        BaseAddUVs(chunk, localPosition, direction);
    }

    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void BaseAddVerts(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd)
    {

    }

    /// <summary>
    /// 添加UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void BaseAddUVs(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {

    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {

    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk)
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
    /// 获取旋转方向
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="getDirection"></param>
    /// <param name="closeBlock"></param>
    /// <param name="blockChunk"></param>
    public virtual void GetCloseRotateBlockByDirection(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum getDirection, out Block closeBlock, out Chunk blockChunk)
    {
        switch (block.blockInfo.rotate_state)
        {
            case 0:
                //不旋转
                block.GetCloseBlockByDirection(chunk, localPosition, getDirection, out closeBlock, out blockChunk);
                break;
            case 1:
                //旋转
                DirectionEnum rotateDirection = GetRotateDirection(direction, getDirection);
                block.GetCloseBlockByDirection(chunk, localPosition, rotateDirection, out closeBlock, out blockChunk);
                break;
            case 2:
                //旋转
                rotateDirection = GetRotateDirection(direction, getDirection);
                if (rotateDirection == DirectionEnum.Down)
                {
                    rotateDirection = DirectionEnum.UP;
                }
                block.GetCloseBlockByDirection(chunk, localPosition, rotateDirection, out closeBlock, out blockChunk);
                break;
            default:
                closeBlock = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
                blockChunk = null;
                break;
        }
    }

    /// <summary>
    /// 根据本身坐标选择方向
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="getDirection"></param>
    /// <returns></returns>
    public DirectionEnum GetRotateDirection(BlockDirectionEnum direction, DirectionEnum getDirection)
    {
        DirectionEnum targetDirection = DirectionEnum.UP;
        switch (direction)
        {
            case BlockDirectionEnum.UpForward:
                targetDirection = getDirection;
                break;
            case BlockDirectionEnum.UpLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                    case DirectionEnum.Down:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Left;
                        break;
                }
                break;
            case BlockDirectionEnum.UpRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                    case DirectionEnum.Down:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Right;
                        break;
                }
                break;
            case BlockDirectionEnum.UpBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                    case DirectionEnum.Down:
                        targetDirection = getDirection;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Forward;
                        break;
                }
                break;


            case BlockDirectionEnum.DownForward:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Forward;
                        break;
                }
                break;
            case BlockDirectionEnum.DownLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Right;
                        break;
                }
                break;
            case BlockDirectionEnum.DownRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Left;
                        break;
                }
                break;
            case BlockDirectionEnum.DownBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Back;
                        break;
                }
                break;


            case BlockDirectionEnum.LeftForward:
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
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Back;
                        break;
                }
                break;
            case BlockDirectionEnum.LeftLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Down;
                        break;
                }
                break;
            case BlockDirectionEnum.LeftRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.UP;
                        break;
                }
                break;
            case BlockDirectionEnum.LeftBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Forward;
                        break;
                }
                break;


            case BlockDirectionEnum.RightForward:
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
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Back;
                        break;
                }
                break;
            case BlockDirectionEnum.RightLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.UP;
                        break;
                }
                break;
            case BlockDirectionEnum.RightRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Down;
                        break;
                }
                break;
            case BlockDirectionEnum.RightBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Forward;
                        break;
                }
                break;


            case BlockDirectionEnum.ForwardForward:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Down;
                        break;
                }
                break;
            case BlockDirectionEnum.ForwardLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Left;
                        break;
                }
                break;
            case BlockDirectionEnum.ForwardRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Right;
                        break;
                }
                break;
            case BlockDirectionEnum.ForwardBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.UP;
                        break;
                }
                break;


            case BlockDirectionEnum.BackForward:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.UP;
                        break;
                }
                break;
            case BlockDirectionEnum.BackLeft:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Left;
                        break;
                }
                break;
            case BlockDirectionEnum.BackRight:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.UP;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Down;
                        break;
                    case DirectionEnum.Forward:
                        targetDirection = DirectionEnum.Left;
                        break;
                    case DirectionEnum.Back:
                        targetDirection = DirectionEnum.Right;
                        break;
                }
                break;
            case BlockDirectionEnum.BackBack:
                switch (getDirection)
                {
                    case DirectionEnum.UP:
                        targetDirection = DirectionEnum.Forward;
                        break;
                    case DirectionEnum.Down:
                        targetDirection = DirectionEnum.Back;
                        break;
                    case DirectionEnum.Left:
                        targetDirection = DirectionEnum.Right;
                        break;
                    case DirectionEnum.Right:
                        targetDirection = DirectionEnum.Left;
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

    public static Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        Vector3 angles;
        switch (direction)
        {
            case BlockDirectionEnum.UpForward:
                angles = new Vector3(0, 0, 0);
                break;
            case BlockDirectionEnum.UpLeft:
                angles = new Vector3(0, -90, 0);
                break;
            case BlockDirectionEnum.UpRight:
                angles = new Vector3(0, 90, 0);
                break;
            case BlockDirectionEnum.UpBack:
                angles = new Vector3(0, 180, 0);
                break;

            case BlockDirectionEnum.DownForward:
                angles = new Vector3(180, 0, 0);
                break;
            case BlockDirectionEnum.DownLeft:
                angles = new Vector3(180, -90, 0);
                break;
            case BlockDirectionEnum.DownRight:
                angles = new Vector3(180, 90, 0);
                break;
            case BlockDirectionEnum.DownBack:
                angles = new Vector3(180, 180, 0);
                break;

            case BlockDirectionEnum.LeftForward:
                angles = new Vector3(0, 0, 90);
                break;
            case BlockDirectionEnum.LeftLeft:
                angles = new Vector3(90, -90, 0);
                break;
            case BlockDirectionEnum.LeftRight:
                angles = new Vector3(-90, 90, 0);
                break;
            case BlockDirectionEnum.LeftBack:
                angles = new Vector3(0, 180, -90);
                break;

            case BlockDirectionEnum.RightForward:
                angles = new Vector3(0, 0, -90);
                break;
            case BlockDirectionEnum.RightLeft:
                angles = new Vector3(-90, -90, 0);
                break;
            case BlockDirectionEnum.RightRight:
                angles = new Vector3(90, 90, 0);
                break;
            case BlockDirectionEnum.RightBack:
                angles = new Vector3(0, 180, 90);
                break;

            case BlockDirectionEnum.ForwardForward:
                angles = new Vector3(90, 0, 0);
                break;
            case BlockDirectionEnum.ForwardLeft:
                angles = new Vector3(0, -90, -90);
                break;
            case BlockDirectionEnum.ForwardRight:
                angles = new Vector3(0, 90, 90);
                break;
            case BlockDirectionEnum.ForwardBack:
                angles = new Vector3(-90, 180, 0);
                break;

            case BlockDirectionEnum.BackForward:
                angles = new Vector3(-90, 0, 0);
                break;
            case BlockDirectionEnum.BackLeft:
                angles = new Vector3(0, -90, 90);
                break;
            case BlockDirectionEnum.BackRight:
                angles = new Vector3(0, 90, -90);
                break;
            case BlockDirectionEnum.BackBack:
                angles = new Vector3(90, 180, 0);
                break;

            default:
                angles = new Vector3(0, 0, 0);
                break;
        }
        return angles;
    }

    /// <summary>
    /// 旋转点位
    /// </summary>
    /// <param name="vert"></param>
    /// <returns></returns>
    public virtual Vector3 RotatePosition(BlockDirectionEnum direction, Vector3 position, Vector3 centerPosition)
    {
        if (block.blockInfo.rotate_state == 0)
        {
            //不旋转
            return position;
        }
        else if (block.blockInfo.rotate_state == 1)
        {
            //已中心点旋转
            Vector3 angles = GetRotateAngles(direction);
            //旋转6面
            Vector3 rotatePosition = VectorUtil.GetRotatedPosition(centerPosition, position, angles);
            return rotatePosition;
        }
        else if (block.blockInfo.rotate_state == 2)
        {
            //已中心点旋转
            Vector3 angles;
            switch (direction)
            {
                case BlockDirectionEnum.UpForward:
                    angles = new Vector3(0, 0, 0);
                    break;
                case BlockDirectionEnum.UpLeft:
                    angles = new Vector3(0, 90, 0);
                    break;
                case BlockDirectionEnum.UpRight:
                    angles = new Vector3(0, -90, 0);
                    break;
                case BlockDirectionEnum.UpBack:
                    angles = new Vector3(0, 180, 0);
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
    /// 添加顶点
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listVerts"></param>
    /// <param name="vert"></param>
    public virtual void AddVert(Vector3Int localPosition, BlockDirectionEnum direction, List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(direction, vert, GetCenterPosition(localPosition)));
    }

    public virtual void AddVert(Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] arrayVerts, int indexVerts, Vector3 vert)
    {
        arrayVerts[indexVerts] = RotatePosition(direction, vert, GetCenterPosition(localPosition));
    }

    public virtual void AddVerts(Vector3Int localPosition, BlockDirectionEnum direction, List<Vector3> listVerts, Vector3[] vertsAdd)
    {
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            listVerts.Add(RotatePosition(direction, localPosition + vertsAdd[i], GetCenterPosition(localPosition)));
        }
    }

    /// <summary>
    /// 增加UV
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
    /// 增加三角下标顺序
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

    public Vector3 GetCenterPosition(Vector3Int localPosition)
    {
        return new Vector3(localPosition.x + 0.5f, localPosition.y + 0.5f, localPosition.z + 0.5f);
    }
}