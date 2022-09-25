using UnityEditor;
using UnityEngine;
using static BiomeCreateTreeTool;

public class BlockTypeSaplingWalnut : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeWalnut,
            treeLeaves = BlockTypeEnum.LeavesWalnut,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTreeEditor(worldPosition - Vector3Int.up, treeData);
    }
}