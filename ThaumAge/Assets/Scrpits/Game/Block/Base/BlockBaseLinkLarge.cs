using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseLinkLarge : Block
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
            if (!buildingInfo.CheckCanSetLinkLargeBuilding(buildingWorldPosition))
            {
                buildingInfo.SetLinkLargeBuilding(buildingWorldPosition);
                return;
            }
        }
    }

    /// <summary>
    /// 获取对应的建筑类型
    /// </summary>
    /// <returns></returns>
    public virtual BuildingTypeEnum GetBuildingType()
    {
        return BuildingTypeEnum.None;
    }

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        DestoryBlockForLinkLarge(chunk, localPosition, direction);
    }

    public static void DestoryBlockForLinkLarge(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(BuildingTypeEnum.InfusionAltar);
        chunk.GetBlockForLocal(localPosition, out Block block, out direction, out chunk);
        block.GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        Vector3Int basePosition = blockMetaData.GetBasePosition();
        buildingInfo.ResetLinkLargeBuilding(basePosition, new List<Vector3Int> { localPosition + chunk.chunkData.positionForWorld });
    }
}