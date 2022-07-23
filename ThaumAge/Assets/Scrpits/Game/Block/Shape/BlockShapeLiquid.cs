using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLiquid : BlockShapeCube
{

    public static float itemVolumeHeight = 1f / BlockBaseLiquid.maxLiquidVolume;

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        if (block.blockType != BlockTypeEnum.None)
        {
            //只有在能旋转的时候才去查询旋转方向
            BlockDirectionEnum direction = BlockDirectionEnum.UpForward;
            BlockBean blockData = chunk.GetBlockData(localPosition);
            //获取水的数据
            BlockMetaLiquid blockMetaLiquid = null;
            if (blockData != null)
            {
                blockMetaLiquid = blockData.GetBlockMeta<BlockMetaLiquid>();
            }

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, colorsAddCube, trisAddCube, blockMetaLiquid);

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, colorsAddCube, trisAddCube, blockMetaLiquid);

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, colorsAddCube, trisAddCube, blockMetaLiquid);

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, colorsAddCube, trisAddCube, blockMetaLiquid);

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, colorsAddCube, trisAddCube, blockMetaLiquid);

            CheckNeedBuildFaceAndBuild(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, colorsAddCube, trisAddCube, blockMetaLiquid);
        }
    }

    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid)
    {
        //如果是自己满水
        if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
        {
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
        }
        else
        {
            Vector3[] vertsAddNew = GetVertsForYMove(vertsAdd, 0, (BlockBaseLiquid.maxLiquidVolume - blockMetaLiquid.volume) * itemVolumeHeight);
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
        }
    }

    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid, BlockMetaLiquid closeBlockMetaLiquid)
    {
        //如果是自己满水
        if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
        {
            //只需要往上移动旁边水的容积
            Vector3[] vertsAddNew = GetVertsForYMove(vertsAdd, closeBlockMetaLiquid.volume * itemVolumeHeight, 0);
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
        }
        else
        {
            //需要往上移动旁边水的容积 再往下移动自己少了的容积
            Vector3[] vertsAddNew = GetVertsForYMove(vertsAdd, closeBlockMetaLiquid.volume * itemVolumeHeight, (BlockBaseLiquid.maxLiquidVolume - blockMetaLiquid.volume) * itemVolumeHeight);
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
        }
    }

    public virtual void BaseAddTrisForLiquid(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, int[] trisAdd)
    {
        int index = chunk.chunkMeshData.verts.Count;
        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type2];
        AddTris(index, trisData, trisAdd);
    }
    public virtual void BaseAddVertsUVsColorsForLiquid(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        AddVertsUVsColors(localPosition, direction,
            chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
            vertsAdd, uvsAdd, colorsAdd);
    }


    public virtual bool CheckNeedBuildFaceAndBuild(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection,
            out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlockChunk != null && closeBlockChunk.isInit)
        {
            if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
            {
                //只是空气方块
                BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                return true;
            }
        }
        else
        {
            //还没有生成chunk
            return false;
        }
        BlockShapeEnum closeBlockShape = closeBlock.blockInfo.GetBlockShape();
        switch (closeBlockShape)
        {
            case BlockShapeEnum.Cube:
                if (closeDirection == DirectionEnum.UP)
                {
                    //水是满的
                    if (blockMetaLiquid == null || blockMetaLiquid.volume == 8)
                    {
                        return false;
                    }
                    BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                    return true;
                }
                return false;
            case BlockShapeEnum.Liquid:
            case BlockShapeEnum.LiquidCross:
            case BlockShapeEnum.LiquidCrossOblique:
                if (block.blockType == closeBlock.blockType 
                    || (block is BlockBaseLiquid blockLiquid && blockLiquid.CheckIsSameType(closeBlockChunk, closeBlock))
                    || (block is BlockBaseLiquidSame blockLiquidSame && blockLiquidSame.CheckIsSameType(closeBlockChunk, closeBlock))
                    )
                {
                    BlockBean closeBlockData = closeBlockChunk.GetBlockData(closeLocalPosition);
                    BlockMetaLiquid closeBlockMetaLiquid = null;
                    if (closeBlockData != null)
                    {
                        closeBlockMetaLiquid = closeBlockData.GetBlockMeta<BlockMetaLiquid>();
                    }

                    if (closeDirection == DirectionEnum.UP)
                    {
                        //如果自己已经满了 则不渲染面
                        if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            return false;
                        }
                        else
                        {
                            BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                            return true;
                        }
                    }
                    else if (closeDirection == DirectionEnum.Down)
                    {
                        //如果下方方块的水已经满了 则不渲染面
                        if (closeBlockMetaLiquid == null || closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            return false;
                        }
                        else
                        {
                            BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                            return true;
                        }
                    }
                    else
                    {
                        //如果旁边的水满了 则不渲染面
                        if (closeBlockMetaLiquid == null || closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            return false;
                        }
                        else
                        {
                            if (blockMetaLiquid == null)
                            {
                                //如果旁边的水容量大于等于自己的水容量 则不渲染面
                                if (closeBlockMetaLiquid.volume >= BlockBaseLiquid.maxLiquidVolume)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                //如果旁边的水容量大于等于自己的水容量 则不渲染面
                                if (closeBlockMetaLiquid.volume >= blockMetaLiquid.volume)
                                {
                                    return false;
                                }
                            }
                            BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid, closeBlockMetaLiquid);
                            return true;
                        }
                    }
                }
                else
                {
                    BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                    return true;
                }
            default:
                BuildFace(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                return true;
        }
    }


    public Vector3[] GetVertsForYMove(Vector3[] oldData, float moveOffsetAdd, float moveOffsetSub)
    {
        Vector3[] newData = new Vector3[oldData.Length];
        for (int i = 0; i < oldData.Length; i++)
        {
            Vector3 itemOldData = oldData[i];
            if (itemOldData.y == 1 && moveOffsetSub > 0)
            {
                newData[i] = itemOldData.AddY(-moveOffsetSub);
            }
            else if (itemOldData.y == 0 && moveOffsetAdd > 0)
            {
                newData[i] = itemOldData.AddY(moveOffsetAdd);
            }
            else
            {
                newData[i] = itemOldData;
            }
        }
        return newData;
    }
}