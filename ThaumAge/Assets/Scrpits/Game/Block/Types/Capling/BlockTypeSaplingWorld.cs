using UnityEditor;
using UnityEngine;
using static BiomeCreateTreeTool;

public class BlockTypeSaplingWorld : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 30,
            maxHeight = 50,
            treeTrunk = BlockTypeEnum.TreeWorld,
            treeLeaves = BlockTypeEnum.LeavesWorld,
            leavesRange = 4,
            trunkRange = 3,
        };
        AddTreeForWorldEditor(worldPosition, treeData);
    }
}