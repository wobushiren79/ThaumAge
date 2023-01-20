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
            BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(BuildingTypeEnum.InfusionAltar);
            Vector3Int buildingWorldPosition = localPosition + chunk.chunkData.positionForWorld;
            if (!buildingInfo.CheckCanSetLinkLargeBuilding(buildingWorldPosition))
            {
                buildingInfo.SetLinkLargeBuilding(buildingWorldPosition);
                //CreateLinkBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, listLinkPosition);
                return;
            }
        }
    }
}