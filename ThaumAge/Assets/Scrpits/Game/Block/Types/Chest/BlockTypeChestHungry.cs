using UnityEditor;
using UnityEngine;

public class BlockTypeChestHungry : BlockBaseChest
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        chestSize = 3 * 7;
    }
}