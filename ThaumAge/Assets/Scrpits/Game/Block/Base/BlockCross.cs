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
            BuildFace(chunk, localPosition, direction, chunkMeshData, localPosition);
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

    public override void AddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.AddTris(chunk, localPosition, direction, chunkMeshData);

        int index = chunkMeshData.verts.Count;
        int triggerIndex = chunkMeshData.vertsTrigger.Count;

        List<int> trisBothFaceSwingData = chunkMeshData.dicTris[(int)BlockMaterialEnum.BothFaceSwing];

        trisBothFaceSwingData.Add(index);
        trisBothFaceSwingData.Add(index + 1);
        trisBothFaceSwingData.Add(index + 2);

        trisBothFaceSwingData.Add(index);
        trisBothFaceSwingData.Add(index + 2);
        trisBothFaceSwingData.Add(index + 3);

        trisBothFaceSwingData.Add(index + 4);
        trisBothFaceSwingData.Add(index + 5);
        trisBothFaceSwingData.Add(index + 6);

        trisBothFaceSwingData.Add(index + 4);
        trisBothFaceSwingData.Add(index + 6);
        trisBothFaceSwingData.Add(index + 7);

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

    public override void AddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.AddUVs(chunk, localPosition, direction, chunkMeshData);
        Vector2 uvStartPosition =  GetUVStartPosition();

        List<Vector2> uvs = chunkMeshData.uvs;
        uvs.Add(uvStartPosition);
        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));

        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y));
        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));
    }

    public override void AddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3 corner)
    {
        base.AddVerts(chunk, localPosition, direction, chunkMeshData, corner);
        List<Vector3> verts = chunkMeshData.verts;

        AddVert(localPosition, direction, verts, new Vector3(corner.x + 0.5f, corner.y, corner.z));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 0.5f, corner.y + 1f, corner.z));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 0.5f, corner.y + 1f, corner.z + 1f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 0.5f, corner.y, corner.z + 1f));

        AddVert(localPosition, direction, verts, new Vector3(corner.x, corner.y, corner.z + 0.5f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x, corner.y + 1f, corner.z + 0.5f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y + 1f, corner.z + 0.5f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y, corner.z + 0.5f));


        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 0, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 1, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 1, 1));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0.5f, 0, 1));

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 0, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 1, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 1, 0.5f));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 0, 0.5f));
    }


    public virtual Vector2 GetUVStartPosition()
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
        else
        {
            //随机选一个
            uvStartPosition = Vector2.zero;
        }
        return uvStartPosition;
    }

}