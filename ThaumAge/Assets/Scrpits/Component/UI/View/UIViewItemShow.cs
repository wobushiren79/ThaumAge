using UnityEditor;
using UnityEngine;

public partial class UIViewItemShow : BaseUIView
{

    protected ItemsBean itemsData;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
        RefreshUI();
    }


    /// <summary>
    /// 设置图标
    /// </summary>
    public void SetIcon(ItemsInfoBean itemsInfo)
    {
        //ItemsHandler.Instance.SetItemsIconById(ui_IVIcon, itemsInfo.id, originalParent.itemsData);
    }
}