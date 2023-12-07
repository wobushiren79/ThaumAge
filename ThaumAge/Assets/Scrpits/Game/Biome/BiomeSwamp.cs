using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeSwamp : Biome
{
    //沙漠
    public BiomeSwamp() : base(BiomeTypeEnum.Swamp)
    {
    }
    public BlockTypeEnum GetBlockForMaxHeightUp(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //int waterHeight = biomeInfo.GetWaterPlaneHeight();
        //if (localPos.y == waterHeight + 1 && localPos.y > terrainData.maxHeight + 1)
        //{
        //    AddLotusLeaf(localPos + chunk.chunkData.positionForWorld);
        //}
        //return base.GetBlockForMaxHeightUp(chunk, localPos, terrainData);
    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    int waterHeight = biomeInfo.GetWaterPlaneHeight();
        //    if (localPos.y == waterHeight)
        //    {

        //    }
        //    else if (localPos.y < waterHeight)
        //    {
        //        int waterOffset = waterHeight - localPos.y;
        //        AddTree(wPos, waterOffset);
        //        return BlockTypeEnum.Dirt;
        //    }
        //    AddWeed(wPos);
        //    AddTree(wPos, 0);
        //    // 地表，使用草
        //    return BlockTypeEnum.Grass;
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
    /// 增加元素花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWood }
        };
        BiomeCreatePlantTool.AddFlower(110, wPos, flowersData);
    }
    protected void AddFlowerWater(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWater }
        };
        BiomeCreatePlantTool.AddFlower(111, wPos, flowersData);
    }

    /// <summary>
    /// 增加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(201, wPos, weedData);
    }

    /// <summary>
    /// 增加柳树
    /// </summary>
    protected void AddTree(Vector3Int wPos, int heightOffset)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.005f,
            minHeight = 8 + heightOffset,
            maxHeight = 12 + heightOffset,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            minLeavesHeight = 5 - heightOffset,
            maxLeavesHeight = 8 - heightOffset,
            leavesRange = 4,
        };
        bool isAdd = BiomeCreateTreeTool.AddTreeForSalix(301, wPos.AddY(-heightOffset), treeData);
        if (isAdd && heightOffset > 0)
        {
            AddTreeBase(wPos, heightOffset);
        }
    }

    /// <summary>
    /// 增加柳树的地基
    /// </summary>
    protected void AddTreeBase(Vector3Int wPos, int heightOffset)
    {
        int offsetOutL = 1;
        int offsetOutR = 1;
        int offsetOutF = 1;
        int offsetOutB = 1;

        int offsetDirectionL = WorldRandTools.Range(0, 2);
        int offsetDirectionR = WorldRandTools.Range(0, 2);
        int offsetDirectionF = WorldRandTools.Range(0, 2);
        int offsetDirectionB = WorldRandTools.Range(0, 2);

        BlockTypeEnum baseTypeL = BlockTypeEnum.Grass;
        BlockTypeEnum baseTypeR = BlockTypeEnum.Grass;
        BlockTypeEnum baseTypeF = BlockTypeEnum.Grass;
        BlockTypeEnum baseTypeB = BlockTypeEnum.Grass;

        for (int h = heightOffset + 3; h > 0; h--)
        {
            int posY = wPos.y + h;
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x - offsetOutL, posY, wPos.z + offsetDirectionL, baseTypeL);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x + 1 + offsetOutR, posY, wPos.z + offsetDirectionR, baseTypeR);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x + offsetDirectionF, posY, wPos.z - offsetOutF, baseTypeF);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x + offsetDirectionB, posY, wPos.z + 1 + offsetOutB, baseTypeB);

            offsetOutL += AddTreeBaseForOffsetOut(out baseTypeL);
            offsetOutR += AddTreeBaseForOffsetOut(out baseTypeR);
            offsetOutF += AddTreeBaseForOffsetOut(out baseTypeF);
            offsetOutB += AddTreeBaseForOffsetOut(out baseTypeB);
        }
    }

    protected int AddTreeBaseForOffsetOut(out BlockTypeEnum baseTypeOut)
    {
        int offsetDis = WorldRandTools.Range(0, 2);
        if (offsetDis == 0)
        {
            baseTypeOut = BlockTypeEnum.Dirt;
        }
        else
        {
            baseTypeOut = BlockTypeEnum.Grass;
        }
        return offsetDis;
    }

    protected void AddLotusLeaf(Vector3Int wPos)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(wPos, 411);
        if (addRate < 0.05)
        {
            int randomDiection = WorldRandTools.Range(11, 15);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(wPos.x, wPos.y, wPos.z, BlockTypeEnum.LotusLeaf, (BlockDirectionEnum)randomDiection);
        }
    }
}