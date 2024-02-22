using UnityEditor;
using UnityEngine;

public class BuildingTypeNormalTreeSnow : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateNormalTreeSnow(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesOak);
    }

    public virtual void CreateBuilding(int blockId, Vector3Int baseWorldPosition,int leavesId, int treeMinHeight, int treeMaxHeight, int leavesHeight, int leavesRange)
    {
        BiomeCreateTreeTool.CreateNormalTreeSnow(baseWorldPosition, blockId, leavesId, treeMinHeight, treeMaxHeight, leavesHeight, leavesRange);
    }
}