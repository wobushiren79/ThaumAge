using TreeEditor;
using UnityEditor;
using UnityEngine;

public class BuildingTypeWorldTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddTreeForWorld(blockId, baseWorldPosition, 30, 50, 2003);

    }
}