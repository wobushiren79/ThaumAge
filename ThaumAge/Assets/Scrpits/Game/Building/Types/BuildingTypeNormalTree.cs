using UnityEditor;
using UnityEngine;

public class BuildingTypeNormalTree : BuildingBaseType
{
    public override void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateNormalTree(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesOak);
    }

    public virtual void CreateBuilding(int blockId, Vector3Int baseWorldPosition, 
        int treeLeaves,int treeMinHeight, int treeMaxHeight, int leavesHeight, int leavesRange)
    {
        BiomeCreateTreeTool.CreateNormalTree(baseWorldPosition, blockId, treeLeaves, treeMinHeight, treeMaxHeight, leavesHeight, leavesRange);
    }
}