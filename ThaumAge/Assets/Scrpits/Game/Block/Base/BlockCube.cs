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

        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + up);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + up + right);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + right);
        chunkMeshData.indexVert++;

        AddVert(localPosition, direction, chunkMeshData.vertsCollider, chunkMeshData.indexVertCollider, corner);
        chunkMeshData.indexVertCollider++;
        AddVert(localPosition, direction, chunkMeshData.vertsCollider, chunkMeshData.indexVertCollider, corner + up);
        chunkMeshData.indexVertCollider++;
        AddVert(localPosition, direction, chunkMeshData.vertsCollider, chunkMeshData.indexVertCollider, corner + up + right);
        chunkMeshData.indexVertCollider++;
        AddVert(localPosition, direction, chunkMeshData.vertsCollider, chunkMeshData.indexVertCollider, corner + right);
        chunkMeshData.indexVertCollider++;
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
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition;
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y);
        chunkMeshData.indexUV++;
    }

    public void AddTris(bool reversed, ChunkMeshData chunkMeshData)
    {
        base.AddTris(chunkMeshData);

        int index = chunkMeshData.indexVert;
        int indexCollider = chunkMeshData.indexVertCollider;

        List<int> listTrisNormal = chunkMeshData.dicTris[BlockMaterialEnum.Normal];
        if (reversed)
        {
            listTrisNormal.Add(index + 0);
            listTrisNormal.Add(index + 1);
            listTrisNormal.Add(index + 2);

            listTrisNormal.Add(index + 0);
            listTrisNormal.Add(index + 2);
            listTrisNormal.Add(index + 3);

            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 0;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 1;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 2;
            chunkMeshData.indexTrisCollider++;

            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 0;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 2;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 3;
            chunkMeshData.indexTrisCollider++;
        }
        else
        {
            listTrisNormal.Add(index + 0);
            listTrisNormal.Add(index + 2);
            listTrisNormal.Add(index + 1);

            listTrisNormal.Add(index + 0);
            listTrisNormal.Add(index + 3);
            listTrisNormal.Add(index + 2);

            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 0;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 2;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 1;
            chunkMeshData.indexTrisCollider++;

            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 0;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 3;
            chunkMeshData.indexTrisCollider++;
            chunkMeshData.trisCollider[chunkMeshData.indexTrisCollider] = indexCollider + 2;
            chunkMeshData.indexTrisCollider++;
        }
    }

}