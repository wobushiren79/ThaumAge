using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    public Vector3 offset0;
    public Vector3 offset1;
    public Vector3 offset2;

    public void InitWorldBiomeSeed()
    {
        offset0 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
        offset1 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
        offset2 = new Vector3(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
    }

    public BlockTypeEnum CreateBiomeBlockType(Vector3 wPos, int width, int height)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height)
        {
            return BlockTypeEnum.None;
        }

        BiomeInfoBean biomeInfo = manager.GetBiomeInfo(1);
        //获取当前位置方块随机生成的高度值
        float genHeight = CreateHeightData(wPos, biomeInfo);

        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return BlockTypeEnum.None;
        }
        float weight = SimplexNoiseUtil.Get2DPerlin(new Vector2(wPos.x, wPos.z), 100, width * 2);
        if (weight > 0.5f)
        {
            return BlockTypeEnum.Grass;
        }
        ////当前方块位置等于随机生成的高度值时，当前方块类型为草地
        //else if (wPos.y == genHeight)
        //{
        //    return BlockTypeEnum.Grass;
        //}
        ////当前方块位置小于随机生成的高度值 且 大于 genHeight - 5时，当前方块类型为泥土
        //else if (wPos.y < genHeight && wPos.y > genHeight - 5)
        //{
        //    return BlockTypeEnum.Dirt;
        //}
        ////其他情况，当前方块类型为碎石
        return BlockTypeEnum.Stone;
    }

    public int CreateHeightData(Vector3 wPos, BiomeInfoBean biomeInfo)
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