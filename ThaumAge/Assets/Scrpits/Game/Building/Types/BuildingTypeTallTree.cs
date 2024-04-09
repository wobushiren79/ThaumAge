using UnityEngine;

public class BuildingTypeTallTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddTreeForTall(blockId, baseWorldPosition, 10, 15, (int)BlockTypeEnum.LeavesBirch, 3);
    }
}