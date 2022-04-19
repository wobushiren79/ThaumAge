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
    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        //检测上方 如果上方
        chunk.GetBlockForLocal(localPosition + Vector3Int.up, out Block upBlock, out BlockDirectionEnum upDirection, out Chunk upChunk);
        if (upBlock == null || upBlock.blockType == BlockTypeEnum.None)
        {
            Vector3[] vertsAddLiquid = new Vector3[vertsAdd.Length];
            for (int i = 0; i < vertsAdd.Length; i++)
            {
                vertsAddLiquid[i] = new Vector3(vertsAdd[i].x, vertsAdd[i].y * (7f / 8f), vertsAdd[i].z);
            }
            BaseAddVertsUVsTrisDetails(chunk, localPosition, direction, face, vertsAddLiquid, uvsAdd, colorsAdd);
        }
        else
        {
            BaseAddVertsUVsTrisDetails(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
        }
    }

    protected virtual void BaseAddVertsUVsTrisDetails(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null)
        {
            BlockLiquidBean blockLiquid = Block.FromMetaData<BlockLiquidBean>(blockData.meta);
            if (blockLiquid != null)
            {
                if (blockLiquid.level == 1)
                {
                    Vector3[] vertsAddNew = GetAddVertsNew(chunk, localPosition, face, vertsAdd);
                    //获取四周的方块
                    base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
                    return;
                }
            }
        }
        base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
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

            GetVertsAddNewForRange(chunk, DirectionEnum.Forward, localPosition, vertsAddNew, vertsAdd, 1);
            GetVertsAddNewForRange(chunk, DirectionEnum.Back, localPosition, vertsAddNew, vertsAdd, 2);
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

            GetVertsAddNewForRange(chunk, DirectionEnum.Left, localPosition, vertsAddNew, vertsAdd, 1);
            GetVertsAddNewForRange(chunk, DirectionEnum.Right, localPosition, vertsAddNew, vertsAdd, 2);
            return vertsAddNew;
        }
        else if (face == DirectionEnum.UP)
        {
            Vector3[] vertsAddNew = new Vector3[vertsAdd.Length];
            for (int i = 0; i < vertsAdd.Length; i++)
            {
                vertsAddNew[i] = vertsAdd[i] - new Vector3(0, 1f, 0);
            }

            GetVertsAddNewForUpDown(chunk, DirectionEnum.Left, localPosition, vertsAddNew, vertsAdd, 0, 1);
            GetVertsAddNewForUpDown(chunk, DirectionEnum.Right, localPosition, vertsAddNew, vertsAdd, 2, 3);
            GetVertsAddNewForUpDown(chunk, DirectionEnum.Forward, localPosition, vertsAddNew, vertsAdd, 0, 3);
            GetVertsAddNewForUpDown(chunk, DirectionEnum.Back, localPosition, vertsAddNew, vertsAdd, 1, 2);
            return vertsAddNew;
        }
        else
        {
            return vertsAdd;
        }
    }

    protected void GetVertsAddNewForUpDown(Chunk chunk, DirectionEnum direction, Vector3Int localPosition, Vector3[] vertsAddNew, Vector3[] vertsAdd, int index1, int index2)
    {
        block.GetCloseBlockByDirection(chunk, localPosition, direction, out Block closeBlock, out Chunk closeChunk, out Vector3Int closeLocalPosition);
        if (closeBlock != null && closeBlock.blockType == block.blockType)
        {
            Vector3Int tempPosition = block.GetClosePositionByDirection(direction, localPosition);
            Vector3Int localPositionClose = tempPosition + chunk.chunkData.positionForWorld - closeChunk.chunkData.positionForWorld;
            BlockBean closeBlockData = closeChunk.GetBlockData(localPositionClose);
            if (closeBlockData == null)
            {
                vertsAddNew[index1] = vertsAdd[index1];
                vertsAddNew[index2] = vertsAdd[index2];
            }
            else
            {
                BlockLiquidBean closeLiquid = Block.FromMetaData<BlockLiquidBean>(closeBlockData.meta);
                if (closeLiquid == null || closeLiquid.level == 0)
                {
                    vertsAddNew[index1] = vertsAdd[index1];
                    vertsAddNew[index2] = vertsAdd[index2];
                }
            }
        }
    }

    protected void GetVertsAddNewForRange(Chunk chunk, DirectionEnum direction, Vector3Int localPosition, Vector3[] vertsAddNew, Vector3[] vertsAdd, int indexPosition)
    {
        block.GetCloseBlockByDirection(chunk, localPosition, direction, out Block closeBlock, out Chunk closeChunk, out Vector3Int closeLocalPosition);

        if (closeBlock != null && closeBlock.blockType == block.blockType)
        {
            Vector3Int tempPosition = block.GetClosePositionByDirection(direction, localPosition);
            Vector3Int localPositionClose = tempPosition + chunk.chunkData.positionForWorld - closeChunk.chunkData.positionForWorld;
            BlockBean closeBlockData = closeChunk.GetBlockData(localPositionClose);
            if (closeBlockData == null)
            {
                vertsAddNew[indexPosition] = vertsAdd[indexPosition];
            }
            else
            {
                BlockLiquidBean closeLiquid = Block.FromMetaData<BlockLiquidBean>(closeBlockData.meta);
                if (closeLiquid == null || closeLiquid.level == 0)
                {
                    vertsAddNew[indexPosition] = vertsAdd[indexPosition];
                }
            }
        }
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk != null && closeBlockChunk.isInit)
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