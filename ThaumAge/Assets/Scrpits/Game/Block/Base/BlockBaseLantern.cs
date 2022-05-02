using UnityEditor;
using UnityEngine;
public class BlockBaseLantern : Block
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        BlockShapeCustomDirectionUpDown blockShapeCustomDirectionUpDown = blockShape as BlockShapeCustomDirectionUpDown;
        for (int i = 0; i < blockShapeCustomDirectionUpDown.colorsAddDirection.Length; i++)
        {
            blockShapeCustomDirectionUpDown.colorsAddDirection[i].a = 2;
        }
        for (int i = 0; i < blockShapeCustomDirectionUpDown.colorsAdd.Length; i++)
        {
            blockShapeCustomDirectionUpDown.colorsAdd[i].a = 2;
        }
    }

    public override Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        return Vector3.zero;
    }
}