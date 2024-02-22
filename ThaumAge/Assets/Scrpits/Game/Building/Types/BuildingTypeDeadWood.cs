using UnityEditor;
using UnityEngine;

public class BuildingTypeDeadWood : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        //增加枯木
        BiomeCreatePlantTool.AddDeadwood(baseWorldPosition);
    }
}