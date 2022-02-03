using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapePlantCross : BlockShapeCross
{
    public BlockShapePlantCross() : base()
    {
        BlockBasePlant.InitPlantVert(vertsAdd);
    }

    public override void BaseAddUVs(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        Vector2[] uvsAdd = this.GetUVsAddForPlant(block, chunk, localPosition, block.blockInfo);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public virtual Vector2[] GetUVsAddForPlant(Block block, Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockBasePlant.FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);

        Vector2 uvStartPosition = GetUVStartPosition(blockInfo, BlockShape.uvWidth, growPro);
        Vector2[] uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + BlockShape.uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
        return uvsAdd;
    }

    /// <summary>
    /// 获取生长UV
    /// </summary>
    public virtual Vector2 GetUVStartPosition(BlockInfoBean blockInfo, float uvWidth, int growth)
    {
        Vector2Int[] arrayUVData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (growth >= arrayUVData.Length)
        {
            //如果生长周期大于UV长度 则取最后一个
            uvStartPosition = new Vector2(uvWidth * arrayUVData[arrayUVData.Length - 1].y, uvWidth * arrayUVData[arrayUVData.Length - 1].x);
        }
        else
        {
            //按生长周期取UV
            uvStartPosition = new Vector2(uvWidth * arrayUVData[growth].y, uvWidth * arrayUVData[growth].x);
        }
        return uvStartPosition;
    }

}