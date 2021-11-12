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
                BuildFace(localPosition, direction, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false, chunkMeshData);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(localPosition, direction, DirectionEnum.Right, new Vector3Int(localPosition.x + 1, localPosition.y, localPosition.z), Vector3.up, Vector3.forward, true, chunkMeshData);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(localPosition, direction, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkMeshData);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(localPosition, direction, DirectionEnum.UP, new Vector3Int(localPosition.x, localPosition.y + 1, localPosition.z), Vector3.forward, Vector3.right, true, chunkMeshData);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(localPosition, direction, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true, chunkMeshData);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(localPosition, direction, DirectionEnum.Back, new Vector3Int(localPosition.x, localPosition.y, localPosition.z + 1), Vector3.up, Vector3.right, false, chunkMeshData);
        }
    }

    public override void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(localPosition, direction, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false, chunkMeshData);
            BuildFace(localPosition, direction, DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, chunkMeshData);
            BuildFace(localPosition, direction, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkMeshData);
            BuildFace(localPosition, direction, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkMeshData);
            BuildFace(localPosition, direction, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true, chunkMeshData);
            BuildFace(localPosition, direction, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, chunkMeshData);
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
    public void BuildFace(Vector3Int localPosition, DirectionEnum direction, DirectionEnum buildDirection, Vector3 corner, Vector3 up, Vector3 right, bool reversed, ChunkMeshData chunkMeshData)
    {
        AddTris(reversed, chunkMeshData);
        AddVerts(localPosition, direction, corner, up, right, chunkMeshData);
        AddUVs(buildDirection, chunkMeshData);
    }

    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, ChunkMeshData chunkMeshData)
    {
        base.AddVerts(localPosition, direction, corner, chunkMeshData);

        ChunkMeshVertsData vertsData = chunkMeshData.vertsData;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner);
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + up);
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + up + right);
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + right);
        vertsData.index++;

        ChunkMeshVertsData vertsColliderData = chunkMeshData.vertsColliderData;
        AddVert(localPosition, direction, vertsColliderData.verts, vertsColliderData.index, corner);
        vertsColliderData.index++;
        AddVert(localPosition, direction, vertsColliderData.verts, vertsColliderData.index, corner + up);
        vertsColliderData.index++;
        AddVert(localPosition, direction, vertsColliderData.verts, vertsColliderData.index, corner + up + right);
        vertsColliderData.index++;
        AddVert(localPosition, direction, vertsColliderData.verts, vertsColliderData.index, corner + right);
        vertsColliderData.index++;
    }

    public void AddUVs(DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.AddUVs(chunkMeshData);

        List<Vector2Int> listData = blockInfo.GetUVPosition();

        Vector2 uvStartPosition;
        if (listData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (listData.Count == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        }
        else if (listData.Count == 3)
        {
            //3种面  上 中 下
            switch (direction)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * listData[2].y, uvWidth * listData[2].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * listData[1].y, uvWidth * listData[1].x);
                    break;
            }
        }
        else
        {
            uvStartPosition = Vector2.zero;
        }
        ChunkMeshUVData uvsData = chunkMeshData.uvsData;
        uvsData.uvs[uvsData.index] = uvStartPosition;
        uvsData.index++;
        uvsData.uvs[uvsData.index] = new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y);
        uvsData.index++;
    }

    public void AddTris(bool reversed, ChunkMeshData chunkMeshData)
    {
        base.AddTris(chunkMeshData);

        int index = chunkMeshData.vertsData.index;
        int indexCollider = chunkMeshData.vertsColliderData.index;

        ChunkMeshTrisData trisData = chunkMeshData.dicTris[(int)BlockMaterialEnum.Normal];
        ChunkMeshTrisData trisColliderData = chunkMeshData.trisColliderData;
        if (reversed)
        {
            trisData.tris[trisData.index] = index;
            trisData.index++;
            trisData.tris[trisData.index] = index+1;
            trisData.index++;
            trisData.tris[trisData.index] = index+2;
            trisData.index++;

            trisData.tris[trisData.index] = index;
            trisData.index++;
            trisData.tris[trisData.index] = index+2;
            trisData.index++;
            trisData.tris[trisData.index] = index+3;
            trisData.index++;

            trisColliderData.tris[trisColliderData.index] = indexCollider;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 1;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 2;
            trisColliderData.index++;

            trisColliderData.tris[trisColliderData.index] = indexCollider;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 2;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 3;
            trisColliderData.index++;
        }
        else
        {
            trisData.tris[trisData.index] = index;
            trisData.index++;
            trisData.tris[trisData.index] = index + 2;
            trisData.index++;
            trisData.tris[trisData.index] = index + 1;
            trisData.index++;

            trisData.tris[trisData.index] = index;
            trisData.index++;
            trisData.tris[trisData.index] = index + 3;
            trisData.index++;
            trisData.tris[trisData.index] = index + 2;
            trisData.index++;

            trisColliderData.tris[trisColliderData.index] = indexCollider;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 2;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 1;
            trisColliderData.index++;

            trisColliderData.tris[trisColliderData.index] = indexCollider;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 3;
            trisColliderData.index++;
            trisColliderData.tris[trisColliderData.index] = indexCollider + 2;
            trisColliderData.index++;
        }
    }

}