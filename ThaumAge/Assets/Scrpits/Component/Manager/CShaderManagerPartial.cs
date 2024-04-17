using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CShaderManager
{
    public Dictionary<BiomeTypeEnum, ComputeShader> dicBiomeCShader = new Dictionary<BiomeTypeEnum, ComputeShader>();
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="callBackForComplete"></param>
    public void LoadResources(Action callBackForComplete)
    {
        dicBiomeCShader.Clear();
        var allBiomeEnum = EnumExtension.GetEnumNames<BiomeTypeEnum>();
        int loadNum = 0;
        for (int i = 0; i < allBiomeEnum.Length; i++)
        {
            var itemBiomeName = allBiomeEnum[i];
            GetComputeShader($"Biome/Biome{itemBiomeName}CShader", (cshader) =>
            {
                BiomeTypeEnum itemBiomeEnum = EnumExtension.GetEnum<BiomeTypeEnum>(itemBiomeName);
                dicBiomeCShader.Add(itemBiomeEnum, cshader);
                loadNum++;
                if (loadNum >= allBiomeEnum.Length)
                {
                    callBackForComplete?.Invoke();
                }
            });
        }
    }

    /// <summary>
    /// 获取地形shader
    /// </summary>
    /// <returns></returns>
    public ComputeShader GetTerrain3DCShader(BiomeTypeEnum biomeType)
    {
        if (dicBiomeCShader.TryGetValue(biomeType, out ComputeShader targetShader))
        {
            return targetShader;
        }
        return null;
    }

}
