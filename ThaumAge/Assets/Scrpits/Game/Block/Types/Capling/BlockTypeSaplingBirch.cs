using UnityEditor;
using UnityEngine;

public class BlockTypeSaplingBirch : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 10,
            maxHeight = 15,
            treeTrunk = BlockTypeEnum.TreeBirch,
            treeLeaves = BlockTypeEnum.LeavesBirch,
            leavesRange = 3,
        };
        BiomeCreateTreeTool.AddTreeForTallEditor(worldPosition - new Vector3Int(0, 1, 0), treeData);
    }
}