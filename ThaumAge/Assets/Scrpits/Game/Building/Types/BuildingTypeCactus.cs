using UnityEditor;
using UnityEngine;

public class BuildingTypeCactus : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddCactus(baseWorldPosition, blockId);
    }
}