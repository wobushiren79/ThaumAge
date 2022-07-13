using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomeForestMagic : Biome
{

    //魔法深林
    public BiomeForestMagic() : base(BiomeTypeEnum.ForestMagic)
    {
    }

    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        if (localPos.y == terrainData.maxHeight)
        {
            if (localPos.y >= biomeInfo.GetWaterPlaneHeight())
            {
                Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
                AddWeed(wPos);
                AddBigTree(wPos);
                AddWorldTree(wPos);
                AddMushroomTree(wPos);
                AddStoneMoss(wPos);
                AddFlower(wPos);
                AddDeadwood(wPos);
            }
            if (localPos.y == biomeInfo.GetWaterPlaneHeight() || localPos.y == biomeInfo.GetWaterPlaneHeight() + 1)
            {
                // 地表，使用草
                return BlockTypeEnum.Sand;
            }
            else
            {
                // 地表，使用草
                return BlockTypeEnum.GrassMagic;
            }
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

    public override void InitBiomeBlockForChunk(Chunk chunk, BiomeMapData biomeMapData)
    {
        base.InitBiomeBlockForChunk(chunk, biomeMapData);

        //获取中心点的地形数据
        Vector3Int centerPosition = new Vector3Int(8,0,8);
        Vector3Int chunkPosition = chunk.chunkData.positionForWorld;
        //获取地形数据
        ChunkTerrainData centerPositionTerrainData = GetTerrainData(chunk, biomeMapData, centerPosition.x, centerPosition.z);

        Vector3Int bigTreeWorldPosition = new Vector3Int
            (chunkPosition.x + centerPosition.x, Mathf.RoundToInt(centerPositionTerrainData.maxHeight), chunkPosition.z + centerPosition.z);
        AddBigTree(bigTreeWorldPosition);
    }

    protected void AddMushroomTree(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        AddBuilding(0.0001f, 101, startPosition, BuildingTypeEnum.MushrooBig);
        AddBuilding(0.0001f, 201, startPosition, BuildingTypeEnum.Mushroom);
        AddBuilding(0.0001f, 301, startPosition, BuildingTypeEnum.MushrooSmall);
    }

    protected void AddTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(401, wPos, treeData);
    }

    protected void AddBigTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeSilver,
            treeLeaves = BlockTypeEnum.LeavesSilver,
            leavesRange = 4,
        };
        BiomeCreateTreeTool.AddTreeForBig(501, wPos, treeData);
    }

    protected void AddWorldTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.00005f,
            minHeight = 30,
            maxHeight = 50,
            treeTrunk = BlockTypeEnum.TreeWorld,
            treeLeaves = BlockTypeEnum.LeavesWorld,
            leavesRange = 4,
            trunkRange = 3,
        };
        BiomeCreateTreeTool.AddTreeForWorld(wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.3f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedMagicLong, BlockTypeEnum.WeedMagicNormal, BlockTypeEnum.WeedMagicShort, BlockTypeEnum.WeedMagicStart }
        };
        BiomeCreatePlantTool.AddPlant(601, wPos, weedData);
    }

    public void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.MushroomLuminous }
        };
        BiomeCreatePlantTool.AddFlower(701, wPos, flowersData);
    }

    protected void AddStoneMoss(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        AddBuilding(0.005f, 801, startPosition, BuildingTypeEnum.StoneMoss);
    }

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeCreatePlantTool.AddDeadwood(101, 0.001f, startPosition);
    }
}