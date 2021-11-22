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
        trisAdd = new int[]
        {
            0,1,2, 0,2,3, 4,5,6, 4,6,7
        };
    }

    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        Vector2 uvStartPosition = GetUVStartPosition();

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BuildBlock(chunk, localPosition, direction);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition, direction, vertsAdd);
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

    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddTris(chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;
        int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;

        List<int> trisBothFaceSwingData = chunk.chunkMeshData.dicTris[(int)BlockMaterialEnum.BothFaceSwing];

        AddTris(index, trisBothFaceSwingData, trisAdd);
        AddTris(triggerIndex, chunk.chunkMeshData.trisTrigger, trisAdd);
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddUVs(chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction,Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        AddVerts(localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsAdd);
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
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        return uvStartPosition;
    }

}