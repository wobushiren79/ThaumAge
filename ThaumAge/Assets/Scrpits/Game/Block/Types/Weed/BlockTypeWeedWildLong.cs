using UnityEditor;
using UnityEngine;

public class BlockTypeWeedWildLong : BlockBasePlant
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        ColorUtility.TryParseHtmlString("#9e9d24", out Color colorWeed);
        blockShape.colorsAdd = new Color[]
        {
            colorWeed,colorWeed,colorWeed,colorWeed,
            colorWeed,colorWeed,colorWeed,colorWeed
        };
    }
}