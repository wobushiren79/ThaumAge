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

    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public Block CreateBlock(Chunk chunk, Vector3Int position, BlockTypeEnum blockType)
    {
        //设置数据
        BlockBean blockData = new BlockBean(blockType, position, position + chunk.worldPosition);
        return CreateBlock(chunk, blockData);
    }

    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    public Block CreateBlock(Chunk chunk, BlockBean blockData)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        Type type = manager.GetRegisterBlock(blockType).GetType();

        Block block = FormatterServices.GetUninitializedObject(type) as Block;

        //Block block = CreateInstance<Block>(type);

        //Block block = Activator.CreateInstance(type) as Block;

        block.SetData(chunk, blockData.localPosition.GetVector3Int(), blockData);
        return block;
    }

    /// <summary>
    /// 用于快速实例化方块 与il2cpp不兼容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objType"></param>
    /// <returns></returns>
    public static T CreateInstance<T>(Type objType) where T : class
    {
        Func<T> returnFunc;
        if (!DelegateStore<T>.Store.TryGetValue(objType.FullName, out returnFunc))
        {
            Func<T> a0l = Expression.Lambda<Func<T>>(Expression.New(objType)).Compile();
            DelegateStore<T>.Store[objType.FullName] = a0l;
            returnFunc = a0l;
        }
        return returnFunc();
    }
    internal static class DelegateStore<T>
    {
        internal static IDictionary<string, Func<T>> Store = new ConcurrentDictionary<string, Func<T>>();
    }

}