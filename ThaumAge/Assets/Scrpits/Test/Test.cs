using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    public Vector2Int imageDim;
    public int regionAmount;
    public bool drawByDistance = false;


    private void Start()
    {
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < 100000; i++)
        {
            Block block = Activator.CreateInstance(typeof(BlockCube)) as Block;
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < 100000; i++)
        {
            Block block = CreateInstance<Block>(typeof(BlockCube)) as Block;
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < 100000; i++)
        {
            BlockCube block = new BlockCube();
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < 100000; i++)
        {
            BlockCube block = (BlockCube)FormatterServices.GetUninitializedObject(typeof(BlockCube));
        }
        TimeUtil.GetMethodTimeEnd("4", stopwatch);

    }

    public static T CreateInstance<T>(Type objType) where T : class
    {
        Func<T> returnFunc;
        if (!DelegateStore<T>.Store.TryGetValue(objType.FullName, out returnFunc))
        {
            Func<T> a0l = Expression.Lambda<Func<T>>(Expression.New(objType)).Compile();
            DelegateStore<T>.Store[objType.FullName] = a0l;
            returnFunc = a0l;
            //var dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + objType.Name, objType, null, objType);
            //ILGenerator ilGen = dynMethod.GetILGenerator();
            //ilGen.Emit(OpCodes.Newobj, objType.GetConstructor(Type.EmptyTypes));
            //ilGen.Emit(OpCodes.Ret);
            //returnFunc = (Func<T>)dynMethod.CreateDelegate(typeof(Func<T>));
            //DelegateStore<T>.Store[objType.FullName] = returnFunc;
        }
        return returnFunc();
    }
    internal static class DelegateStore<T>
    {
        internal static IDictionary<string, Func<T>> Store = new ConcurrentDictionary<string, Func<T>>();
    }

}
