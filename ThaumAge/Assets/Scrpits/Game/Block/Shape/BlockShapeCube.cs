using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class BlockShapeCube : BlockShape
{
    public static Vector3[] vertsAddLeft = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(0,0,1)
        };
    public static Vector3[] vertsAddRight = new Vector3[]
        {
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
        };
    public static Vector3[] vertsAddDown = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0)
        };
    public static Vector3[] vertsAddUp = new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0)
        };
    public static Vector3[] vertsAddForward = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
            new Vector3(1,0,0)
        };
    public static Vector3[] vertsAddBack = new Vector3[]
        {
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
        };

    //正方形的方块 所用数据
    public Vector2[] uvsAddLeft;
    public Vector2[] uvsAddRight;
    public Vector2[] uvsAddDown;
    public Vector2[] uvsAddUp;
    public Vector2[] uvsAddForward;
    public Vector2[] uvsAddBack;

    protected int[] trisAddReversed;
    public BlockShapeCube() : base()
    {
        trisAdd = new int[]
        {
            0,1,2, 0,2,3
        };

        trisAddReversed = new int[]
        {
            0,2,1, 0,3,2
        };
    }

    public override void InitData(Block block)
    {
        base.InitData(block);
        Vector2 uvStart = GetUVStartPosition(block, DirectionEnum.Left);

        uvsAddLeft = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Right);
        uvsAddRight = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Down);
        uvsAddDown = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.UP);
        uvsAddUp = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Forward);
        uvsAddForward = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Back);
        uvsAddBack = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
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
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.BuildBlock(chunk, localPosition, direction);

        if (block.blockType != BlockTypeEnum.None)
        {
            int startVertsIndex = chunk.chunkMeshData.verts.Count;
            int startTrisIndex = chunk.chunkMeshData.dicTris[block.blockInfo.material_type].Count;

            int startVertsColliderIndex = 0;
            int startTrisColliderIndex = 0;

            if (block.blockInfo.collider_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisCollider.Count;
            }
            else if (block.blockInfo.trigger_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsTrigger.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisTrigger.Count;
            }
            int buildFaceCount = 0;

            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, false);
                buildFaceCount++;
            }

            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, true);
                buildFaceCount++;
            }

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, false);
                buildFaceCount++;
            }

            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, true);
                buildFaceCount++;
            }

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, true);
                buildFaceCount++;
            }

            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, false);
                buildFaceCount++;
            }

            int vertsCount = buildFaceCount * 4;
            int trisIndex = buildFaceCount * 6;

            if (vertsCount != 0)
            {
                chunk.chunkMeshData.AddMeshIndexData(localPosition,
                    startVertsIndex, vertsCount, startTrisIndex, trisIndex,
                    startVertsColliderIndex, vertsCount, startTrisColliderIndex, trisIndex);
            }
        }
    }

    public override void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.BuildBlockNoCheck(chunk, localPosition, direction);
        if (block.blockType != BlockTypeEnum.None)
        {
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, false);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, false);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, false);
        }
    }


    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="buildDirection"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="chunkData"></param>
    public virtual void BuildFace(Block block, Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAddFace, Vector2[] uvsAdd, bool reversed)
    {
        BaseAddTris(chunk, localPosition, direction, face, reversed);
        BaseAddVerts(chunk, localPosition, direction, face, vertsAddFace);
        BaseAddUVs(chunk, localPosition, direction, face, uvsAdd);
    }

    public virtual void BaseAddVerts(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd)
    {
        AddVerts(localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsAdd);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsAdd);
    }

    public virtual void BaseAddUVs(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector2[] uvsAdd)
    {
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }


    public virtual void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, bool reversed)
    {
        base.BaseAddTris(chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisNormal = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
        if (reversed)
        {
            AddTris(index, trisNormal, trisAdd);
            if (block.blockInfo.collider_state == 1)
            {
                int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                AddTris(colliderIndex, trisCollider, trisAdd);
            }
            if (block.blockInfo.trigger_state == 1)
            {
                int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
                AddTris(triggerIndex, trisTrigger, trisAdd);
            }
        }
        else
        {
            AddTris(index, trisNormal, trisAddReversed);
            if (block.blockInfo.collider_state == 1)
            {
                int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                AddTris(colliderIndex, trisCollider, trisAddReversed);
            }
            if (block.blockInfo.trigger_state == 1)
            {
                int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
                AddTris(triggerIndex, trisTrigger, trisAddReversed);
            }
        }
    }

    /// <summary>
    /// 获取起始UV
    /// </summary>
    /// <param name="buildDirection"></param>
    /// <returns></returns>
    public virtual Vector2 GetUVStartPosition(Block block, DirectionEnum buildDirection)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (arrayUVData.Length == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        else if (arrayUVData.Length == 3)
        {
            //3种面  上 中 下
            switch (buildDirection)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[2].y, uvWidth * arrayUVData[2].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[1].y, uvWidth * arrayUVData[1].x);
                    break;
            }
        }
        else if (arrayUVData.Length == 6)
        {
            //6种面  上 中 下
            switch (buildDirection)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[1].y, uvWidth * arrayUVData[1].x);
                    break;
                case DirectionEnum.Left:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[2].y, uvWidth * arrayUVData[2].x);
                    break;
                case DirectionEnum.Right:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[3].y, uvWidth * arrayUVData[3].x);
                    break;
                case DirectionEnum.Forward:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[4].y, uvWidth * arrayUVData[4].x);
                    break;
                case DirectionEnum.Back:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[5].y, uvWidth * arrayUVData[5].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
            }
        }
        else
        {
            uvStartPosition = Vector2.zero;
        }
        return uvStartPosition;
    }
}