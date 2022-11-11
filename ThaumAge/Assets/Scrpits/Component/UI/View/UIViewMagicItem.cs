using UnityEditor;
using UnityEngine;

public partial class UIViewMagicItem : BaseUIView
{

    protected ItemsBean itemData;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemData)
    {
        this.itemData = itemData;
    }
}