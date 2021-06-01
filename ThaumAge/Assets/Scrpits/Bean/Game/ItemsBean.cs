using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class ItemsBean 
{
    public long itemsId;
    public int number;
    public string meta;

    public T GetMetaData<T>()
    {
       T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public void SetMetaData<T>(T data)
    {
        meta = JsonUtil.ToJson<T>(data);
    }
}