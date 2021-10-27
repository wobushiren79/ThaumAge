using UnityEditor;
using UnityEngine;

public class Item
{
    public ItemsBean itemsData;

    public void SetItemData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
    }

    /// <summary>
    /// 使用
    /// </summary>
    public virtual void Use()
    {

    }

    /// <summary>
    /// 使用目标
    /// </summary>
    public virtual void UseTarget()
    {

    }
}