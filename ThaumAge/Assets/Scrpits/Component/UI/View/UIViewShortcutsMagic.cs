using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public partial class UIViewShortcutsMagic : BaseUIView
{
    protected List<UIViewMagicItem> listMagicItem = new List<UIViewMagicItem>();

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
        listMagicItem.Clear();
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
                int indexMagic = 0;
                for (int i = 0; i < itemMetaData.listMagicCore.Count; i++)
                {
                    ItemsBean itemDataMagicCore = itemMetaData.listMagicCore[i];

                    if (itemDataMagicCore.itemId == 0)
                        continue;
                    //获取法术核心数据
                    ItemMetaMagicCore itemMetaMagicCoreData = itemDataMagicCore.GetMetaData<ItemMetaMagicCore>();

                    ElementalTypeEnum elementalType = itemMetaMagicCoreData.GetElement();
                    //如果这个法术核心是空置的 则不显示法术
                    if (elementalType == ElementalTypeEnum.None)
                        continue;

                    GameObject objItem = Instantiate(gameObject, ui_ViewMagicItem.gameObject);
                    UIViewMagicItem maigcItem = objItem.GetComponent<UIViewMagicItem>();
                    maigcItem.SetData(itemDataMagicCore, indexMagic);
                    listMagicItem.Add(maigcItem);
                    indexMagic++;
                }
                if (userData.indexForShortcutsMagic >= listMagicItem.Count)
                {
                    userData.indexForShortcutsMagic = listMagicItem.Count - 1;
                }
                if (userData.indexForShortcutsMagic < 0)
                {
                    userData.indexForShortcutsMagic = 0;
                }
            }
            else
            {
                userData.indexForShortcutsMagic = 0;
            }
            this.TriggerEvent(EventsInfo.UIViewShortcutsMagic_ChangeSelect, userData.indexForShortcutsMagic);
        }
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);

        if (listMagicItem.IsNull())
            return;
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        int indexForShortcutsBefore = userData.indexForShortcutsMagic;
        int indexForShortcuts;
        switch (inputType)
        {
            case InputActionUIEnum.F1:
                indexForShortcuts = 0;
                break;
            case InputActionUIEnum.F2:
                indexForShortcuts = 1;
                break;
            case InputActionUIEnum.F3:
                indexForShortcuts = 2;
                break;
            case InputActionUIEnum.F4:
                indexForShortcuts = 3;
                break;
            case InputActionUIEnum.F5:
                indexForShortcuts = 4;
                break;
            case InputActionUIEnum.F6:
                indexForShortcuts = 5;
                break;
            case InputActionUIEnum.F7:
                indexForShortcuts = 6;
                break;
            case InputActionUIEnum.F8:
                indexForShortcuts = 7;
                break;
            default:
                indexForShortcuts = 0;
                break;
        }
        if (indexForShortcuts >= listMagicItem.Count)
        {
            indexForShortcuts = listMagicItem.Count - 1;
        }
        userData.indexForShortcutsMagic = indexForShortcuts;
        this.TriggerEvent(EventsInfo.UIViewShortcutsMagic_ChangeSelect, userData.indexForShortcutsMagic);
    }

}