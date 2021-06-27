using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeVolcano : Biome
{
    //火山
    public BiomeVolcano() : base(BiomeTypeEnum.Volcano)
    {
    }

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(biomeInfo, genHeight, localPos, wPos);
        float noise = (genHeight - biomeInfo.minHeight) / biomeInfo.amplitude;
        if (noise >= 0.9f)
        {
            if (localPos.y >= genHeight - 3)
            {
                return BlockTypeEnum.None;
            }
            else if (localPos.y > 20 && localPos.y < genHeight - 1)
            {
                return BlockTypeEnum.Magma;
            }
            else
            {
                return BlockTypeEnum.StoneVolcanic;
            }
        }
        if (genHeight == localPos.y)
        {
            AddDeadwood(wPos);
        }
        return BlockTypeEnum.StoneVolcanic;
    }

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeForTreeData treeData = new BiomeForTreeData();
        treeData.addRate = 0.005f;
        treeData.minHeight = 1;
        treeData.maxHeight = 8;
        treeData.treeTrunk = BlockTypeEnum.TreeOak;
        BiomeCreateTool.AddDeadwood(101, startPosition, treeData);
    }
}