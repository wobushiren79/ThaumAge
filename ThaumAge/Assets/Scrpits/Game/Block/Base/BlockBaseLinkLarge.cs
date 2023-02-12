using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BlockBaseLinkLarge : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 1)
        {
            //检测是否能放下这个多方块结构
            BuildingTypeEnum buildingType = GetBuildingType();
            BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(buildingType);
            Vector3Int buildingWorldPosition = localPosition + chunk.chunkData.positionForWorld;
            if (buildingInfo.CheckCanSetLinkLargeBuilding(buildingWorldPosition, false, out BlockDirectionEnum baseBlockDirection))
            {
                buildingInfo.SetLinkLargeBuilding(buildingWorldPosition, baseBlockDirection);
                return;
            }
        }
    }

    /// <summary>
    /// 获取对应的建筑类型
    /// </summary>
    /// <returns></returns>
    public abstract BuildingTypeEnum GetBuildingType();

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        DestoryBlockForLinkLarge(chunk, localPosition, direction);
    }

    public static void DestoryBlockForLinkLarge(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        chunk.GetBlockForLocal(localPosition, out Block block, out direction, out chunk);
        block.GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        Vector3Int basePosition = blockMetaData.GetBasePosition();

        //获取主方块数据
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(basePosition, out Block baseBlock, out Chunk baseChunk);
        if (baseChunk != null)
        {
            //获取建筑类型
            BlockBaseLinkLarge blockBaseLinkLarge = baseBlock as BlockBaseLinkLarge;
            BuildingTypeEnum buildingType = blockBaseLinkLarge.GetBuildingType();

            BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(buildingType);
            buildingInfo.ResetLinkLargeBuilding(basePosition, new List<Vector3Int> { localPosition + chunk.chunkData.positionForWorld });
        }
    }
}