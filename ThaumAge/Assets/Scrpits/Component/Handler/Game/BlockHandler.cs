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
    public List<string> listBreakBlock = new List<string>();

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
    public GameObject BreakBlock(Vector3Int worldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition,
            out BlockTypeEnum blockType,
            out DirectionEnum blockDirection,
            out Chunk chunk);
        return null;
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