using UnityEditor;
using UnityEngine;

public class Biome 
{
    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(int genHeight)
    {
        return BlockTypeEnum.Grass;
    }
}