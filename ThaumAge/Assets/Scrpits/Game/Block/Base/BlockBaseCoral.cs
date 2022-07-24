using UnityEditor;
using UnityEngine;

public class BlockBaseCoral : BlockBaseLiquidSame
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        InitCoralColor();
    }
    public override bool CheckIsSameType(Chunk closeChunk, Block closeBlock)
    {
        return BlockTypeWater.CheckIsSameTypeCommon(closeChunk, closeBlock);
    }

    /// <summary>
    /// 初始化颜色
    /// </summary>
    public virtual void InitCoralColor()
    {
        ColorUtility.TryParseHtmlString(GetCoralColorStr(), out Color colorWeed);
        blockShape.colorsAdd = new Color[]
        {
            colorWeed,colorWeed,colorWeed,colorWeed,
            colorWeed,colorWeed,colorWeed,colorWeed
        };
    }

    /// <summary>
    /// 获取珊瑚颜色
    /// </summary>
    public virtual string GetCoralColorStr()
    {
        return "#000000";
    }
}