using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIViewShortcuts : BaseUIView
{
    public UIViewItemContainer ui_ShortcutItem_1;
    public UIViewItemContainer ui_ShortcutItem_2;
    public UIViewItemContainer ui_ShortcutItem_3;
    public UIViewItemContainer ui_ShortcutItem_4;
    public UIViewItemContainer ui_ShortcutItem_5;
    public UIViewItemContainer ui_ShortcutItem_6;
    public UIViewItemContainer ui_ShortcutItem_7;
    public UIViewItemContainer ui_ShortcutItem_8;
    public UIViewItemContainer ui_ShortcutItem_9;
    public UIViewItemContainer ui_ShortcutItem_10;

    protected List<UIViewItemContainer> listShortcut;

    public override void Awake()
    {
        base.Awake();
        listShortcut = new List<UIViewItemContainer>();
        listShortcut.Add(ui_ShortcutItem_1);
        listShortcut.Add(ui_ShortcutItem_2);
        listShortcut.Add(ui_ShortcutItem_3);
        listShortcut.Add(ui_ShortcutItem_4);
        listShortcut.Add(ui_ShortcutItem_5);
        listShortcut.Add(ui_ShortcutItem_6);
        listShortcut.Add(ui_ShortcutItem_7);
        listShortcut.Add(ui_ShortcutItem_8);
        listShortcut.Add(ui_ShortcutItem_9);
        listShortcut.Add(ui_ShortcutItem_10);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        InitShortcuts();
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        int indexForShortcutsBefore = userData.indexForShortcuts;
        int indexForShortcuts;
        bool isRefreshUI = true;
        base.OnInputActionForStarted(inputType);
        switch (inputType)
        {
            case InputActionUIEnum.N0:
                indexForShortcuts = 9;
                break;
            case InputActionUIEnum.N1:
                indexForShortcuts = 0;
                break;
            case InputActionUIEnum.N2:
                indexForShortcuts = 1;
                break;
            case InputActionUIEnum.N3:
                indexForShortcuts = 2;
                break;
            case InputActionUIEnum.N4:
                indexForShortcuts = 3;
                break;
            case InputActionUIEnum.N5:
                indexForShortcuts = 4;
                break;
            case InputActionUIEnum.N6:
                indexForShortcuts = 5;
                break;
            case InputActionUIEnum.N7:
                indexForShortcuts = 6;
                break;
            case InputActionUIEnum.N8:
                indexForShortcuts = 7;
                break;
            case InputActionUIEnum.N9:
                indexForShortcuts = 8;
                break;
            case InputActionUIEnum.NAdd:
                indexForShortcuts = userData.indexForShortcuts + 1;
                break;
            case InputActionUIEnum.NSub:
                indexForShortcuts = userData.indexForShortcuts - 1;
                break;
            default:
                return;
        }
        //如果没有改变 则不处理
        if (indexForShortcutsBefore == indexForShortcuts)
        {
            return;
        }
        if (indexForShortcuts > 9)
        {
            indexForShortcuts = 0;
        }
        else if (indexForShortcuts < 0)
        {
            indexForShortcuts = 9;
        }
        userData.SetShortcuts(indexForShortcuts);
        if (isRefreshUI)
        {
            RefreshUI();
        }
    }


    /// <summary>
    /// 初始化状态栏
    /// </summary>
    public void InitShortcuts()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        for (int i = 0; i < listShortcut.Count; i++)
        {
            listShortcut[i].SetData(userData.GetItemsFromShortcut(i), i);
        }

        for (int i = 0; i < listShortcut.Count; i++)
        {
            UIViewItemContainer uiViewItem = listShortcut[i];
            if (uiViewItem.viewIndex == userData.indexForShortcuts)
            {
                //如果选中的是当前状态栏
                uiViewItem.SetSelectState(true);
            }
            else
            {
                //如果未选中当前状态栏
                uiViewItem.SetSelectState(false);
            }
        }
    }

}