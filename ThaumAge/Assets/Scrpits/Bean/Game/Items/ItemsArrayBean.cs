using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsArrayBean 
{
    public long[] itemIds;
    public int itemNumber;

    public ItemsArrayBean(long[] itemIds, int itemNumber)
    {
        this.itemIds = itemIds;
        this.itemNumber = itemNumber;
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="listDataStr"></param>
    /// <returns></returns>
    public static List<ItemsArrayBean> GetListItemsArrayBean(string listDataStr)
    {
        List<ItemsArrayBean> listData = new List<ItemsArrayBean>();
        string[] listItemsData = listDataStr.SplitForArrayStr('&');
        for (int i = 0; i < listItemsData.Length; i++)
        {
            string itemData1 = listItemsData[i];
            string[] itemData2 = itemData1.SplitForArrayStr(':');
            long[] itemIds = itemData2[0].SplitForArrayLong('|');

            if (itemData2.Length == 1)
            {
                listData.Add(new ItemsArrayBean(itemIds,1));
            }
            else
            {
                listData.Add(new ItemsArrayBean(itemIds, int.Parse(itemData2[1])));
            }
        }
        return listData;
    }
}