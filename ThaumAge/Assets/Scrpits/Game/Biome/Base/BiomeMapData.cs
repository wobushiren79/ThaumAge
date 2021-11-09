using UnityEditor;
using UnityEngine;

public class BiomeMapData
{
    //最大高度
    public int maxHeight;
    //距离该方块最近的生态点距离
    public float minBiomeDis = float.MaxValue;
    //距离该方块第二近的生态点距离
    public float secondMinBiomeDis = float.MaxValue;
    //距离最近和第二进的距离差
    public float offsetDis;
    //生态
    public Biome biome;
    public void InitData(Vector3Int wPos, Vector3Int[] listBiomeCenterPosition, Biome[] listBiome)
    {
        //距离该方块最近的生态点距离
        float minBiomeDis = float.MaxValue;
        //距离该方块第二近的生态点距离
        float secondMinBiomeDis = float.MaxValue;
        //最靠近的生态点
        Vector3Int minBiomePosition = Vector3Int.zero;
        //便利中心点，寻找最靠近的生态点（维诺图）
        for (int i = 0; i < listBiomeCenterPosition.Length; i++)
        {
            Vector3Int itemCenterPosition = listBiomeCenterPosition[i];
            float tempDis = Vector3Int.Distance(itemCenterPosition, wPos);

            //如果小于最小距离
            if (tempDis <= minBiomeDis)
            {
                minBiomePosition = itemCenterPosition;
                minBiomeDis = tempDis;
            }
            //如果大于最小距离 并且小于第二小距离
            else if (tempDis > minBiomeDis && tempDis <= secondMinBiomeDis)
            {
                secondMinBiomeDis = tempDis;
            }
        }

        //获取该点的生态信息
        //int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        //RandomTools biomeRandom = RandomUtil.GetRandom(worldSeed, biomeCenterPosition.x, biomeCenterPosition.z);
        //int biomeIndex = biomeRandom.NextInt(listBiome.Count);
        int biomeIndex = WorldRandTools.Range(listBiome.Length, minBiomePosition);
        biome = listBiome[biomeIndex];

        //获取当前位置方块随机生成的高度值
        maxHeight = BiomeHandler.Instance.GetHeightData(wPos, biome.biomeInfo);

        offsetDis = secondMinBiomeDis - minBiomeDis;
    }
}