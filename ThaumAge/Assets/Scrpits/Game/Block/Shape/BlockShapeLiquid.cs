using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLiquid : BlockShapeCube
{
    public override void InitData(Block block)
    {
        base.InitData(block);
        uvsAdd = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
        };
    }

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
        {
            base.BaseAddVerts(chunk, localPosition, direction, face, vertsAdd);
        }
        else
        {
            //如果有数据 需要判断是几级水流
            BlockLiquidBean blockLiquid = BlockBaseLiquid.FromMetaData<BlockLiquidBean>(blockData.meta);
            if (blockLiquid == null)
            {
                base.BaseAddVerts(chunk, localPosition, direction, face, vertsAdd);
            }
            else
            {
                if (blockLiquid.level == 1)
                {
                    Vector3[] vertsAddNew = GetAddVertsNew(chunk, localPosition, face, vertsAdd);
                    //获取四周的方块
                    base.BaseAddVerts(chunk, localPosition, direction, face, vertsAddNew);
                }
                else
                {
                    base.BaseAddVerts(chunk, localPosition, direction, face, vertsAdd);
                }
            }
        }
    }

    /// <summary>
    /// 获取新增加的点
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="face"></param>
    /// <param name="vertsAdd"></param>
    /// <returns></returns>
    protected Vector3[] GetAddVertsNew(Chunk chunk, Vector3Int localPosition, DirectionEnum face, Vector3[] vertsAdd)
    {
        if (face == DirectionEnum.Left || face == DirectionEnum.Right)
        {
            Vector3[] vertsAddNew = new Vector3[vertsAdd.Length];
            for (int i = 0; i < vertsAdd.Length; i++)
            {
                if (i == 1 || i == 2)
                {
                    vertsAddNew[i] = vertsAdd[i] - new Vector3(0, 1f, 0);
                }
                else
                {
                    vertsAddNew[i] = vertsAdd[i];
                }
            }
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Forward, out Block forwardBlock, out Chunk forwardChunk);
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Back, out Block backBlock, out Chunk backChunk);
            if (forwardBlock != null && forwardBlock.blockType == block.blockType)
            {
                vertsAddNew[1] = vertsAdd[1];
            }
            if (backBlock != null && backBlock.blockType == block.blockType)
            {
                vertsAddNew[2] = vertsAdd[2];
            }
            return vertsAddNew;
        }
        else if (face == DirectionEnum.Forward || face == DirectionEnum.Back)
        {
            Vector3[] vertsAddNew = new Vector3[vertsAdd.Length];
            for (int i = 0; i < vertsAdd.Length; i++)
            {
                if (i == 1 || i == 2)
                {
                    vertsAddNew[i] = vertsAdd[i] - new Vector3(0, 1f, 0);
                }
                else
                {
                    vertsAddNew[i] = vertsAdd[i];
                }
            }
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Left, out Block leftBlock, out Chunk leftChunk);
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Right, out Block rightBlock, out Chunk rightChunk);
            if (leftBlock != null && leftBlock.blockType == block.blockType)
            {
                vertsAddNew[1] = vertsAdd[1];
            }
            if (rightBlock != null && rightBlock.blockType == block.blockType)
            {
                vertsAddNew[2] = vertsAdd[2];
            }
            return vertsAddNew;
        }
        else if (face == DirectionEnum.UP)
        {
            Vector3[] vertsAddNew = new Vector3[vertsAdd.Length];
            for (int i = 0; i < vertsAdd.Length; i++)
            {
                vertsAddNew[i] = vertsAdd[i] - new Vector3(0, 1f, 0);
            }
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Left, out Block leftBlock, out Chunk leftChunk);
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Right, out Block rightBlock, out Chunk rightChunk);
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Forward, out Block forwardBlock, out Chunk forwardChunk);
            block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Back, out Block backBlock, out Chunk backChunk);
            if (leftBlock != null && leftBlock.blockType == block.blockType)
            {
                vertsAddNew[0] = vertsAdd[0];
                vertsAddNew[1] = vertsAdd[1];
            }

            if (rightBlock != null && rightBlock.blockType == block.blockType)
            {
                vertsAddNew[2] = vertsAdd[2];
                vertsAddNew[3] = vertsAdd[3];
            }

            if (forwardBlock != null && forwardBlock.blockType == block.blockType)
            {
                vertsAddNew[0] = vertsAdd[0];
                vertsAddNew[3] = vertsAdd[3];
            }

            if (backBlock != null && backBlock.blockType == block.blockType)
            {
                vertsAddNew[1] = vertsAdd[1];
                vertsAddNew[2] = vertsAdd[2];
            }

            return vertsAddNew;
        }
        else
        {
            return vertsAdd;
        }
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
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
            case BlockShapeEnum.Liquid:
                if (closeBlock.blockType == block.blockType)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            default:
                return true;
        }
    }
}