using UnityEditor;
using UnityEngine;

public partial class UIPopupItemInfo : PopupShowView
{
    public long itemId;
    public ItemsInfoBean ItemsInfo;

    public void SetData(long itemId)
    {
        offsetPivot = new Vector2(0.1f, 0f);

        this.itemId = itemId;
        ItemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        Sprite itemIcon = ItemsHandler.Instance.manager.GetItemsIconById(itemId);
        SetItemIcon(itemIcon);
        SetItemName(ItemsInfo.name);
        SetItemDetails("");
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="iconSp"></param>
    public void SetItemIcon(Sprite iconSp)
    {
        if (iconSp == null)
        {
            ui_ItemIcon.sprite = IconHandler.Instance.GetUnKnowSprite();
        }
        else
        {
            ui_ItemIcon.sprite = iconSp;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="itemName"></param>
    public void SetItemName(string itemName)
    {
        ui_ItemName.text = itemName;
    }

    /// <summary>
    /// 设置描述
    /// </summary>
    /// <param name="itemDetails"></param>
    public void SetItemDetails(string itemDetails)
    {
        if (itemDetails.IsNull())
        {
            ui_Details.gameObject.SetActive(false);
        }
        else
        {
            ui_Details.gameObject.SetActive(true);
            ui_ItemDetails.text = itemDetails;
        }
    }

}