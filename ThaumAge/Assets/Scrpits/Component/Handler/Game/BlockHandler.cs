using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Scripting;
using static UnityEngine.Animations.AimConstraint;
using UnityEngine.UIElements;

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
        //方块ID
        public int blockId;
        //方块结构
        public int blockBuilding;
    }

    public Terrain3DCShaderNoiseLayer[] terrain3DCShaderNoises;

    /// <summary>
    /// 生成地形数据
    /// </summary>
    public void CreateBaseBlockDataForGPU(Chunk chunk, Action<BlockData[]> callBackForComplete)
    {
        BiomeManager biomeManager = BiomeHandler.Instance.manager;
        Biome biome = biomeManager.GetBiome(chunk.chunkData.biomeType);

        Biome biomeL = biomeManager.GetBiome(chunk.chunkData.biomeTypeL);
        Biome biomeR = biomeManager.GetBiome(chunk.chunkData.biomeTypeR);
        Biome biomeF = biomeManager.GetBiome(chunk.chunkData.biomeTypeF);
        Biome biomeB = biomeManager.GetBiome(chunk.chunkData.biomeTypeB);

        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        Terrain3DCShaderBean terrain3DCShaderBean = new Terrain3DCShaderBean();
        terrain3DCShaderBean.chunkPosition = chunk.chunkData.positionForWorld;
        terrain3DCShaderBean.chunkSizeW = chunk.chunkData.chunkWidth;
        terrain3DCShaderBean.chunkSizeH = chunk.chunkData.chunkHeight;
        terrain3DCShaderBean.stateCaves = 1;
        terrain3DCShaderBean.stateBedrock = 1;
        terrain3DCShaderBean.seed = worldSeed;
        terrain3DCShaderBean.seedOffset = Vector3.zero;
        terrain3DCShaderBean.noiseLayers = new Terrain3DCShaderNoiseLayer[] 
        { 
            biome.terrain3DCShaderNoise,
            biomeL.terrain3DCShaderNoise,
            biomeR.terrain3DCShaderNoise,
            biomeF.terrain3DCShaderNoise,
            biomeB.terrain3DCShaderNoise
        };
        terrain3DCShaderBean.oreDatas = biome.terrain3DCShaderOre;
        CShaderHandler.Instance.HandleTerrain3DCShader(terrain3DCShaderBean, (terrainData) =>
        {
            BlockData[] blockArray = new BlockData[terrain3DCShaderBean.GetBlockTotalNum()];
            terrainData.blockArrayBuffer.GetData(blockArray);

            uint[] count = new uint[1];
            terrainData.blockCountBuffer.GetData(count);

            callBackForComplete?.Invoke(blockArray);
        });
    }

    /// <summary>
    /// 处理生成的地形数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="blockArray"></param>
    public void HandleBaseBlockData(Chunk chunk, BlockData[] blockArray)
    {
        int chunkWidth = chunk.chunkData.chunkWidth;
        int chunkHeight = chunk.chunkData.chunkHeight;
        for (int x = chunk.chunkData.chunkWidth - 1; x >= 0; x--)
        {
            for (int z = chunk.chunkData.chunkWidth - 1; z >= 0; z--)
            {
                for (int y = chunk.chunkData.chunkHeight - 1; y >= 0; y--)
                {
                    var itemData = blockArray[x + (y * chunkWidth) + (z * chunkWidth * chunkHeight)];
                    //如果是空 则跳过
                    if (itemData.blockId == 0)
                        continue;
                    Block block = manager.GetRegisterBlock(itemData.blockId);
                    //添加方块
                    chunk.chunkData.SetBlockForLocal(x, y, z, block, BlockDirectionEnum.UpForward);
                    //创建特殊结构方框的数据
                    if (itemData.blockBuilding != 0)
                    {
                        Biome biome = BiomeHandler.Instance.manager.GetBiome(chunk.chunkData.biomeType);
                        biome.CreateBlockBuilding(chunk, itemData.blockId, itemData.blockBuilding, chunk.chunkData.positionForWorld + new Vector3Int(x, y, z));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 读取的方块
    /// </summary>
    public void LoadSaveBlockData(Chunk chunk)
    {
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        //获取数据中的chunk
        UserDataBean userData = gameDataManager.GetUserData();
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        //如果是登录界面不需要读取数据
        if (worldType == WorldTypeEnum.Launch)
        {
            return;
        }
        chunk.chunkSaveData = gameDataManager.GetChunkSaveData(userData.userId, worldType, chunk.chunkData.positionForWorld);
        //如果没有世界数据 则创建一个
        if (chunk.chunkSaveData == null)
        {
            chunk.chunkSaveData = new ChunkSaveBean();
            chunk.chunkSaveData.worldType = (int)worldType;
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