using UnityEditor;
using UnityEngine;

public class BlockTypeWeedMagicLong : BlockBasePlant
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        ColorUtility.TryParseHtmlString("#00796b", out Color colorWeed);
        blockShape.colorsAdd = new Color[]
        {
            colorWeed,colorWeed,colorWeed,colorWeed,
            colorWeed,colorWeed,colorWeed,colorWeed
        };
    }
}