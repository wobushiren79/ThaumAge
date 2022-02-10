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
        GameObject objBlock = Instantiate(chunk.objBlockContainer, objModel);
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
            BlockCptBreak BlockCptBreak;

            if (listBreakBlockIdle.Count > 0)
            {
                BlockCptBreak = listBreakBlockIdle.Dequeue();
                BlockCptBreak.SetData(block, worldPosition);
                BlockCptBreak.ShowObj(true);
                dicBreakBlock.Add(worldPosition, BlockCptBreak);
            }
            else
            {
                //创建破碎效果
                GameObject objBlockBreak = Instantiate(gameObject, manager.blockBreakModel);
                BlockCptBreak = objBlockBreak.GetComponent<BlockCptBreak>();
                BlockCptBreak.SetData(block, worldPosition);
                dicBreakBlock.Add(worldPosition, BlockCptBreak);
            }
            BlockCptBreak.Break(damage);
            return BlockCptBreak;
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

}