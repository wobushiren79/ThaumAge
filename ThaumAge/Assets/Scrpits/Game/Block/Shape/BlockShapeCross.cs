﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCross : Block
{
    public BlockShapeCross() : base()
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
            int startVertsIndex = chunk.chunkMeshData.verts.Count;
            int startTrisIndex = chunk.chunkMeshData.dicTris[blockInfo.material_type].Count;

            int startVertsColliderIndex = 0;
            int startTrisColliderIndex = 0;

            if (blockInfo.collider_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisCollider.Count;
            }
            else if (blockInfo.trigger_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsTrigger.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisTrigger.Count;
            }

            BuildFace(chunk, localPosition, direction, vertsAdd);

            chunk.chunkMeshData.AddMeshIndexData(localPosition,
                     startVertsIndex, vertsAdd.Length, startTrisIndex, trisAdd.Length,
                     startVertsColliderIndex, vertsColliderAdd.Length, startTrisColliderIndex, trisColliderAdd.Length);
        }
    }

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition,DirectionEnum direction)
    {
        //获取下方方块
        Vector3Int downLocalPosition = localPosition + Vector3Int.down;
        chunk.chunkData.GetBlockForLocal(downLocalPosition, out Block downBlock, out DirectionEnum downDirection);
        //如果下方方块为NONE或者为液体
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None || downBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
        {
            Block newBlock = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);
            chunk.chunkData.SetBlockForLocal(localPosition, newBlock, DirectionEnum.UP);
            //更新方块
            WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, this, newBlock, DirectionEnum.UP, false);
        }
    }

    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddTris(chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;
        int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;

        List<int> trisBothFaceSwingData = chunk.chunkMeshData.dicTris[blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisBothFaceSwingData, trisAdd);
        if (blockInfo.collider_state == 1)
            AddTris(triggerIndex, trisCollider, trisColliderAdd);
        if (blockInfo.trigger_state == 1)
            AddTris(triggerIndex, trisTrigger, trisColliderAdd);
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddUVs(chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        AddVerts(localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);
        if (blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
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