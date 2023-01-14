using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaBox : BlockMetaBase
{
    public ItemsBean[] items;

    public BlockMetaBox(int number)
    {
        items = new ItemsBean[number];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new ItemsBean();
        }
    }

    public BlockMetaBox(int number, ItemsBean[] listData)
    {
        items = new ItemsBean[number];
        for (int i = 0; i < items.Length; i++)
        {
            if (i < listData.Length)
            {
                items[i] = listData[i];
            }
            else
            {
                items[i] = new ItemsBean();
            }
        }
    }
}