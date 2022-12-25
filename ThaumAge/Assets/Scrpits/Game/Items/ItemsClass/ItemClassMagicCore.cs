using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassMagicCore : Item
{
    /// <summary>
    /// 设置图标
    /// </summary>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        base.SetItemIcon(itemData, itemsInfo, ivTarget, srTarget);

        Color colorIcon = GetItemIconColor(itemData, itemsInfo);

        if (ivTarget != null)
        {
            ivTarget.color = colorIcon;
        }
        if (srTarget != null)
        {
            srTarget.color = colorIcon;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    public override void SetItemName(Text tvTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        ItemMetaMagicCore itemMagicCore = itemData.GetMetaData<ItemMetaMagicCore>();
        //获取法术核心元素类型
        ElementalTypeEnum elementalType = itemMagicCore.GetElement();
        var elementalInfo = ElementalInfoCfg.GetItemData(elementalType);
        string elementalName;
        if (elementalType == ElementalTypeEnum.None)
        {
            elementalName = TextHandler.Instance.GetTextById(9001);
        }
        else
        {
            elementalName = elementalInfo.GetName();
        }
        string name = $"{itemsInfo.GetName()} {elementalName}";
        tvTarget.text = name;
    }

    /// <summary>
    /// 获取道具颜色
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemsInfo"></param>
    /// <returns></returns>
    public override Color GetItemIconColor(ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        ItemMetaMagicCore itemMagicCore = itemData.GetMetaData<ItemMetaMagicCore>();
        //获取法术核心元素类型
        ElementalTypeEnum elementalType = itemMagicCore.GetElement();
        Color colorIcon = Color.white;
        if (elementalType != ElementalTypeEnum.None)
        {
            var elementalInfo = ElementalInfoCfg.GetItemData(elementalType);
            ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out colorIcon);
        }
        return colorIcon;
    }
}