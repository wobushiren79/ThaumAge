using UnityEditor;
using UnityEngine;

public class BlockTypeWeedGrassLong : BlockBasePlant
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        ColorUtility.TryParseHtmlString("#0a8f08", out Color colorWeed);
        blockShape.colorsAdd = new Color[]
        {
            colorWeed,colorWeed,colorWeed,colorWeed,
            colorWeed,colorWeed,colorWeed,colorWeed
        };
    }
}