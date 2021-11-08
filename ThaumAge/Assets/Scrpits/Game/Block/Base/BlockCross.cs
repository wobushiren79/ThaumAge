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

        int index = chunkMeshData.indexVert;
        int triggerIndex = chunkMeshData.vertsTrigger.Count;

        List<int> listTrisBothFaceSwing = chunkMeshData.dicTris[BlockMaterialEnum.BothFaceSwing];

        listTrisBothFaceSwing.Add(index + 0);
        listTrisBothFaceSwing.Add(index + 1);
        listTrisBothFaceSwing.Add(index + 2);

        listTrisBothFaceSwing.Add(index + 0);
        listTrisBothFaceSwing.Add(index + 2);
        listTrisBothFaceSwing.Add(index + 3);

        listTrisBothFaceSwing.Add(index + 4);
        listTrisBothFaceSwing.Add(index + 5);
        listTrisBothFaceSwing.Add(index + 6);

        listTrisBothFaceSwing.Add(index + 4);
        listTrisBothFaceSwing.Add(index + 6);
        listTrisBothFaceSwing.Add(index + 7);

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
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition;
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(0, uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(uvWidth, uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(uvWidth, 0);
        chunkMeshData.indexUV++;

        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition;
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(0, uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(uvWidth, uvWidth);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = uvStartPosition + new Vector2(uvWidth, 0);
        chunkMeshData.indexUV++;
    }

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {
        base.AddVerts(localPosition, direction, corner, chunkMeshData);
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0.5f, 0, 0));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0.5f, 1, 0));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0.5f, 1, 1));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0.5f, 0, 1));
        chunkMeshData.indexVert++;

        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0, 0, 0.5f));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0, 1, 0.5f));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 1, 0.5f));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 0, 0.5f));
        chunkMeshData.indexVert++;


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