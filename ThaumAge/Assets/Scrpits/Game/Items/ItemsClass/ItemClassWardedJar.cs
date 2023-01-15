using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassWardedJar : ItemTypeBlock
{
    /// <summary>
    /// 设置道具图标
    /// </summary>
    /// <param name="ivTarget"></param>
    /// <param name="itemsInfo"></param>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        base.SetItemIcon(itemData, itemsInfo, ivTarget, srTarget);

        //如果没有东西 则不设置
        if (itemData.meta.IsNull())
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