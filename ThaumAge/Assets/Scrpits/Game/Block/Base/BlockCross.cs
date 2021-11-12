using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCross : Block
{
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(localPosition, direction, localPosition, chunkMeshData);
        }
    }

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.RefreshBlock(chunk, localPosition, direction);
        //获取下方方块
        chunk.GetBlockForLocal(localPosition + Vector3Int.down, out Block downBlock, out DirectionEnum downBlockdirection, out bool isInside);
        //如果下方方块为NONE或者为液体
        if (isInside && (downBlock == null || downBlock.blockType == BlockTypeEnum.None || downBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid))
        {
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
            WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk);
        }
    }

    public override void AddTris(ChunkMeshData chunkMeshData)
    {
        base.AddTris(chunkMeshData);

        int index = chunkMeshData.vertsData.index;
        int triggerIndex = chunkMeshData.vertsTrigger.Count;

        ChunkMeshTrisData trisBothFaceSwingData = chunkMeshData.dicTris[(int)BlockMaterialEnum.BothFaceSwing];

        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 1;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 2;
        trisBothFaceSwingData.index++;

        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 2;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 3;
        trisBothFaceSwingData.index++;

        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 4;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 5;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 6;
        trisBothFaceSwingData.index++;

        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 4;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 6;
        trisBothFaceSwingData.index++;
        trisBothFaceSwingData.tris[trisBothFaceSwingData.index] = index + 7;
        trisBothFaceSwingData.index++;

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 1);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);
        chunkMeshData.trisTrigger.Add(triggerIndex + 3);

        chunkMeshData.trisTrigger.Add(triggerIndex + 4);
        chunkMeshData.trisTrigger.Add(triggerIndex + 5);
        chunkMeshData.trisTrigger.Add(triggerIndex + 6);

        chunkMeshData.trisTrigger.Add(triggerIndex + 4);
        chunkMeshData.trisTrigger.Add(triggerIndex + 6);
        chunkMeshData.trisTrigger.Add(triggerIndex + 7);
    }

    public override void AddUVs(ChunkMeshData chunkMeshData)
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
        else
        {
            //随机选一个
            uvStartPosition = Vector2.zero;
        }
        ChunkMeshUVData uvsData = chunkMeshData.uvsData;

        uvsData.uvs[uvsData.index] = uvStartPosition;
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(0, uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(uvWidth, uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(uvWidth, 0);
        uvsData.index++;

        uvsData.uvs[uvsData.index] = uvStartPosition;
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(0, uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(uvWidth, uvWidth);
        uvsData.index++;
        uvsData.uvs[uvsData.index] = uvStartPosition + new Vector2(uvWidth, 0);
        uvsData.index++;
    }

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {
        base.AddVerts(localPosition, direction, corner, chunkMeshData);
        ChunkMeshVertsData vertsData = chunkMeshData.vertsData;

        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0.5f, 0, 0));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0.5f, 1, 0));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0.5f, 1, 1));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0.5f, 0, 1));
        vertsData.index++;

        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0, 0, 0.5f));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0, 1, 0.5f));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 1, 0.5f));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 0, 0.5f));
        vertsData.index++;


        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 0, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 1, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 1, 1));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 0, 1));

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 0, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 1, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 1, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 0, 0.5f));
    }


}