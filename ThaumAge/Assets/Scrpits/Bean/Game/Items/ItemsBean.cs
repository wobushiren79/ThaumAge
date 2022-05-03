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

    public T GetMetaData<T>() where T : ItemsDetailsBean
    {        
        //如果meta数据是null的 则按items的类型赋予不同的数据
        if (meta.IsNull()||meta.Equals("{}"))
        {
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
            Item item = ItemsHandler.Instance.manager.GetRegisterItem(itemsInfo.id,itemsInfo.GetItemsType());
            ItemsDetailsBean itemsDetails = item.GetItemsDetailsBean(itemId);
            meta =  SetMetaData(itemsDetails);
            return itemsDetails as T;
        }
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public static T GetMetaData<T>(string meta) where T : ItemsDetailsBean
    {
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public string SetMetaData<T>(T data) where T : ItemsDetailsBean
    {
        meta = JsonUtil.ToJson<T>(data);
        return meta;
    }
}