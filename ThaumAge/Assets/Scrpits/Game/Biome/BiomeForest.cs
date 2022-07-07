using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeForest : Biome
{
    //森林
    public BiomeForest() : base(BiomeTypeEnum.Forest)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            int waterHeight = biomeInfo.GetWaterPlaneHeight();
            if(localPos.y == waterHeight)
            {
                AddStoneMoss(wPos);
                return BlockTypeEnum.Sand;
            }
            else if (localPos.y < waterHeight)
            {
                return BlockTypeEnum.Dirt;
            }

            AddWeed(wPos);
            AddFlowerAndDeadWood(wPos);
            //AddTree(wPos);
            //AddBigTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass;
        }
        if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 10)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (localPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            //其他石头
            return BlockTypeEnum.Stone;
        }
    }

    /// <summary>
    /// 添加花花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerAndDeadWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.01f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum}
        };
        BiomeCreatePlantTool.AddFlower(101,wPos, flowersData);
        //增加枯木
        BiomeCreatePlantTool.AddDeadwood(102,0.001f, wPos);
    }

    protected void AddTree(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.05f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(111, wPos, treeData);
    }

    protected void AddBigTree(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 4,
        };
        BiomeCreateTreeTool.AddTreeForBig(222,wPos, treeData);
    }


    /// <summary>
    /// 添加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.5f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedLong, BlockTypeEnum.WeedNormal, BlockTypeEnum.WeedShort }
        };
        BiomeCreatePlantTool.AddPlant(333,wPos, weedData);
    }

    /// <summary>
    /// 增加苔石
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddStoneMoss(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        BiomeCreateTool.AddBuilding(0.01f, 801, startPosition, BuildingTypeEnum.StoneMoss);
    }
}