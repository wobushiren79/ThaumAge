using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomePrairieLava : Biome
{
    //火山
    public BiomePrairieLava() : base(BiomeTypeEnum.PrairieLava)
    {
    }
    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    // 地表，使用草
        //    return BlockTypeEnum.StoneVolcanic;
        //}
        //if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 5)
        //{
        //    //中使用泥土
        //    return BlockTypeEnum.StoneVolcanic;
        //}
        //else if (localPos.y == 0)
        //{
        //    //基础
        //    return BlockTypeEnum.Foundation;
        //}
        //else
        //{
        //    //其他石头
        //    return BlockTypeEnum.Stone;
        //}
    }

    public BlockTypeEnum GetBlockForMaxHeightUp(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y <= biomeInfo.GetWaterPlaneHeight())
        //{
        //    Block tagetBlock = chunk.chunkData.GetBlockForLocal(localPos);
        //    if (tagetBlock == null || tagetBlock.blockType == BlockTypeEnum.None)
        //    {
        //        return BlockTypeEnum.Magma;
        //    }
        //    return BlockTypeEnum.None;
        //}
        //else
        //{
        //    return BlockTypeEnum.None;
        //}
    }

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeCreatePlantTool.AddDeadwood(startPosition);
    }

    /// <summary>
    /// 增加火焰花
    /// </summary>
    /// <param name="wPos"></param>
    public void AddFireFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreatePlantTool.AddFlower(201, wPos, flowersData);
    }
}