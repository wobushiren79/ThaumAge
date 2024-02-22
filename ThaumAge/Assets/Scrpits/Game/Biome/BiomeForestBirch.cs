using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeForestBirch : Biome
{
    //森林
    public BiomeForestBirch() : base(BiomeTypeEnum.ForestBirch)
    {

    }

    public override void CreateBlockBuilding(Chunk chunk, int blockId, int blockBuilding, Vector3Int baseWorldPosition)
    {
        BuildingTypeEnum blockBuildingType = (BuildingTypeEnum)blockBuilding;
        if (blockBuildingType == BuildingTypeEnum.FallDownTree)
        {
            BuildingTypeFallDownTree buildingType = BiomeHandler.Instance.manager.GetBuildingType<BuildingTypeFallDownTree>(blockBuilding);
            buildingType.CreateBuilding(blockId, baseWorldPosition, 3, 6);
        }
        else
        {
            base.CreateBlockBuilding(chunk, blockId, blockBuilding, baseWorldPosition);
        }
    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    int waterHeight = biomeInfo.GetWaterPlaneHeight();

        //    if (localPos.y < waterHeight)
        //    {
        //        return BlockTypeEnum.Dirt;
        //    }
        //    else if (localPos.y == waterHeight)
        //    {

        //    }
        //    AddFlowerAndDeadWood(wPos);
        //    AddTreeForFallDown(wPos);
        //    AddTreeForTall(wPos);
        //    AddWeed(wPos);
        //    // 地表，使用草
        //    return BlockTypeEnum.GrassWild;
        //}
        //if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 10)
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

    public void InitBiomeBlockForChunk(Chunk chunk)
    {
        //base.InitBiomeBlockForChunk(chunk, biomeMapData);
        ////获取地形数据
        //ChunkTerrainData startTerrainData = GetTerrainData(chunk, biomeMapData, 0, 0);

        //int waterHeight = biomeInfo.GetWaterPlaneHeight();
        //Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData.maxHeight, chunk.chunkData.positionForWorld.z);
        //if (startTerrainData.maxHeight == waterHeight - 1)
        //{

        //}
        //else if (startTerrainData.maxHeight < waterHeight - 1)
        //{
        //    AddFlowerWater(flowerPosition);
        //}
        //else
        //{
        //    AddFlowerWood(flowerPosition);
        //}
    }


    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTreeForTall(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.008f,
            minHeight = 10,
            maxHeight = 15,
            treeTrunk = BlockTypeEnum.TreeBirch,
            treeLeaves = BlockTypeEnum.LeavesBirch,
            leavesRange = 3,
        };
        //BiomeCreateTreeTool.AddTreeForTall(201, wPos, treeData);
    }
}