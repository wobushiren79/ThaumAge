using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockShapeCubeHalf : BlockShapeCube
{
    protected static float offsetCubeHalf = 0.5f;
    protected static Vector3[] HandleHalfVerts(Vector3[] verts, Vector3 offsetPosition)
    {
        Vector3[] newVerts = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 itemVert = verts[i];
            float newX = offsetPosition.x == 0
                ? itemVert.x : (offsetPosition.x > 0
                ? (itemVert.x < 0.5f ? itemVert.x + offsetPosition.x : itemVert.x) : (itemVert.x > 0.5f ? itemVert.x + offsetPosition.x : itemVert.x));
            float newY = offsetPosition.y == 0
                ? itemVert.y : (offsetPosition.y > 0
                ? (itemVert.y < 0.5f ? itemVert.y + offsetPosition.y : itemVert.y) : (itemVert.y > 0.5f ? itemVert.y + offsetPosition.y : itemVert.y));
            float newZ = offsetPosition.z == 0
                ? itemVert.z : (offsetPosition.z > 0
                ? (itemVert.z < 0.5f ? itemVert.z + offsetPosition.z : itemVert.z) : (itemVert.z > 0.5f ? itemVert.z + offsetPosition.z : itemVert.z));
            newVerts[i] = new Vector3(newX, newY, newZ);
        }
        return newVerts;
    }

    protected static Vector2[] HandleHalfUvs(Vector2 uvStart, Vector2[] verts, Vector2 offsetPosition)
    {
        Vector2[] newVerts = new Vector2[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector2 itemVert = verts[i];
            float newX = offsetPosition.x == 0
                ? itemVert.x : (offsetPosition.x > 0
                ? (itemVert.x < uvStart.x + (uvWidth / 2f) ? itemVert.x + offsetPosition.x : itemVert.x) : (itemVert.x > uvStart.x + (uvWidth / 2f) ? itemVert.x + offsetPosition.x : itemVert.x));
            float newY = offsetPosition.y == 0
                ? itemVert.y : (offsetPosition.y > 0
                ? (itemVert.y < uvStart.y + (uvWidth / 2f) ? itemVert.y + offsetPosition.y : itemVert.y) : (itemVert.y > uvStart.y + (uvWidth / 2f) ? itemVert.y + offsetPosition.y : itemVert.y));
            newVerts[i] = new Vector2(newX, newY);
        }
        return newVerts;
    }

    public static Vector3[][] vertsAddHalfUp = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,offsetCubeHalf,0))
    };

    public static Vector3[][] vertsAddHalfDown = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,-offsetCubeHalf,0))
    };

    public static Vector3[][] vertsAddHalfLeft = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(-offsetCubeHalf,0,0))
    };

    public static Vector3[][] vertsAddHalfRight = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(offsetCubeHalf,0,0))
    };

    public static Vector3[][] vertsAddHalfForward = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,0,-offsetCubeHalf))
    };

    public static Vector3[][] vertsAddHalfBack = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,0,offsetCubeHalf))
    };

    public Vector2[][] uvsAddHalfUp;
    public Vector2[][] uvsAddHalfDown;
    public Vector2[][] uvsAddHalfLeft;
    public Vector2[][] uvsAddHalfRight;
    public Vector2[][] uvsAddHalfForward;
    public Vector2[][] uvsAddHalfBack;

    public override void InitData(Block block)
    {
        base.InitData(block);
        Vector2 uvStart = GetUVStartPosition(block, DirectionEnum.Left);
        float halfUV = uvWidth / 2f;
        uvsAddHalfUp = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(0,halfUV)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(0,halfUV)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(0,halfUV)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(0,halfUV))
        };
        uvsAddHalfDown = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(0,-halfUV)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(0,-halfUV)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(0,-halfUV)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(0,-halfUV))
        };
        uvsAddHalfLeft = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(-halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(-halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(-halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(-halfUV,0))
        };
        uvsAddHalfRight = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(halfUV,0))
        };
        uvsAddHalfForward = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(-halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(-halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(0,-halfUV)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(0,-halfUV)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(0,0))
        };
        uvsAddHalfBack = new Vector2[][]
        {
            HandleHalfUvs(uvStart,uvsAddLeft, new Vector2(halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddRight, new Vector2(halfUV,0)),
            HandleHalfUvs(uvStart,uvsAddUp, new Vector2(0,halfUV)),
            HandleHalfUvs(uvStart,uvsAddDown, new Vector2(0,halfUV)),
            HandleHalfUvs(uvStart,uvsAddForward, new Vector2(0,0)),
            HandleHalfUvs(uvStart,uvsAddBack, new Vector2(0,0))
        };
    }

    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        if (block.blockType != BlockTypeEnum.None)
        {
            //只有在能旋转的时候才去查询旋转方向
            BlockDirectionEnum direction = BlockDirectionEnum.UpForward;
            if (block.blockInfo.rotate_state != 0)
            {
                direction = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
            }
            BlockBean blockData = chunk.GetBlockData(localPosition);
            if (blockData == null)
            {
                blockData = new BlockBean(localPosition,block.blockType);
            }
            BlockMetaCubeHalf blockMeta = blockData.GetBlockMeta<BlockMetaCubeHalf>();
            if (blockMeta == null)
            {
                blockMeta = new BlockMetaCubeHalf();
                blockMeta.SetHalfPosition(DirectionEnum.Down);
            }

            DirectionEnum halfPosition = blockMeta.GetHalfPosition();

            Vector3[][] arrayVertsData = null;
            Vector2[][] arrayUvsData = null;
            switch (halfPosition)
            {
                case DirectionEnum.UP:
                    arrayVertsData = vertsAddHalfUp;
                    arrayUvsData = uvsAddHalfUp;
                    break;
                case DirectionEnum.Down:
                    arrayVertsData = vertsAddHalfDown;
                    arrayUvsData = uvsAddHalfDown;
                    break;
                case DirectionEnum.Left:
                    arrayVertsData = vertsAddHalfLeft;
                    arrayUvsData = uvsAddHalfLeft;
                    break;
                case DirectionEnum.Right:
                    arrayVertsData = vertsAddHalfRight;
                    arrayUvsData = uvsAddHalfRight;
                    break;
                case DirectionEnum.Forward:
                    arrayVertsData = vertsAddHalfForward;
                    arrayUvsData = uvsAddHalfForward;
                    break;
                case DirectionEnum.Back:
                    arrayVertsData = vertsAddHalfBack;
                    arrayUvsData = uvsAddHalfBack;
                    break;
            }

            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left, blockMeta))
            {
                Vector3[] useVertsAddLeft = arrayVertsData[0];
                Vector2[] useUvsAddLeft = arrayUvsData[0];
                BuildFace(chunk, localPosition, direction, DirectionEnum.Left, useVertsAddLeft, useUvsAddLeft, colorsAdd, trisAdd);
            }

            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right, blockMeta))
            {
                Vector3[] useVertsAddRight = arrayVertsData[1];
                Vector2[] useUvsAddRight = arrayUvsData[1];
                BuildFace(chunk, localPosition, direction, DirectionEnum.Right, useVertsAddRight, useUvsAddRight, colorsAdd, trisAdd);
            }

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down, blockMeta))
            {
                Vector3[] useVertsAddDown = arrayVertsData[3];
                Vector2[] useUvsAddDown = arrayUvsData[3];
                BuildFace(chunk, localPosition, direction, DirectionEnum.Down, useVertsAddDown, useUvsAddDown, colorsAdd, trisAdd);
            }

            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP, blockMeta))
            {
                Vector3[] useVertsAddUp = arrayVertsData[2];
                Vector2[] useUvsAddUp = arrayUvsData[2];
                BuildFace(chunk, localPosition, direction, DirectionEnum.UP, useVertsAddUp, useUvsAddUp, colorsAdd, trisAdd);
            }

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward, blockMeta))
            {
                Vector3[] useVertsAddForward = arrayVertsData[4];
                Vector2[] useUvsAddForward = arrayUvsData[4];
                BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, useVertsAddForward, useUvsAddForward, colorsAdd, trisAdd);
            }

            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back, blockMeta))
            {
                Vector3[] useVertsAddBack = arrayVertsData[5];
                Vector2[] useUvsAddBack = arrayUvsData[5];
                BuildFace(chunk, localPosition, direction, DirectionEnum.Back, useVertsAddBack, useUvsAddBack, colorsAdd, trisAdd);
            }
        }
    }

    public virtual bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection, BlockMetaCubeHalf blockMeta)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlockChunk != null && closeBlockChunk.isInit)
        {
            if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
            {
                //只是空气方块
                return true;
            }
        }
        else
        {
            //还没有生成chunk
            return false;
        }

        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return CheckNeedBuildFaceForCube(closeDirection, blockMeta);
            case BlockShapeEnum.CubeHalf:
                BlockBean blockData = closeBlockChunk.GetBlockData(closeLocalPosition);
                BlockMetaCubeHalf blockMetaClose;
                if (blockData == null)
                {
                    blockMetaClose = new BlockMetaCubeHalf();
                    blockMetaClose.halfPosition = (int)DirectionEnum.Down;
                }
                else
                {
                    blockMetaClose = blockData.GetBlockMeta<BlockMetaCubeHalf>();
                }
                return CheckNeedBuilFaceForCubeHalf(closeDirection, blockMeta, blockMetaClose);
            default:
                return true;
        }
    }

    protected virtual bool CheckNeedBuildFaceForCube(DirectionEnum faceDirection, BlockMetaCubeHalf blockMeta)
    {
        DirectionEnum halfPosition = blockMeta.GetHalfPosition();
        switch (faceDirection)
        {
            case DirectionEnum.Down:
                if (halfPosition == DirectionEnum.UP)
                    return true;
                else
                    return false;
            case DirectionEnum.UP:
                if (halfPosition == DirectionEnum.Down)
                    return true;
                else
                    return false;
            case DirectionEnum.Left:
                if (halfPosition == DirectionEnum.Right)
                    return true;
                else
                    return false;
            case DirectionEnum.Right:
                if (halfPosition == DirectionEnum.Left)
                    return true;
                else
                    return false;
            case DirectionEnum.Forward:
                if (halfPosition == DirectionEnum.Back)
                    return true;
                else
                    return false;
            case DirectionEnum.Back:
                if (halfPosition == DirectionEnum.Forward)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }


    protected virtual bool CheckNeedBuilFaceForCubeHalf(DirectionEnum faceDirection, BlockMetaCubeHalf blockMeta, BlockMetaCubeHalf blockMetaClose)
    {
        DirectionEnum halfPosition = blockMeta.GetHalfPosition();
        DirectionEnum halfPositionClose = blockMetaClose.GetHalfPosition();
        switch (faceDirection)
        {
            case DirectionEnum.Down:
                if (halfPosition == DirectionEnum.UP)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            return true;
                        case DirectionEnum.UP:
                            return false;
                        case DirectionEnum.Left:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Right:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Forward:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Back:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        default:
                            return false;
                    }
                }
            case DirectionEnum.UP:
                if (halfPosition == DirectionEnum.Down)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            return false;
                        case DirectionEnum.UP:
                            return true;
                        case DirectionEnum.Left:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Right:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Forward:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Back:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        default:
                            return false;
                    }
                }
            case DirectionEnum.Left:
                if (halfPosition == DirectionEnum.Right)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.UP:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Left:
                            return true;
                        case DirectionEnum.Right:
                            return false;
                        case DirectionEnum.Forward:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Back:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        default:
                            return false;
                    }
                }
            case DirectionEnum.Right:
                if (halfPosition == DirectionEnum.Left)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.UP:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Left:
                            return false;
                        case DirectionEnum.Right:
                            return true;
                        case DirectionEnum.Forward:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Back:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        default:
                            return false;
                    }
                }
            case DirectionEnum.Forward:
                if (halfPosition == DirectionEnum.Back)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.UP:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Left:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Right:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Forward:
                            return true;
                        case DirectionEnum.Back:
                            return false;
                        default:
                            return false;
                    }
                }
            case DirectionEnum.Back:
                if (halfPosition == DirectionEnum.Forward)
                    return true;
                else
                {
                    switch (halfPositionClose)
                    {
                        case DirectionEnum.Down:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.UP:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Left:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Right:
                            if (halfPosition == halfPositionClose)
                                return false;
                            else
                                return true;
                        case DirectionEnum.Forward:
                            return false;
                        case DirectionEnum.Back:
                            return true;
                        default:
                            return false;
                    }
                }
            default:
                return false;
        }
    }


    /// <summary>
    /// 获取完整的mesh数据
    /// </summary>
    /// <returns></returns>
    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        Mesh mesh = new Mesh();
        Vector3[][] arrayVertsData = vertsAddHalfDown;
        Vector2[][] arrayUvsData = uvsAddHalfDown;
        mesh.vertices = arrayVertsData[0]
            .Concat(arrayVertsData[1])
            .Concat(arrayVertsData[2])
            .Concat(arrayVertsData[3])
            .Concat(arrayVertsData[4])
            .Concat(arrayVertsData[5])
            .ToArray();
        mesh.triangles = trisAdd
            .Concat(trisAdd.Add(4))
            .Concat(trisAdd.Add(8))
            .Concat(trisAdd.Add(12))
            .Concat(trisAdd.Add(16))
            .Concat(trisAdd.Add(20))
            .ToArray();
        mesh.uv = arrayUvsData[0]
             .Concat(arrayUvsData[1])
             .Concat(arrayUvsData[2])
             .Concat(arrayUvsData[3])
             .Concat(arrayUvsData[4])
             .Concat(arrayUvsData[5])
             .ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}