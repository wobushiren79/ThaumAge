using UnityEditor;
using UnityEngine;

public class BuildingTypeSeaweed : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreatePlantTool.AddLongPlant(blockId, baseWorldPosition);
    }
}