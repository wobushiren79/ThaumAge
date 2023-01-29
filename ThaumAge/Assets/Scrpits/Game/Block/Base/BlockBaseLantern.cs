using UnityEditor;
using UnityEngine;
public class BlockBaseLantern : Block
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        BlockShapeCustomDirectionUpDown blockShapeCustomDirectionUpDown = blockShape as BlockShapeCustomDirectionUpDown;
        blockShapeCustomDirectionUpDown.SetColorsEmission(2);
    }

    public override Vector3 GetRotateAngles(BlockDirectionEnum direction)
    {
        return Vector3.zero;
    }
}