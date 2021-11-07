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
    public Dictionary<Vector3Int, BlockBreak> dicBreakBlock = new Dictionary<Vector3Int, BlockBreak>();
    //闲置的破碎方块
    public Queue<BlockBreak> listBreakBlockIdle = new Queue<BlockBreak>();

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
    public BlockBreak BreakBlock(Vector3Int worldPosition, Block block, int damage)
    {
        if (dicBreakBlock.TryGetValue(worldPosition, out BlockBreak value))
        {
            value.Break(damage);
            return value;
        }
        else
        {
            BlockBreak blockBreak;

            if (listBreakBlockIdle.Count > 0)
            {
                blockBreak = listBreakBlockIdle.Dequeue();
                blockBreak.SetData(block, worldPosition);
                blockBreak.ShowObj(true);
                dicBreakBlock.Add(worldPosition, blockBreak);
            }
            else
            {
                //创建破碎效果
                GameObject objBlockBreak = Instantiate(gameObject, manager.blockBreakModel);
                blockBreak = objBlockBreak.GetComponent<BlockBreak>();
                blockBreak.SetData(block, worldPosition);
                dicBreakBlock.Add(worldPosition, blockBreak);
            }
            blockBreak.Break(damage);
            return blockBreak;
        }
    }

    /// <summary>
    /// 删除破碎效果
    /// </summary>
    public void DestroyBreakBlock(Vector3Int worldPosition)
    {
        if (dicBreakBlock.TryGetValue(worldPosition, out BlockBreak value))
        {
            value.ShowObj(false);
            value.SetBreakPro(0);
            dicBreakBlock.Remove(worldPosition);
            listBreakBlockIdle.Enqueue(value);
        }
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