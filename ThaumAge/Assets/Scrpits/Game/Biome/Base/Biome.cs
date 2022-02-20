using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class Biome
{
    public BiomeTypeEnum biomeType;
    public BiomeInfoBean biomeInfo;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
        biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(this.biomeType);
    }

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(Chunk chunk,BiomeInfoBean biomeInfo,int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        return BlockTypeEnum.Stone;
    }

}