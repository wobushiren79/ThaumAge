using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class BlockShapeCube : BlockShape
{
    protected Vector3[] vertsAddLeft;
    protected Vector3[] vertsAddRight;
    protected Vector3[] vertsAddDown;
    protected Vector3[] vertsAddUp;
    protected Vector3[] vertsAddForward;
    protected Vector3[] vertsAddBack;

    protected int[] trisAddReversed;
    public BlockShapeCube() : base()
    {
        vertsAddLeft = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(0,0,1)
        };
        vertsAddRight = new Vector3[]
        {
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
        };
        vertsAddDown = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0)
        };
        vertsAddUp = new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0)
        };
        vertsAddForward = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
            new Vector3(1,0,0)
        };
        vertsAddBack = new Vector3[]
        {
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
        };

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

        block.uvsAddLeft = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Right);
        block.uvsAddRight = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Down);
        block.uvsAddDown = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.UP);
        block.uvsAddUp = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Forward);
        block.uvsAddForward = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Back);
        block.uvsAddBack = new Vector2[]
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
    public override void BuildBlock(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BuildBlock(block, chunk, localPosition, direction);

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
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Left))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, block.uvsAddLeft, false);
                buildFaceCount++;
            }

            //Right
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Right))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, block.uvsAddRight, true);
                buildFaceCount++;
            }

            //Bottom
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Down))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, block.uvsAddDown, false);
                buildFaceCount++;
            }

            //Top
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.UP))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, block.uvsAddUp, true);
                buildFaceCount++;
            }

            //Forward
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, block.uvsAddForward, true);
                buildFaceCount++;
            }

            //Back
            if (CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Back))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, block.uvsAddBack, false);
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

    public override void BuildBlockNoCheck(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BuildBlockNoCheck(block, chunk, localPosition, direction);
        if (block.blockType != BlockTypeEnum.None)
        {
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, block.uvsAddLeft, false);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, block.uvsAddRight, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, block.uvsAddDown, false);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, block.uvsAddUp, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, block.uvsAddForward, true);
            BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, block.uvsAddBack, false);
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
    public virtual void BuildFace(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum face, Vector3[] vertsAddFace, Vector2[] uvsAdd, bool reversed)
    {
        BaseAddTris(block, chunk, localPosition, direction, reversed);
        BaseAddVerts(block, chunk, localPosition, direction, vertsAddFace);
        BaseAddUVs(block, chunk, localPosition, direction, face, uvsAdd);
    }

    public override void BaseAddVerts(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(block, chunk, localPosition, direction, vertsAdd);

        AddVerts(block, localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(block, localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsAdd);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(block, localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsAdd);
    }

    public virtual void BaseAddUVs(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum face, Vector2[] uvsAdd)
    {
        base.BaseAddUVs(block, chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }


    public virtual void BaseAddTris(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, bool reversed)
    {
        base.BaseAddTris(block, chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;
        int indexCollider = 0;

        if (block.blockInfo.collider_state == 1)
            indexCollider = chunk.chunkMeshData.vertsCollider.Count;
        if (block.blockInfo.trigger_state == 1)
            indexCollider = chunk.chunkMeshData.trisCollider.Count;

        List<int> trisNormal = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
        if (reversed)
        {
            AddTris(index, trisNormal, trisAdd);
            if (block.blockInfo.collider_state == 1)
                AddTris(indexCollider, trisCollider, trisAdd);
            if (block.blockInfo.trigger_state == 1)
                AddTris(indexCollider, trisTrigger, trisAdd);
        }
        else
        {
            AddTris(index, trisNormal, trisAddReversed);
            if (block.blockInfo.collider_state == 1)
                AddTris(indexCollider, trisCollider, trisAddReversed);
            if (block.blockInfo.trigger_state == 1)
                AddTris(indexCollider, trisTrigger, trisAddReversed);
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
        else
        {
            uvStartPosition = Vector2.zero;
        }
        return uvStartPosition;
    }
}