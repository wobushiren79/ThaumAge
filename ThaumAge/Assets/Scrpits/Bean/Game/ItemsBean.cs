using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class ItemsBean 
{
    public long itemId;
    public int number;
    public string meta;

    public ItemsBean()
    {

    }

    public ItemsBean(long itemId)
    {
        this.itemId = itemId;
    }

    public ItemsBean(long itemId, int number)
    {
        this.itemId = itemId;
        this.number = number;
    }

    public ItemsBean(long itemId, int number,string meta)
    {
        this.itemId = itemId;
        this.number = number;
        this.meta = meta;
    }

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