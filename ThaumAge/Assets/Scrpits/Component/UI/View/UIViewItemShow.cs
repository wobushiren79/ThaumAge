using UnityEditor;
using UnityEngine;

public partial class UIViewItemShow : BaseUIView
{
    protected ItemsBean itemsData;
    protected UIPopupItemInfoButton infoButton;

    public override void Awake()
    {
        base.Awake();
        infoButton = GetComponent<UIPopupItemInfoButton>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
        infoButton.SetItemData(itemsData);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        SetIcon(itemsData);
        SetNumber(itemsData.number);
    }


    /// <summary>
    /// 设置图标
    /// </summary>
    public void SetIcon(ItemsBean itemsData)
    {
        ItemsHandler.Instance.SetItemsIconById(ui_IVIcon, itemsData.itemId, itemsData);
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    public void SetNumber(int number)
    {
        ui_TVNumber.text = $"{number}";
    }
}