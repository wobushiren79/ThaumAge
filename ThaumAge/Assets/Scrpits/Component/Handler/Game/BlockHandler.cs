using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: Preserve]

public class BlockHandler : BaseHandler<BlockHandler, BlockManager>
{
    //破碎方块合集
    public Dictionary<Vector3Int, BlockCptBreak> dicBreakBlock = new Dictionary<Vector3Int, BlockCptBreak>();
    //闲置的破碎方块
    public Queue<BlockCptBreak> listBreakBlockIdle = new Queue<BlockCptBreak>();


    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="blockId"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public GameObject CreateBlockModel(Chunk chunk, ushort blockId, string modelName)
    {
        GameObject objModel = manager.GetBlockModel(blockId, modelName);
        if (objModel == null)
            return null;
        GameObject objBlock = Instantiate(chunk.chunkComponent.objBlockContainer, objModel);
        return objBlock;
    }

    /// <summary>
    /// 破坏方块
    /// </summary>
    /// <returns></returns>
    public BlockCptBreak BreakBlock(Vector3Int worldPosition, Block block, int damage)
    {
        if (dicBreakBlock.TryGetValue(worldPosition, out BlockCptBreak value))
        {
            value.Break(damage);
            return value;
        }
        else
        {
            BlockCptBreak blockCptBreak;

            if (listBreakBlockIdle.Count > 0)
            {
                blockCptBreak = listBreakBlockIdle.Dequeue();
                blockCptBreak.SetData(block, worldPosition);
                blockCptBreak.ShowObj(true);
                dicBreakBlock.Add(worldPosition, blockCptBreak);
            }
            else
            {
                //创建破碎效果
                GameObject objBlockBreak = Instantiate(gameObject, manager.blockBreakModel);
                blockCptBreak = objBlockBreak.GetComponent<BlockCptBreak>();
                blockCptBreak.SetData(block, worldPosition);
                dicBreakBlock.Add(worldPosition, blockCptBreak);
            }
            blockCptBreak.Break(damage);
            return blockCptBreak;
        }
    }

    /// <summary>
    /// 删除破碎效果
    /// </summary>
    public void DestroyBreakBlock(Vector3Int worldPosition)
    {
        if (dicBreakBlock.TryGetValue(worldPosition, out BlockCptBreak value))
        {
            value.ShowObj(false);
            value.SetBreakPro(0);
            dicBreakBlock.Remove(worldPosition);
            listBreakBlockIdle.Enqueue(value);
        }
    }

    /// <summary>
    /// 获取方块实例模型
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GameObject GetBlockObj(Vector3Int worldPosition)
    {
        Chunk chunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(worldPosition);
        return chunk.GetBlockObjForLocal(worldPosition - chunk.chunkData.positionForWorld);
    }

    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    //public Block CreateBlock(Chunk chunk, BlockTypeEnum blockType, Vector3Int localPosition, DirectionEnum direction)
    //{
    //    Type type = manager.GetRegisterBlock(blockType).GetType();
    //    Block block = FormatterServices.GetUninitializedObject(type) as Block;
    //    //Block block = CreateInstance<Block>(type);
    //    //Block block = Activator.CreateInstance(type) as Block;
    //    block.SetData(chunk, blockType, localPosition, direction);
    //    return block;
    //}


    /// <summary>
    /// 用于快速实例化方块 与il2cpp不兼容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objType"></param>
    /// <returns></returns>
    //public static T CreateInstance<T>(Type objType) where T : class
    //{
    //    Func<T> returnFunc;
    //    if (!DelegateStore<T>.Store.TryGetValue(objType.FullName, out returnFunc))
    //    {
    //        Func<T> a0l = Expression.Lambda<Func<T>>(Expression.New(objType)).Compile();
    //        DelegateStore<T>.Store[objType.FullName] = a0l;
    //        returnFunc = a0l;
    //    }
    //    return returnFunc();
    //}
    //internal static class DelegateStore<T>
    //{
    //    internal static IDictionary<string, Func<T>> Store = new ConcurrentDictionary<string, Func<T>>();
    //}
    public struct BlockData
    {
        public int blockId;
    }

    public Terrain3DCShaderNoiseLayers[] terrain3DCShaderNoises;

