using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIPopupItemInfo : PopupShowView
{
    protected ItemsBean itemData;
    protected ItemsInfoBean itemsInfo;

    public override void Awake()
    {
        base.Awake();
        ui_ItemAttribute.ShowObj(false);
        ui_ItemElement.ShowObj(false);
    }

    public void SetData(ItemsBean itemData)
    {
        offsetPivot = new Vector2(0.1f, 0f);

        this.itemData = itemData;
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);

        SetItemDurability();
        SetItemIcon(itemData.itemId);
        SetItemIconColor(itemsInfo.GetItemsColor());
        SetItemName(itemsInfo.GetName());
        SetItemDetails("");
        SetAttribute(itemsInfo);
        SetElemental(itemsInfo);
    }

    /// <summary>
    /// 设置道具耐久
    /// </summary>
    public void SetItemDurability()
    {
        Item item = ItemsHandler.Instance.manager.GetRegisterItem(itemData.itemId, itemsInfo.GetItemsType());
        if (item is ItemBaseTool  itemTool)
        {
            ItemsMetaTool itemsMeta = itemData.GetMetaData<ItemsMetaTool>();
            ui_Durability.ShowObj(true);
            ui_ViewCommonPro.SetData(itemsMeta.curDurability, itemsMeta.durability);
            return;
        }
        ui_Durability.ShowObj(false);
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="iconSp"></param>
    public void SetItemIcon(long itemsId)
    {
        ItemsHandler.Instance.SetItemsIconById(ui_ItemIcon, itemsId);
    }

    /// <summary>
    /// 设置道具颜色
    /// </summary>
    /// <param name="colorStr"></param>
    public void SetItemIconColor(Color colorIcon)
    {
        ui_ItemIcon.color = colorIcon;
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

    /// <summary>
    /// 设置属性
    /// </summary>
    public void SetAttribute(ItemsInfoBean itemsInfo)
    {
        CptUtil.RemoveChildsByActive(ui_Attribute);
        AttributeBean attributeData = itemsInfo.GetAttributeData();
        if (attributeData == null || attributeData.dicAttributeData == null || attributeData.dicAttributeData.Count == 0)
        {
            ui_Attribute.ShowObj(false);
            return;
        }
        ui_Attribute.ShowObj(true);
        foreach (var itemAttribute in attributeData.dicAttributeData)
        {
            //如果是装备耐久 不在信息里显示
            if (itemAttribute.Key == AttributeTypeEnum.Durability)
                continue;
            GameObject objItem = Instantiate(ui_Attribute.gameObject, ui_ItemAttribute.gameObject);
            Text tvContent = objItem.transform.Find("AttributeText").GetComponent<Text>();
            Image ivIcon = objItem.transform.Find("AttributeIcon").GetComponent<Image>();

            string iconKey = AttributeBean.GetAttributeIconKey(itemAttribute.Key);
            string contentStr = AttributeBean.GetAttributeText(itemAttribute.Key);
            IconHandler.Instance.manager.GetUISpriteByName(iconKey, (sprite) =>
            {
                 ivIcon.sprite = sprite;
            });
            tvContent.text = $"{contentStr}{TextHandler.Instance.noBreakingSpace}{itemAttribute.Value}";
        }
    }

    public void SetElemental(ItemsInfoBean itemsInfo)
    {
        CptUtil.RemoveChildsByActive(ui_Elemental);
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        if (userData.characterData.GetAttributeValue(AttributeTypeEnum.ShowElemental) == 0)
        {
            ui_Elemental.ShowObj(false);
            return;
        }
        ui_Elemental.ShowObj(true);
        Dictionary<ElementalTypeEnum, int>  dicElemental = itemsInfo.GetAllElemental();
        foreach (var itemElemental in dicElemental)
        {
            ElementalInfoBean elementalInfo = GameInfoHandler.Instance.manager.GetElementalInfo(itemElemental.Key);
            GameObject objItem = Instantiate(ui_Elemental.gameObject, ui_ItemElement.gameObject);

            Text tvContent = objItem.transform.Find("ElementNum").GetComponent<Text>();
            Image ivIcon = objItem.transform.Find("ElementIcon").GetComponent<Image>();

            IconHandler.Instance.manager.GetUISpriteByName(elementalInfo.icon_key,(Sprite iconSp) =>
            {
                ColorUtility.TryParseHtmlString($"#{elementalInfo.color}", out Color ivColor);
                ivIcon.sprite = iconSp;
                ivIcon.color = ivColor;
            });

            tvContent.text = $"{itemElemental.Value}";
        }
    }
}