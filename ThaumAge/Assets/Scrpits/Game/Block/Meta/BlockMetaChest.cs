using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaChest : BlockMetaBase
{
    public ItemsBean[] items;

    public BlockMetaChest()
    {
        items = new ItemsBean[0];
    }

    public BlockMetaChest(int number)
    {
        items = new ItemsBean[number];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new ItemsBean();
        }
    }

    public BlockMetaChest(int number, ItemsBean[] listData)
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