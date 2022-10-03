using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class UIPopupItemInfoButton : PopupButtonView<UIPopupItemInfo>
{
    public ItemsBean itemData;
    public override void Awake()
    {
        base.Awake();
        popupType = PopupEnum.ItemInfo;
    }

    /// <summary>
    /// 设置道具ID
    /// </summary>
    /// <param name="itemId"></param>
    public void SetItemData(ItemsBean itemData)
    {
        this.itemData = itemData;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.itemData == null || this.itemData.itemId == 0)
            return;
        base.OnPointerEnter(eventData);
    }

    public override void PopupHide()
    {

    }

    public override void PopupShow()
    {
        popupShow.SetData(itemData);
    }

}