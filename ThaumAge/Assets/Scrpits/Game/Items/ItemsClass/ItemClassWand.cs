using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassWand : Item
{
    /// <summary>
    /// 设置道具图标
    /// </summary>
    public override void SetItemIcon(Image ivTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        //设置杖柄
        if (itemMetaBuckets.rodId != 0)
        {
            //设置图标
            ItemsInfoBean itemsInfoForRod = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.rodId);
            SetItemIcon(ivTarget, itemsInfoForRod);
        }
        //设置杖端
        if (itemMetaBuckets.capId != 0)
        {
            GameObject objIvSomething = ItemsHandler.Instance.Instantiate(ivTarget.gameObject, ivTarget.gameObject);
            objIvSomething.ShowObj(true);
            Image ivSomething = objIvSomething.GetComponent<Image>();

            //设置图标
            ItemsInfoBean itemsInfoForCap = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
            IconHandler.Instance.manager.GetItemsSpriteByName($"{itemsInfoForCap.icon_key}_1", (spIcon) =>
            {
                if (spIcon == null)
                {
                    IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                    {
                        if (ivSomething != null)
                        {
                            ivSomething.sprite = spIcon;
                        }
                    });
                }
                if (ivSomething != null)
                {
                    ivSomething.sprite = spIcon;
                }
            });
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    public override void SetItemName(Text tvTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        ItemsInfoBean capInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
        ItemsInfoBean rodInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.rodId);
        string name = $"{capInfo.GetName()}{rodInfo.GetName()}{itemsInfo.GetName()}";
        tvTarget.text = name;
    }


    public override void GetItemIconTex(ItemsBean itemData, ItemsInfoBean itemsInfo, Action<Texture2D> callBack)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        ItemsHandler.Instance.manager.GetItemsIconById(itemMetaBuckets.rodId, (rodSprite) =>
        {            
            //设置图标
            ItemsInfoBean itemsInfoForCap = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
            IconHandler.Instance.manager.GetItemsSpriteByName($"{itemsInfoForCap.icon_key}_1", (capSprite) =>
            {
                if (capSprite != null)
                {
                    Texture2D itemTex = TextureUtil.SpriteToTexture2D(new Sprite[] { rodSprite, capSprite });
                    callBack?.Invoke(itemTex);
                }
                else
                {
                    Debug.LogError($"没有找到icon_key为 {itemsInfoForCap.icon_key}_1 的sprite");
                }
            });
        });
    }
}