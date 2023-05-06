using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLiquid : BlockShapeCube
{

    public static float itemVolumeHeight = 1f / BlockBaseLiquid.maxLiquidVolume;
    public static float liquidDefHeightOffset = 1 / 16f;

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

    public virtual void BuildFaceForLiquid(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        bool isCloseLiquid, BlockMetaLiquid blockMetaLiquid, BlockMetaLiquid closeBlockMetaLiquid)
    {
        //构建液体面
        Vector3[] vertsAddNew;
        //如果旁边不是液体
        if (!isCloseLiquid)
        {
            //如果是自己满水
            if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
            {
                vertsAddNew = GetVertsForYMove(vertsAdd, 0, -liquidDefHeightOffset);
            }
            //如果不是满水
            else
            {
                vertsAddNew = GetVertsForYMove(vertsAdd, 0, -(BlockBaseLiquid.maxLiquidVolume - blockMetaLiquid.volume) * itemVolumeHeight);
            }
        }
        //如果旁边是液体
        else
        {
            //如果是自己满水
            if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
            {
                //如果旁边的水也是满水
                if (closeBlockMetaLiquid == null|| closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                {
                    //只需要往上移动旁边水的容积
                    vertsAddNew = GetVertsForYMove(vertsAdd, 1 - liquidDefHeightOffset, 0);
                }
                else
                {
                    //只需要往上移动旁边水的容积
                    vertsAddNew = GetVertsForYMove(vertsAdd, closeBlockMetaLiquid.volume * itemVolumeHeight, -liquidDefHeightOffset);
                }
            }
            else
            {
                //需要往上移动旁边水的容积 再往下移动自己少了的容积
                vertsAddNew = GetVertsForYMove(vertsAdd, closeBlockMetaLiquid.volume * itemVolumeHeight, -(BlockBaseLiquid.maxLiquidVolume - blockMetaLiquid.volume) * itemVolumeHeight);
            }
        }
        BaseAddTrisForLiquid(chunk, localPosition, direction, face, trisAdd);
        BaseAddVertsUVsColorsForLiquid(chunk, localPosition, direction, face, vertsAddNew, uvsAdd, colorsAdd);
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


    /// <summary>
    /// 检测是否是满水 并且有下降1/16
    /// </summary>
    /// <returns></returns>
    //public virtual bool CheckIsFullOffset()
    //{

    //}

    public virtual bool CheckNeedBuildFaceAndBuild(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd,
        BlockMetaLiquid blockMetaLiquid)
    {
        if (localPosition.y == 0)
        {
            return false;
        }
        //获取旁边方块的数据
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlockChunk != null && closeBlockChunk.isInit)
        {
            //如果旁边是空气方块 生成面
            if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
            {
                BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
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
                    //水是满的 则不生成面 水面紧贴方块的面
                    BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
                    return true;
                }
                //其他面 则不生成面
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
                    //获取旁边液体方块数据
                    BlockBean closeBlockData = closeBlockChunk.GetBlockData(closeLocalPosition);
                    BlockMetaLiquid closeBlockMetaLiquid = null;
                    if (closeBlockData != null)
                    {
                        closeBlockMetaLiquid = closeBlockData.GetBlockMeta<BlockMetaLiquid>();
                    }
                    //------------上---------------
                    if (closeDirection == DirectionEnum.UP)
                    {
                        //如果自己已经满了 则不渲染面
                        if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            return false;
                        }
                        else
                        {
                            BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
                            return true;
                        }
                    }
                    //------------下---------------
                    else if (closeDirection == DirectionEnum.Down)
                    {
                        //如果下方方块的水已经满了 则不渲染面
                        if (closeBlockMetaLiquid == null || closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            return false;
                        }
                        else
                        {
                            BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
                            return true;
                        }
                    }            
                    //------------侧---------------
                    else
                    {
                        //如果旁边的水满了
                        if (closeBlockMetaLiquid == null || closeBlockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                        {
                            //如果自己是满水
                            if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                            {
                                closeBlockChunk.GetBlockForLocal(closeLocalPosition + Vector3Int.up, out Block closeUpBlock, out BlockDirectionEnum closeUpDirection, out Chunk closeUpBlockChunk);
                                //如果旁边的上方是相关的水方块 
                                if (closeUpBlock != null
                                    && (closeUpBlock.blockType == closeBlock.blockType
                                        || (closeUpBlock is BlockBaseLiquid closeUpBlockLiquid && closeUpBlockLiquid.CheckIsSameType(closeBlockChunk, closeBlock))
                                        || (closeUpBlock is BlockBaseLiquidSame closeUpBlockLiquidSame && closeUpBlockLiquidSame.CheckIsSameType(closeBlockChunk, closeBlock))))
                                {
                                    return false;
                                }
                                //其他情况有偏移1/16
                                else
                                {
                                    //获取上方方块
                                    chunk.GetBlockForLocal(localPosition + Vector3Int.up, out Block upBlock, out BlockDirectionEnum upDirection, out Chunk upBlockChunk);
                                    if (upBlock != null
                                        && (upBlock.blockType == closeBlock.blockType
                                            || (upBlock is BlockBaseLiquid upBlockLiquid && upBlockLiquid.CheckIsSameType(chunk, block))
                                            || (upBlock is BlockBaseLiquidSame upBlockLiquidSame && upBlockLiquidSame.CheckIsSameType(chunk, block))))
                                    {
                                        BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, true, blockMetaLiquid, closeBlockMetaLiquid);
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        //如果旁边的水没满
                        else
                        {
                            //如果自身是满的
                            if (blockMetaLiquid == null || blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
                            {
                                BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, true, blockMetaLiquid, closeBlockMetaLiquid);
                                return true;
                            }
                            //如果自身不满
                            else
                            {
                                //如果旁边的水容量大于等于自己的水容量 则不渲染面
                                if (closeBlockMetaLiquid.volume >= blockMetaLiquid.volume)
                                {
                                    return false;
                                }
                                //其他情况需要渲染
                                else
                                {
                                    BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, true, blockMetaLiquid, closeBlockMetaLiquid);
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //TODO 暂时按不同类型的方块生成面
                    BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
                    return true;
                }
            //其他类型的方块则统一生成面 
            default:
                BuildFaceForLiquid(chunk, localPosition, direction, closeDirection, vertsAdd, uvsAdd, colorsAdd, trisAdd, false, blockMetaLiquid, null);
                return true;
        }
    }

    /// <summary>
    /// 获取水面Y轴位移
    /// </summary>
    public Vector3[] GetVertsForYMove(Vector3[] oldData, float moveOffsetBottom, float moveOffsetTop)
    {
        Vector3[] newData = new Vector3[oldData.Length];
        for (int i = 0; i < oldData.Length; i++)
        {
            Vector3 itemOldData = oldData[i];
            if (itemOldData.y == 1 && moveOffsetTop != 0)
            {
                newData[i] = itemOldData.AddY(moveOffsetTop);
            }
            else if (itemOldData.y == 0 && moveOffsetBottom != 0)
            {
                newData[i] = itemOldData.AddY(moveOffsetBottom);
            }
            else
            {
                newData[i] = itemOldData;
            }
        }
        return newData;
    }
}