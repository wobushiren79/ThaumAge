using UnityEditor;
using UnityEngine;

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
}