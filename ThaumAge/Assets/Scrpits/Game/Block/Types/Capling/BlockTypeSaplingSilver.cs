using UnityEditor;
using UnityEngine;
using static BiomeCreateTreeTool;

public class BlockTypeSaplingSilver : BlockBaseSapling
{
    public override void CreateTree(Vector3Int worldPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 1f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeSilver,
            treeLeaves = BlockTypeEnum.LeavesSilver,
            leavesRange = 4,
        };
        BiomeCreateTreeTool.AddTreeForBigEditor(worldPosition - Vector3Int.up, treeData);
    }
}