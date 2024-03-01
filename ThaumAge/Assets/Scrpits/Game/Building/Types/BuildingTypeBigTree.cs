using TreeEditor;
using UnityEditor;
using UnityEngine;

public class BuildingTypeBigTree : BuildingBaseType
{

    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddTreeForBig(blockId, baseWorldPosition, 6, 10, 2002, 4);
    }

}