using UnityEngine;

public class BuildingTypeSalixTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        bool isAdd = BiomeCreateTreeTool.AddTreeForSalix(blockId, baseWorldPosition, 8, 12, (int)BlockTypeEnum.LeavesOak, 4, 5, 8);
    }
}