using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class BlockCube : Block
{
    public BlockCube() : base()
    {

    }

    public BlockCube(BlockTypeEnum blockType) : base(blockType)
    {

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
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Right, new Vector3Int(localPosition.x + 1, localPosition.y, localPosition.z), Vector3.up, Vector3.forward, true);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.UP, new Vector3Int(localPosition.x, localPosition.y + 1, localPosition.z), Vector3.forward, Vector3.right, true);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Back, new Vector3Int(localPosition.x, localPosition.y, localPosition.z + 1), Vector3.up, Vector3.right, false);
        }
    }

    public override void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Right,  localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true);
            BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false);
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
    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, DirectionEnum buildDirection, Vector3 corner, Vector3 up, Vector3 right, bool reversed)
    {
        AddTris(chunk, localPosition, direction, chunkMeshData,reversed);
        AddVerts(chunk, localPosition, direction, chunkMeshData, corner, up, right);
        AddUVs(chunk, localPosition, direction, chunkMeshData, buildDirection);
    }

    public virtual void AddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3 corner, Vector3 up, Vector3 right)
    {
        base.AddVerts(chunk, localPosition, direction, chunkMeshData, corner);

        List<Vector3> verts = chunkMeshData.verts;
        AddVert(localPosition, direction, verts, corner);
        AddVert(localPosition, direction, verts, corner + up);
        AddVert(localPosition, direction, verts, corner + up + right);
        AddVert(localPosition, direction, verts, corner + right);

        List<Vector3> vertsCollider = chunkMeshData.vertsCollider;
        AddVert(localPosition, direction, vertsCollider, corner);
        AddVert(localPosition, direction, vertsCollider, corner + up);
        AddVert(localPosition, direction, vertsCollider, corner + up + right);
        AddVert(localPosition, direction, vertsCollider, corner + right);
    }

    public virtual void AddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, DirectionEnum buildDirection)
    {
        base.AddUVs(chunk, localPosition, direction, chunkMeshData);

        Vector2 uvStartPosition = GetUVStartPosition(buildDirection);

        List<Vector2> uvs = chunkMeshData.uvs;
        uvs.Add(uvStartPosition);
        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));
    }


    public virtual void AddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, bool reversed)
    {
        base.AddTris(chunk, localPosition, direction, chunkMeshData);

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