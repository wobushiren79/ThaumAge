using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    public Vector3 offset0;
    public Vector3 offset1;
    public Vector3 offset2;
    public float offsetBiome;
    public void InitWorldBiomeSeed()
    {
        offsetBiome = Random.value * 1000;
        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
    }

    /// <summary>
    /// 根据生物生态 创造方块
    /// </summary>
    /// <param name="listBiome"></param>
    /// <param name="wPos"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public BlockTypeEnum CreateBiomeBlockType(Chunk chunk, List<Biome> listBiome, Vector3Int blockLocPosition)
    {
        Vector3Int wPos = blockLocPosition + chunk.worldPosition;
        //y坐标是否在Chunk内
        if (wPos.y >= chunk.height)
        {
            return BlockTypeEnum.None;
        }

        float strongestWeight = float.MinValue;
        int strongestBiomeIndex = 0;

        BiomeInfoBean biomeInfo = null;

        for (int i = 0; i < listBiome.Count; i++)
        {
            Biome itemBiome = listBiome[i];
            BiomeInfoBean tempBiomeInfo = manager.GetBiomeInfo(itemBiome.biomeType);

            float weight = SimplexNoiseUtil.Generate(new Vector2(wPos.x, wPos.z), offsetBiome, tempBiomeInfo.scale);
            if (weight >= strongestWeight)
            {
                strongestWeight = weight;
                strongestBiomeIndex = i;
                biomeInfo = tempBiomeInfo;
            }
        }

        //获取当前位置方块随机生成的高度值
        int genHeight = GetHeightData(wPos, biomeInfo);
        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return BlockTypeEnum.None;
        }
        Biome biome = listBiome[strongestBiomeIndex];
        //获取方块
        return biome.GetBlockType(genHeight, wPos);
    }

    /// <summary>
    /// 获取高度信息
    /// </summary>
    /// <param name="wPos"></param>
    /// <param name="biomeInfo"></param>
    /// <returns></returns>
    public int GetHeightData(Vector3 wPos, BiomeInfoBean biomeInfo)
    {

        //让随机种子，振幅，频率，应用于我们的噪音采样结果
        float x0 = (wPos.x + offset0.x) * biomeInfo.frequency;
        float y0 = (wPos.y + offset0.y) * biomeInfo.frequency;
        float z0 = (wPos.z + offset0.z) * biomeInfo.frequency;

        float x1 = (wPos.x + offset1.x) * biomeInfo.frequency * 2;
        float y1 = (wPos.y + offset1.y) * biomeInfo.frequency * 2;
        float z1 = (wPos.z + offset1.z) * biomeInfo.frequency * 2;

        float x2 = (wPos.x + offset2.x) * biomeInfo.frequency / 4;
        float y2 = (wPos.y + offset2.y) * biomeInfo.frequency / 4;
        float z2 = (wPos.z + offset2.z) * biomeInfo.frequency / 4;

        float noise0 = SimplexNoiseUtil.Generate(x0, y0, z0) * biomeInfo.amplitude;
        float noise1 = SimplexNoiseUtil.Generate(x1, y1, z1) * biomeInfo.amplitude / 2;
        float noise2 = SimplexNoiseUtil.Generate(x2, y2, z2) * biomeInfo.amplitude / 4;

        //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + biomeInfo.minHeight);
    }


}