using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

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

    public ItemsBean(ItemsBean otherItem)
    {
        this.itemId = otherItem.itemId;
        this.number = otherItem.number;
        this.meta = otherItem.meta;
    }

    public void ClearData()
    {
        this.itemId = 0;
        this.number = 0;
        this.meta = null;
    }

    public T GetMetaData<T>() where T : ItemBaseMeta
    {        
        //如果meta数据是null的 则按items的类型赋予不同的数据
        if (meta.IsNull()||meta.Equals("{}"))
        {
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
            Item item = ItemsHandler.Instance.manager.GetRegisterItem(itemsInfo.id,itemsInfo.GetItemsType());
            //获取不同道具的初始化meta数据
            ItemBaseMeta itemsDetails = item.GetInitMetaData<T>(itemId);

            meta =  SetMetaData(itemsDetails);
            return itemsDetails as T;
        }
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="listDataStr"></param>
    /// <returns></returns>
    public static List<ItemsBean> GetListItemsArrayBean(string listDataStr)
    {
        List<ItemsBean> listData = new List<ItemsBean>();
        string[] listItemsData = listDataStr.SplitForArrayStr('&');
        for (int i = 0; i < listItemsData.Length; i++)
        {
            string itemData1 = listItemsData[i];
            string[] itemData2 = itemData1.SplitForArrayStr(':');
            long itemIds = long.Parse(itemData2[0]);

            if (itemData2.Length == 1)
            {
                listData.Add(new ItemsBean(itemIds, 1));
            }
            else
            {
                listData.Add(new ItemsBean(itemIds, int.Parse(itemData2[1])));
            }
        }
        return listData;
    }


    public static T GetMetaData<T>(string meta) where T : ItemBaseMeta
    {
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public string SetMetaData<T>(T data) where T : ItemBaseMeta
    {
        meta = JsonUtil.ToJson<T>(data);
        return meta;
    }
}