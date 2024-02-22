using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeMountain : Biome
{
    protected int maxHight;
    protected int lineSnow;
    protected int lineHalfSnow;
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {

    }

    public override void CreateBlockBuilding(Chunk chunk, int blockId, int blockBuilding, Vector3Int baseWorldPosition)
    {
        if ((BuildingTypeEnum)blockBuilding == BuildingTypeEnum.NormalTreeSnow)
        {
            BuildingTypeNormalTreeSnow buildingType = BiomeHandler.Instance.manager.GetBuildingType<BuildingTypeNormalTreeSnow>(blockBuilding);
            buildingType.CreateBuilding(blockId, baseWorldPosition, (int)BlockTypeEnum.LeavesCherry, 3, 6, 4, 2);
        }
        else
        {
            base.CreateBlockBuilding(chunk, blockId, blockBuilding, baseWorldPosition);
        }
    }
}