using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //模型列表
    public Dictionary<string, GameObject> dicModel = new Dictionary<string, GameObject>();

    //存储着世界中所有的Chunk
    public Dictionary<Vector3, Chunk> dicChunk = new Dictionary<Vector3, Chunk>();

    /// <summary>
    /// 获取区块模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetChunkModel()
    {
        GameObject objModel = GetModel(dicModel, "block/base", "Chunk", "Assets/Prefabs/Game/Chunk.prefab");
        return objModel;
    }

    //public Chunk GetChunk(Vector3 wPos)
    //{
    //    for (int i = 0; i < chunks.Count; i++)
    //    {
    //        Vector3 tempPos = chunks[i].transform.position;

    //        //wPos是否超出了Chunk的XZ平面的范围
    //        if ((wPos.x < tempPos.x) || (wPos.z < tempPos.z) || (wPos.x >= tempPos.x + 20) || (wPos.z >= tempPos.z + 20))
    //            continue;

    //        return chunks[i];
    //    }
    //    return null;
    //}

    /// <summary>
    /// 增加区域
    /// </summary>
    /// <param name="chunk"></param>
    public void AddChunk(Vector3 position, Chunk chunk)
    {
        dicChunk.Add(position, chunk);
    }

    public int seed;
    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;
    public float baseHeight = 10;
    public float frequency = 0.025f;
    public float amplitude = 1;

    public BlockBean[,,] CreateChunkBlockData(int width,int height)
    {
        //初始化随机种子
        Random.InitState(seed);
        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);

        //初始化Map
        BlockBean[,,] mapForBlock = new BlockBean[width, height, width];

        //遍历map，生成其中每个Block的信息
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    mapForBlock[x, y, z] = new BlockBean();
                    mapForBlock[x, y, z].blockType = GenerateBlockType(new Vector3(x, y, z) + transform.position, height);
                }
            }
        }

        return mapForBlock;
    }

    int GenerateHeight(Vector3 wPos)
    {

        //让随机种子，振幅，频率，应用于我们的噪音采样结果
        float x0 = (wPos.x + offset0.x) * frequency;
        float y0 = (wPos.y + offset0.y) * frequency;
        float z0 = (wPos.z + offset0.z) * frequency;

        float x1 = (wPos.x + offset1.x) * frequency * 2;
        float y1 = (wPos.y + offset1.y) * frequency * 2;
        float z1 = (wPos.z + offset1.z) * frequency * 2;

        float x2 = (wPos.x + offset2.x) * frequency / 4;
        float y2 = (wPos.y + offset2.y) * frequency / 4;
        float z2 = (wPos.z + offset2.z) * frequency / 4;

        float noise0 = SimplexNoiseUtil.Generate(x0, y0, z0) * amplitude;
        float noise1 = SimplexNoiseUtil.Generate(x1, y1, z1) * amplitude / 2;
        float noise2 = SimplexNoiseUtil.Generate(x2, y2, z2) * amplitude / 4;

        //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + baseHeight);
    }

    public BlockTypeEnum GenerateBlockType(Vector3 wPos,int height)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height)
        {
            return BlockTypeEnum.None;
        }

        //获取当前位置方块随机生成的高度值
        float genHeight = GenerateHeight(wPos);

        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return BlockTypeEnum.None;
        }
        //当前方块位置等于随机生成的高度值时，当前方块类型为草地
        else if (wPos.y == genHeight)
        {
            return BlockTypeEnum.Grass;
        }
        //当前方块位置小于随机生成的高度值 且 大于 genHeight - 5时，当前方块类型为泥土
        else if (wPos.y < genHeight && wPos.y > genHeight - 5)
        {
            return BlockTypeEnum.Dirt;
        }
        //其他情况，当前方块类型为碎石
        return BlockTypeEnum.Gravel;
    }
}