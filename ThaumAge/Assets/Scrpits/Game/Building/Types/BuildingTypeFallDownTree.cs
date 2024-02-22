using UnityEditor;
using UnityEngine;

public class BuildingTypeFallDownTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddTreeForFallDown(baseWorldPosition, blockId);
    }

    public void CreateBuilding(int blockId, Vector3Int baseWorldPosition, int treeMinHeight, int treeMaxHeight)
    {
        BiomeCreateTreeTool.AddTreeForFallDown(baseWorldPosition, blockId, treeMinHeight, treeMaxHeight);
    }
}