    /// <summary>
    /// 生成基础地形数据 
    /// </summary>
    public void CreateBaseBlockData(Chunk chunk, BiomeMapData biomeMapData,Action callBackForComplete)
    {
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        Biome biome = null;
        for (int x = 0; x < chunk.chunkData.chunkWidth; x++)
        {
            for (int z = 0; z < chunk.chunkData.chunkWidth; z++)
            {
                ChunkTerrainData itemTerrainData = biomeMapData.arrayChunkTerrainData[x * chunk.chunkData.chunkWidth + z];
                biome = BiomeHandler.Instance.manager.GetBiome(worldType, itemTerrainData.biomeIndex);
                for (int y = 0; y < chunk.chunkData.chunkHeight; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    //获取方块类型
                    BlockTypeEnum blockType = BiomeHandler.Instance.CreateBiomeBlockType(chunk, position, biome, itemTerrainData);
                    //如果是空 则跳过
                    if (blockType == BlockTypeEnum.None)
                        continue;

                    Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
                    //添加方块
                    chunk.chunkData.SetBlockForLocal(x, y, z, block, BlockDirectionEnum.UpForward);
                }
            }
        }
        //生成区块的对应方块（洞穴 大物体之类）
        BiomeHandler.Instance.CreateBiomeBlockTypeForChunk(chunk, biomeMapData, biome);
        callBackForComplete?.Invoke();
    }

    public void CreateBaseBlockDataForGPU(Chunk chunk, BiomeMapData biomeMapData, Action callBackForComplete)
    {
        if (terrain3DCShaderNoises == null)
        {
            terrain3DCShaderNoises = new Terrain3DCShaderNoiseLayers[1];
            Terrain3DCShaderNoiseLayers itemTemp = new Terrain3DCShaderNoiseLayers();
            itemTemp.frequency = 700;
            itemTemp.amplitude = 0.27f;
            itemTemp.lacunarity = 3.3f;
            itemTemp.octaves = 5;
            itemTemp.caveScale = 550;
            itemTemp.caveThreshold = 0.75f;
            itemTemp.caveFrequency = 2;
            itemTemp.caveAmplitude = 0.5f;
            itemTemp.caveOctaves = 4;
            itemTemp.groundMinHeigh = 40;
            terrain3DCShaderNoises[0] = itemTemp;

        }
        Terrain3DCShaderBean terrain3DCShaderBean = new Terrain3DCShaderBean();
        terrain3DCShaderBean.chunkPosition = chunk.chunkData.positionForWorld;
        terrain3DCShaderBean.chunkSizeW = chunk.chunkData.chunkWidth;
        terrain3DCShaderBean.chunkSizeH = chunk.chunkData.chunkHeight;
        terrain3DCShaderBean.stateCaves = 1;
        terrain3DCShaderBean.stateBedrock = 1;
        terrain3DCShaderBean.oceanHeight = 42;
        terrain3DCShaderBean.seed = 5;
        terrain3DCShaderBean.seedOffset = Vector3.zero;
        terrain3DCShaderBean.noiseLayers = terrain3DCShaderNoises;
        CShaderHandler.Instance.HandleTerrain3DCShader(terrain3DCShaderBean, (terrainData) =>
        {
            BlockData[] blockArray = new BlockData[terrain3DCShaderBean.GetBlockTotalNum()];
            terrainData.blockArrayBuffer.GetData(blockArray);

            uint[] count = new uint[1];
            terrainData.blockCountBuffer.GetData(count);
            for (int x = 0; x < chunk.chunkData.chunkWidth; x++)
            {
                for (int z = 0; z < chunk.chunkData.chunkWidth; z++)
                {
                    for (int y = 0; y < chunk.chunkData.chunkHeight; y++)
                    {
                        var itemData = blockArray[x + (y * terrainData.chunkSizeW) + (z * terrainData.chunkSizeW * terrainData.chunkSizeH)];
                        BlockTypeEnum blockType = (BlockTypeEnum)itemData.blockId;
                        //如果是空 则跳过
                        if (blockType == BlockTypeEnum.None)
                            continue;

                        Block block = manager.GetRegisterBlock(BlockTypeEnum.Dirt);
                        //添加方块
                        chunk.chunkData.SetBlockForLocal(x, y, z, block, BlockDirectionEnum.UpForward);
                    }
                }
            }
            callBackForComplete?.Invoke();
        });
    }


    /// <summary>
    /// 读取的方块
    /// </summary>
    public void LoadSaveBlockData(Chunk chunk)
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();

        chunk.chunkSaveData = gameDataManager.GetChunkSaveData(userData.userId, WorldCreateHandler.Instance.manager.worldType, chunk.chunkData.positionForWorld);
        //如果没有世界数据 则创建一个
        if (chunk.chunkSaveData == null)
        {
            chunk.chunkSaveData = new ChunkSaveBean();
            chunk.chunkSaveData.worldType = (int)WorldCreateHandler.Instance.manager.worldType;
            chunk.chunkSaveData.userId = userData.userId;
            chunk.chunkSaveData.position = chunk.chunkData.positionForWorld;
        }
        else
        {
            chunk.chunkSaveData.InitData();
            Dictionary<int, BlockBean> dicBlockData = chunk.chunkSaveData.dicBlockData;
            foreach (var itemData in dicBlockData)
            {
                BlockBean blockData = itemData.Value;
                Vector3Int positionBlock = blockData.localPosition;

                Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockData.blockId);
                chunk.chunkData.SetBlockForLocal(positionBlock, block, blockData.direction);
            }
        }
    }
}