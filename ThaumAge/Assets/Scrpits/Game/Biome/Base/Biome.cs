using UnityEditor;
using UnityEngine;

public class Biome 
{
    public BiomeTypeEnum biomeType;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
    }

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(int genHeight, Vector3Int wPos)
    {
        return BlockTypeEnum.Grass;
    }
}