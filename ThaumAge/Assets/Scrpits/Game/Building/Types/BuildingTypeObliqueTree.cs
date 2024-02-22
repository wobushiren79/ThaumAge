using UnityEditor;
using UnityEngine;

public class BuildingTypeObliqueTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateObliqueTree(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesPalm);
    }
}