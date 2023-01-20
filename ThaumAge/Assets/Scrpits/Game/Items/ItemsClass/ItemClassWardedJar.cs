using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassWardedJar : ItemTypeBlock
{

    /// <summary>
    /// 设置名字
    /// </summary>
    public override void SetItemName(Text tvTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        if (itemData != null && !itemData.meta.IsNull())
        {
            BlockMetaWardedJar blockMetaWarded = JsonUtil.FromJson<BlockMetaWardedJar>(itemData.meta);
            if (blockMetaWarded != null && blockMetaWarded.curElemental != 0 && blockMetaWarded.elementalType != 0)
            {
                string name;
                if (blockMetaWarded.elementalTypeForLabel != 0)
                {
                    ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData((ElementalTypeEnum)blockMetaWarded.elementalTypeForLabel);
                    //有标签
                    name = $"{itemsInfo.GetName()} {elementalInfo.GetName()}";
                    tvTarget.text = name;
                    return;
                }
                else
                {
                    //无标签
                    if (blockMetaWarded.curElemental != 0 && blockMetaWarded.elementalType != 0)
                    {
                        ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData((ElementalTypeEnum)blockMetaWarded.elementalType);
                        name = $"{itemsInfo.GetName()} {elementalInfo.GetName()}";
                        tvTarget.text = name;
                        return;
                    }
                }

            }
        }
        base.SetItemName(tvTarget, itemData, itemsInfo);
    }

    /// <summary>
    /// 设置道具图标
    /// </summary>
    /// <param name="ivTarget"></param>
    /// <param name="itemsInfo"></param>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        base.SetItemIcon(itemData, itemsInfo, ivTarget, srTarget);

        //如果没有东西 则不设置
        if (itemData == null || itemData.meta.IsNull())
        {
            return;
        }
        BlockMetaWardedJar blockMetaWardedJar = JsonUtil.FromJson<BlockMetaWardedJar>(itemData.meta);
        if (blockMetaWardedJar == null || blockMetaWardedJar.curElemental == 0)
        {
            return;
        }
        //如果有东西
        GameObject objIvSomething = null;
        if (ivTarget != null)
        {
            objIvSomething = ItemsHandler.Instance.Instantiate(ivTarget.gameObject, ivTarget.gameObject);
        }
        if (srTarget != null)
        {
            objIvSomething = ItemsHandler.Instance.Instantiate(srTarget.gameObject, srTarget.gameObject);
        }
        if (objIvSomething == null)
        {
            return;
        }
        objIvSomething.ShowObj(true);

        Image ivSomething = objIvSomething.GetComponent<Image>();
        SpriteRenderer srSomething = objIvSomething.GetComponent<SpriteRenderer>();

        ElementalTypeEnum elementalType = blockMetaWardedJar.GetElementalType();
        ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData(elementalType);
        ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out Color colorIcon);
        if (ivSomething != null)
        {
            ivSomething.color = colorIcon;
        }
        if (srSomething != null)
        {
            srSomething.color = colorIcon;
            srSomething.material.SetColor("_Color", colorIcon);
        }
        //设置图标
        string iconKeySomething = iconKeySomething = "icon_item_wardedjar";

        IconHandler.Instance.manager.GetItemsSpriteByName(iconKeySomething, (spIcon) =>
        {
            if (spIcon == null)
            {
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    if (ivSomething != null)
                    {
                        ivSomething.sprite = spIcon;
                    }
                    if (srSomething != null)
                    {
                        srSomething.sprite = spIcon;
                        srSomething.sortingOrder = 1;
                    }
                });
            }
            else
            {
                if (ivSomething != null)
                {
                    ivSomething.sprite = spIcon;
                }
                if (srSomething != null)
                {
                    srSomething.sprite = spIcon;
                    srSomething.sortingOrder = 1;
                }
            }
        });
    }

}