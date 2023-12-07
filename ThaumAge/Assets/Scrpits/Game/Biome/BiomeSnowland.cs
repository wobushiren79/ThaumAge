using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BiomeSnowland : Biome
{
    //海洋
    public BiomeSnowland() : base(BiomeTypeEnum.Snowland)
    {

    }

    public BlockTypeEnum GetBlockForMaxHeightUp(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        //int waterPlaneHeight = biomeInfo.GetWaterPlaneHeight();
        //if (localPos.y == waterPlaneHeight)
        //{
        //    if (terrainData.maxHeight > 60)
        //    {
        //        Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //        //AddIceHalf(wPos);
        //        //生成概率
        //        float addRate = WorldRandTools.GetValue(wPos, 111);
        //        if (addRate < 0.01f)
        //        {
        //            return BlockTypeEnum.Water;
        //        }
        //        else
        //        {
        //            return BlockTypeEnum.StoneIce;
        //        }
        //    }
        //    return BlockTypeEnum.Water;
        //}
        //else if (localPos.y == waterPlaneHeight + 1)
        //{
        //    if (localPos.y == terrainData.maxHeight + 1)
        //    {
        //        if (terrainData.maxHeight > 60)
        //        {
        //            return BlockTypeEnum.HalfStoneSnow;
        //        }
        //        else
        //        {
        //            return BlockTypeEnum.StoneSnow;
        //        }
        //    }
        //}
        //else if (localPos.y < waterPlaneHeight)
        //{
        //    Block tagetBlock = chunk.chunkData.GetBlockForLocal(localPos);
        //    if (tagetBlock == null || tagetBlock.blockType == BlockTypeEnum.None)
        //    {
        //        return BlockTypeEnum.Water;
        //    }
        //}
        //return BlockTypeEnum.None;
    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //int waterPlaneHeight = biomeInfo.GetWaterPlaneHeight();
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;      
        //    // 地表，使用草
        //    if (localPos.y > waterPlaneHeight)
        //    {
        //        AddWeed(wPos);
        //        if (localPos.y == waterPlaneHeight + 1)
        //        {
        //            return BlockTypeEnum.StoneSnow;
        //        }
        //        return BlockTypeEnum.GrassSnow;
        //    }
        //    else
        //    {
        //        return BlockTypeEnum.Dirt;
        //    }
        //}
        //else if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 5)
        //{
        //    //中使用泥土
        //    return BlockTypeEnum.Dirt;
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

    /// <summary>
    /// 添加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedSnowLong, BlockTypeEnum.WeedSnowNormal, BlockTypeEnum.WeedSnowShort, BlockTypeEnum.WeedSnowStart }
        };
        BiomeCreatePlantTool.AddPlant(333, wPos, weedData);
    }

    protected void AddIceHalf(Vector3Int wPos)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(wPos, 111);
        if (addRate < 0.1f)
        {
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x, wPos.y + 1, wPos.z, BlockTypeEnum.HalfStoneIce);
        }
    }
}