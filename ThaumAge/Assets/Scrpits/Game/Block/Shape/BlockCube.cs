using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class BlockCube : Block
{
    protected Vector3[] vertsAddLeft;
    protected Vector3[] vertsAddRight;
    protected Vector3[] vertsAddDown;
    protected Vector3[] vertsAddUp;
    protected Vector3[] vertsAddForward;
    protected Vector3[] vertsAddBack;
    public BlockCube()
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
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Left, vertsAddLeft, false);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Right, vertsAddRight, true);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Down, vertsAddDown, false);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.UP, vertsAddUp, true);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Forward, vertsAddForward, true);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Back, vertsAddBack, false);
        }
    }

    public override void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Left, vertsAddLeft, false);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Right, vertsAddRight, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Down, vertsAddDown, false);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.UP, vertsAddUp, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Forward, vertsAddForward, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Back, vertsAddBack, false);
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
    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, DirectionEnum buildDirection, Vector3[] vertsAddFace, bool reversed)
    {
        BaseAddTris(chunk, localPosition, direction, chunkMeshData,reversed);
        BaseAddVerts(chunk, localPosition, direction, chunkMeshData, vertsAddFace);
        BaseAddUVs(chunk, localPosition, direction, chunkMeshData, buildDirection);
    }

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, chunkMeshData, vertsAdd);
        AddVerts(localPosition, direction, chunkMeshData.verts, vertsAdd);
        AddVerts(localPosition, direction, chunkMeshData.vertsCollider, vertsAdd);
    }

    public virtual void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, DirectionEnum buildDirection)
    {
        base.BaseAddUVs(chunk, localPosition, direction, chunkMeshData);

        Vector2 uvStartPosition = GetUVStartPosition(buildDirection);

        List<Vector2> uvs = chunkMeshData.uvs;
        uvs.Add(uvStartPosition);
        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));
    }


    public virtual void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, bool reversed)
    {
        base.BaseAddTris(chunk, localPosition, direction, chunkMeshData);

        int index = chunkMeshData.verts.Count;
        int indexCollider = chunkMeshData.vertsCollider.Count;

        List<int> trisNormal = chunkMeshData.dicTris[(int)BlockMaterialEnum.Normal];
        List<int> trisCollider = chunkMeshData.trisCollider;
        if (reversed)
        {
            trisNormal.Add(index);
            trisNormal.Add(index + 1);
            trisNormal.Add(index + 2);

            trisNormal.Add(index);
            trisNormal.Add(index + 2);
            trisNormal.Add(index + 3);

            trisCollider.Add(indexCollider);
            trisCollider.Add(indexCollider + 1);
            trisCollider.Add(indexCollider + 2);

            trisCollider.Add(indexCollider);
            trisCollider.Add(indexCollider + 2);
            trisCollider.Add(indexCollider + 3);
        }
        else
        {
            trisNormal.Add(index);
            trisNormal.Add(index + 2);
            trisNormal.Add(index + 1);

            trisNormal.Add(index);
            trisNormal.Add(index + 3);
            trisNormal.Add(index + 2);

            trisCollider.Add(indexCollider);
            trisCollider.Add(indexCollider + 2);
            trisCollider.Add(indexCollider + 1);

            trisCollider.Add(indexCollider);
            trisCollider.Add(indexCollider + 3);
            trisCollider.Add(indexCollider + 2);
        }
    }

    /// <summary>
    /// 获取起始UV
    /// </summary>
    /// <param name="buildDirection"></param>
    /// <returns></returns>
    public virtual Vector2 GetUVStartPosition(DirectionEnum buildDirection)
    {
        Vector2Int[] arrayUVData = blockInfo.GetUVPosition();

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