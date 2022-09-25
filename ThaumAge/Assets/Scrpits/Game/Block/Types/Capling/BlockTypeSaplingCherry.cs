using UnityEditor;
using UnityEngine;

public class BlockTypeSaplingCherry : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeCherry,
            treeLeaves = BlockTypeEnum.LeavesCherry,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTreeEditor(worldPosition + Vector3Int.down, treeData);
    }
}