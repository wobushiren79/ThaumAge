using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //模型列表
    public Dictionary<string, GameObject> dicModel = new Dictionary<string, GameObject>();

    //存储着世界中所有的Chunk
    public Dictionary<Vector3, Chunk> dicChunk = new Dictionary<Vector3, Chunk>();

    //世界种子
    public int worldSeed;

    /// <summary>
    /// 获取区块模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetChunkModel()
    {
        GameObject objModel = GetModel(dicModel, "block/base", "Chunk", "Assets/Prefabs/Game/Chunk.prefab");
        return objModel;
    }

    /// <summary>
    /// 设置世界种子
    /// </summary>
    /// <param name="worldSeed"></param>
    public void SetWorldSeed(int worldSeed)
    {
        this.worldSeed = worldSeed;
        //初始化随机种子
        Random.InitState(worldSeed);
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

    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;

    public float frequency = 0.025f;
    public float amplitude = 10;


    /// <summary>
    /// 创建区域方块数据
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight">生成地形的最低高低</param>
    /// <returns></returns>
    public Dictionary<Vector3Int, BlockBean> CreateChunkBlockData(int width, int height, int minHeight)
    {

        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);

        //初始化Map
        Dictionary<Vector3Int, BlockBean> mapForBlock = new Dictionary<Vector3Int, BlockBean>();

        int halfWidth = width / 2;
        //遍历map，生成其中每个Block的信息
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    BlockBean blockData = new BlockBean();
                    blockData.position = new Vector3IntBean(x - halfWidth, y, z - halfWidth);
                    blockData.blockType = CreateBlockType(blockData.position.GetVector3Int() + transform.position, height, minHeight);
                    mapForBlock.Add(blockData.position.GetVector3Int(), blockData);
                }
            }
        }
        return mapForBlock;
    }

    public int CreateHeightData(Vector3 wPos, int minHeight)
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
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + minHeight);
    }

    public BlockTypeEnum CreateBlockType(Vector3 wPos, int height, int minHeight)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height)
        {
            return BlockTypeEnum.None;
        }

        //获取当前位置方块随机生成的高度值
        float genHeight = CreateHeightData(wPos, minHeight);
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