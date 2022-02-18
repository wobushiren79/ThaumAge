using UnityEditor;
using UnityEngine;

public class BlockBoxBean : BlockBaseBean
{
    public ItemsBean[] items;

    public BlockBoxBean(int number)
    {
        items = new ItemsBean[number];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new ItemsBean();
        }
    }
}