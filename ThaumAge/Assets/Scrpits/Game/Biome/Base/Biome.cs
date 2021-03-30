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
    public virtual BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        return BlockTypeEnum.Stone;
    }

    public virtual void AddTree(Vector3Int startPosition)
    {
        int addRate = WorldCreateHandler.Instance.manager.worldRandom.Next(0, 100);
        if (addRate < 2)
        {
            for (int i = 0; i < 5; i++)
            {
                BlockBean blockData = new BlockBean(BlockTypeEnum.Oak, Vector3Int.zero, startPosition + Vector3Int.up * (i + 1));
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
            }
        }
    }

    public virtual void AddWeed(Vector3Int startPosition)
    {
        int addRate = WorldCreateHandler.Instance.manager.worldRandom.Next(0, 100);
        if (addRate < 10)
        {
            BlockBean blockData = new BlockBean(BlockTypeEnum.Weed_Normal, Vector3Int.zero, startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }
}