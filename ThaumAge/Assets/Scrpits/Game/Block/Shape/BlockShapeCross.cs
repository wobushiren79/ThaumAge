using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCross : Block
{
    public BlockShapeCross()
    {
        vertsAdd = new Vector3[]
        {
            new Vector3(0.5f,0f,0f),
            new Vector3(0.5f,1f,0f),
            new Vector3(0.5f,1f,1f),
            new Vector3(0.5f,0f,1f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(1f,1f,0.5f),
            new Vector3(1f,0f,0.5f)
        };
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition, direction, chunkMeshData, vertsAdd);
        }
    }

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.RefreshBlock(chunk, localPosition);
        //获取下方方块
        Block downBlock = chunk.chunkData.GetBlockForLocal(localPosition + Vector3Int.down);
        //如果下方方块为NONE或者为液体
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None || downBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
        {
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
            WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk);
        }
    }

    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BaseAddTris(chunk, localPosition, direction, chunkMeshData);

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

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BaseAddUVs(chunk, localPosition, direction, chunkMeshData);
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

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, chunkMeshData, vertsAdd);
        AddVerts(localPosition, direction, chunkMeshData.verts, vertsAdd);
        AddVerts(localPosition, direction, chunkMeshData.vertsTrigger, vertsAdd);
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