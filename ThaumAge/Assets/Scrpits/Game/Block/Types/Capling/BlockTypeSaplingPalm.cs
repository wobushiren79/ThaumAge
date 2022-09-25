using UnityEditor;
using UnityEngine;

public class BlockTypeSaplingPalm : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 5,
            maxHeight = 8,
            treeTrunk = BlockTypeEnum.TreePalm,
            treeLeaves = BlockTypeEnum.LeavesPalm,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTreeForObliqueEditor(worldPosition + Vector3Int.down, treeData);
    }
}