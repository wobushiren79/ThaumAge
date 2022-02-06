﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropCross : BlockShapeCross
{
    public BlockShapeCropCross() : base()
    {
        BlockBaseCrop.InitCropVert(vertsAdd);
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        Vector2[] uvsAdd = this.GetUVsAddForCrop(chunk, localPosition, block.blockInfo);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public virtual Vector2[] GetUVsAddForCrop(Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockCropBean blockCropData = BlockBaseCrop.FromMetaData<BlockCropBean>(blockData.meta);

        Vector2 uvStartPosition = GetUVStartPosition(blockInfo, blockCropData);
        Vector2[] uvsAdd = new Vector2[]
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
        return uvsAdd;
    }

    /// <summary>
    /// 获取生长UV
    /// </summary>
    public virtual Vector2 GetUVStartPosition(BlockInfoBean blockInfo, BlockCropBean blockCropData)
    {
        BlockBaseCrop blockCrop = BlockHandler.Instance.manager.GetRegisterBlock(blockInfo.id) as BlockBaseCrop;
        List<Vector2Int[]> listUVData = blockCrop.GetListGrowUV();
        Vector2 uvStartPosition;
        if (listUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (blockCropData.growPro >= blockCrop.GetCropLifeCycle())
        {
            //如果生长周期大于UV长度 则取最后一个
            Vector2Int[] itemUVData = listUVData[listUVData.Count - 1];
            uvStartPosition = new Vector2(uvWidth * itemUVData[blockCropData.uvIndex].y, uvWidth * itemUVData[blockCropData.uvIndex].x);
        }
        else
        {
            Vector2Int[] itemUVData = listUVData[blockCropData.growPro];
            //按生长周期取UV
            uvStartPosition = new Vector2(uvWidth * itemUVData[blockCropData.uvIndex].y, uvWidth * itemUVData[blockCropData.uvIndex].x);
        }
        return uvStartPosition;
    }

}