using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLiquid : BlockShapeCube
{

    public static float itemVolumeHeight = 1f / BlockBaseLiquid.maxLiquidVolume;

    public BlockShapeLiquid(Block block) : base(block)
    {
        uvsAddLeft = new Vector2[]
        {
            new Vector2(0 ,1),
            new Vector2(1,1),
            new Vector2(1 ,0),
            new Vector2(0 ,0)
        };

        uvsAddRight = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        uvsAddDown = new Vector2[]
        {
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,0)
        };

        uvsAddUp = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        uvsAddForward = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        uvsAddBack = new Vector2[]
        {
            new Vector2(0 ,1),
            new Vector2(1,1),
            new Vector2(1 ,0),
            new Vector2(0 ,0)
        };
    }

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
    /// <summary>
    /// 构建面
    /// </summary>
    public virtual void BuildFaceNoCloseLiquid(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid)
    {
        //如果是自己满水
        if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
        {
            Vector3[] vertsAddNew = GetVertsForYMove(vertsAdd, 0, 1 / 16f);
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
        }
        else
        {
            Vector3[] vertsAddNew = GetVertsForYMove(vertsAdd, 0, (BlockBaseLiquid.maxLiquidVolume - blockMetaLiquid.volume) * itemVolumeHeight);
            BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
            BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
        }
    }

    /// <summary>
    /// 构建面
    /// </summary>
    public virtual void BuildFaceHasCloseLiquid(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid , BlockMetaLiquid closeBlockMetaLiquid)
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
        //获取旁边方块的数据
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlockChunk != null && closeBlockChunk.isInit)
        {
            //如果旁边是空气方块
            if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
            {
                BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                return true;
            }
        }
        else
        {
            //还没有生成chunk 则不生成面
            return false;
        }
        BlockShapeEnum closeBlockShape = closeBlock.blockInfo.GetBlockShape();
        switch (closeBlockShape)
        {
            //如果旁边的方块或者是耕地或者正方形方块
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.Plough:
                //如果是上面的面
                if (closeDirection == DirectionEnum.UP)
                {
                    //水是满的
                    if (blockMetaLiquid == null || blockMetaLiquid.volume == 8)
                    {
                        return false;
                    }
                    //如果水没满 则需要构建上方的水面
                    else
                    {
                        BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                        return true;
                    }
                }
                //其他面
                else
                {
                    return false;
                }
            //如果旁边的方块是与水相关的方块
            case BlockShapeEnum.Liquid:
            case BlockShapeEnum.LiquidCross:
            case BlockShapeEnum.LiquidCrossOblique:
                //检测是否是同种类型的液体方块
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
                            BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
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
                            BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                            return true;
                        }
                    }
                    else
                    {
                        //如果旁边的水满了 则不渲染面
                        if (closeBlockMetaLiquid == null || closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            //如果旁边的上方是空的
                            closeBlock.GetCloseBlockByDirection(closeBlockChunk, closeLocalPosition, DirectionEnum.UP, out Block closeUpBlock, out Chunk closeUpChunk, out Vector3Int closeUpLocalPosition);
                            if (closeUpChunk != null)
                            {
                                if (closeUpBlock == null || closeUpBlock.blockType == BlockTypeEnum.None)
                                {
                                    BuildFaceHasCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid, closeBlockMetaLiquid);
                                    return true;
                                }
                            }
                            return false;
                        }
                        //如果旁边的水没满
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
                            BuildFaceHasCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid, closeBlockMetaLiquid);
                            return true;
                        }
                    }
                }
                else
                {
                    BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                    return true;
                }
            default:
                BuildFaceNoCloseLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, blockMetaLiquid);
                return true;
        }
    }

    /// <summary>
    /// 获取水面Y轴位移
    /// </summary>
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