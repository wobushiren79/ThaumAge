using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeTest : Biome
{
    //测试
    public BiomeTest() : base(BiomeTypeEnum.Test)
    {

    }

    public override void CreateBlockBuilding(Chunk chunk, int blockId, int blockBuilding, Vector3Int baseWorldPosition)
    {
        BuildingTypeEnum blockBuildingType = (BuildingTypeEnum)blockBuilding;
        if (blockBuildingType == BuildingTypeEnum.FallDownTree)
        {
            BuildingTypeFallDownTree buildingType = BiomeHandler.Instance.manager.GetBuildingType<BuildingTypeFallDownTree>(blockBuilding);
            buildingType.CreateBuilding(blockId, baseWorldPosition, 3, 6);
        }
        else
        {
            base.CreateBlockBuilding(chunk, blockId, blockBuilding, baseWorldPosition);
        }
    }
}