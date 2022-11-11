using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public partial class UIViewShortcutsMagic : BaseUIView
{
    public override void Awake()
    {
        base.Awake();
        ui_ViewMagicItem.ShowObj(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        this.RegisterEvent(EventsInfo.UIViewShortcutsMagic_InitData, InitData);
        InitData();
    }


    public void InitData()
    {
        //首先删除所有老数据
        rectTransform.transform.DestroyAllChild(true);

        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
        if (itemsData.itemId == 0)
            return;
        //如果是法杖
        if (itemsInfo.GetItemsType() == ItemsTypeEnum.Wand)
        {
            ItemMetaWand itemMetaData = itemsData.GetMetaData<ItemMetaWand>();
            if (!itemMetaData.listMagicCore.IsNull())
            {
                for (int i = 0; i < itemMetaData.listMagicCore.Count; i++)
                {
                    ItemsBean itemDataMagicCore= itemMetaData.listMagicCore[i];
                    if (itemDataMagicCore.itemId == 0)
                        continue;
                    GameObject objItem = Instantiate(gameObject, ui_ViewMagicItem.gameObject);
                    UIViewMagicItem maigcItem = objItem.GetComponent<UIViewMagicItem>();
                    maigcItem.SetData(itemMetaData.listMagicCore[i]);
                }
            }
        }
    }

    /// <summary>
    /// 创建魔法
    /// </summary>
    public void CreateMagicItem(List<ItemsBean> listMagicCore)
    {

    }
}