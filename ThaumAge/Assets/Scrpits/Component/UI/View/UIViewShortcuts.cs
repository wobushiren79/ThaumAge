﻿using System;
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

    public List<UIViewItemContainer> listShortcut;

    //是否能改变道具
    public static bool CanChangeItem = true;

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
        //设置回调
        for(int i = 0; i < listShortcut.Count; i++)
        {
            listShortcut[i].SetCallBackForSetViewItem(actionForItemChanged);
        }
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        InitShortcuts();
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);

        if (!CanChangeItem)
            return;
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        int indexForShortcutsBefore = userData.indexForShortcuts;
        int indexForShortcuts;

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
                indexForShortcuts = indexForShortcutsBefore + 1;
                break;
            case InputActionUIEnum.NSub:
                indexForShortcuts = indexForShortcutsBefore - 1;
                break;
            default:
                return;
        }
        userData.SetShortcuts(indexForShortcuts);
        GameHandler.Instance.manager.player.RefreshHandItem();
        InitSelect();
        //事件通知
        EventHandler.Instance.TriggerEvent(EventsInfo.UIViewShortcuts_ChangeSelect, indexForShortcuts);
    }


    /// <summary>
    /// 初始化状态栏
    /// </summary>
    public void InitShortcuts()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        for (int i = 0; i < listShortcut.Count; i++)
        {
            listShortcut[i].SetViewItemByData(UIViewItemContainer.ContainerType.Shortcuts, userData.GetItemsFromShortcut(i), i);
        }
        InitSelect();
    }

    public void InitSelect()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
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


    /// <summary>
    /// 道具改变回调
    /// </summary>
    /// <param name="callBackForSetViewItem"></param>
    Action<UIViewItemContainer, ItemsBean> actionForItemChanged = (container, itemsData) =>
    {
        //如果是快捷栏
        if (container.containerType == UIViewItemContainer.ContainerType.Shortcuts)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            //如果当前快捷栏等于改变的道具栏 所以刷新手上道具
            if (userData.indexForShortcuts == container.viewIndex)
            {
                GameHandler.Instance.manager.player.RefreshHandItem();
            }      
        }
    };
}