using UnityEditor;
using UnityEngine;

public class Biome
{
    public BiomeTypeEnum biomeType;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
    }

    public struct TreeData 
    {
        public int addRateMin;
        public int addRateMax;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum treeTrunk;//树干
        public BlockTypeEnum treeLeaves;//树叶
    }

    public struct WeedData 
    {
        public int addRateMin;
        public int addRateMax;
    }

    public struct Cactus
    {
        public int addRateMin;
        public int addRateMax;
    }

    public struct Flowers
    {
        public int addRateMin;
        public int addRateMax;
    }


    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        return BlockTypeEnum.Stone;
    }

    /// <summary>
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public virtual void AddTree(Vector3Int startPosition, TreeData treeData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        System.Random random = new System.Random(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        int addRate = random.Next(0, treeData.addRateMax);
        if (addRate < treeData.addRateMin)
        {
            for (int i = 0; i < 5; i++)
            {
                BlockBean blockData = new BlockBean(treeData.treeTrunk, Vector3Int.zero, startPosition + Vector3Int.up * (i + 1));
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
            }
        }
    }

    public virtual void AddWeed(Vector3Int startPosition, WeedData weedData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        System.Random random = new System.Random(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        int addRate = random.Next(0, weedData.addRateMax);
        if (addRate < weedData.addRateMin)
        {
            BlockBean blockData = new BlockBean(BlockTypeEnum.Weed_Normal, Vector3Int.zero, startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }
}