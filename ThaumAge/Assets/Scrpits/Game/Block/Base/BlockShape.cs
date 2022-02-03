using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShape
{
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

    public Vector3[] vertsAdd;
    public int[] trisAdd;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    public virtual void InitData(Block block)
    {

    }

    /// <summary>
    /// 构建方块
    /// </summary>
    /// <param name="block"></param>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public virtual void BuildBlock(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }
    public virtual void BuildBlockNoCheck(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
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
    public virtual void BuildFace(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        BaseAddTris(block, chunk, localPosition, direction);
        BaseAddVerts(block, chunk, localPosition, direction, vertsAdd);
        BaseAddUVs(block, chunk, localPosition, direction);
    }

    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public virtual void BaseAddVerts(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {

    }

    /// <summary>
    /// 添加UV
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="uvs"></param>
    public virtual void BaseAddUVs(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {

    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public virtual void BaseAddTris(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
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
    public virtual bool CheckNeedBuildFace(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(block, chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
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
    public virtual void GetCloseRotateBlockByDirection(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum getDirection, out Block closeBlock, out Chunk blockChunk)
    {
        if (block.blockInfo.rotate_state == 0)
        {
            //不旋转
            block.GetCloseBlockByDirection(chunk, localPosition, getDirection, out closeBlock, out blockChunk);
        }
        else if (block.blockInfo.rotate_state == 1)
        {
            //旋转
            DirectionEnum rotateDirection = GetRotateDirection(direction, getDirection);
            block.GetCloseBlockByDirection(chunk, localPosition, rotateDirection, out closeBlock, out blockChunk);
        }
        else
        {
            closeBlock = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            blockChunk = null;
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
    public virtual Vector3 RotatePosition(Block block, DirectionEnum direction, Vector3 position, Vector3 centerPosition)
    {
        if (block.blockInfo.rotate_state == 0)
        {
            //不旋转
            return position;
        }
        else if (block.blockInfo.rotate_state == 1)
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
    /// 添加顶点
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="listVerts"></param>
    /// <param name="vert"></param>
    public virtual void AddVert(Block block, Vector3Int localPosition, DirectionEnum direction, List<Vector3> listVerts, Vector3 vert)
    {
        listVerts.Add(RotatePosition(block, direction, vert, GetCenterPosition(localPosition)));
    }

    public virtual void AddVert(Block block, Vector3Int localPosition, DirectionEnum direction, Vector3[] arrayVerts, int indexVerts, Vector3 vert)
    {
        arrayVerts[indexVerts] = RotatePosition(block, direction, vert, GetCenterPosition(localPosition));
    }

    public virtual void AddVerts(Block block, Vector3Int localPosition, DirectionEnum direction, List<Vector3> listVerts, Vector3[] vertsAdd)
    {
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            listVerts.Add(RotatePosition(block, direction, localPosition + vertsAdd[i], GetCenterPosition(localPosition)));
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