using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTreeTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override void CreateBlockBuilding(Chunk chunk, int blockId, int blockBuilding, Vector3Int baseWorldPosition)
    {
        if ((BuildingTypeEnum)blockBuilding == BuildingTypeEnum.NormalTree)
        {
            BuildingTypeNormalTree buildingType = BiomeHandler.Instance.manager.GetBuildingType<BuildingTypeNormalTree>(blockBuilding);
            buildingType.CreateBuilding(blockId, baseWorldPosition, (int)BlockTypeEnum.LeavesCherry, 6, 10, 4, 2);
        }
        else
        {
            base.CreateBlockBuilding(chunk, blockId, blockBuilding, baseWorldPosition);
        }
    }
}