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

    /// <summary>
    /// 初始化状态栏
    /// </summary>
    public void InitShortcuts()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        ui_ShortcutItem_1.SetData(userData.GetItemsFromShortcut(0), new Vector2Int(0, 0));
        ui_ShortcutItem_2.SetData(userData.GetItemsFromShortcut(1), new Vector2Int(1, 0));
        ui_ShortcutItem_3.SetData(userData.GetItemsFromShortcut(2), new Vector2Int(2, 0));
        ui_ShortcutItem_4.SetData(userData.GetItemsFromShortcut(3), new Vector2Int(3, 0));
        ui_ShortcutItem_5.SetData(userData.GetItemsFromShortcut(4), new Vector2Int(4, 0));
        ui_ShortcutItem_6.SetData(userData.GetItemsFromShortcut(5), new Vector2Int(5, 0));
        ui_ShortcutItem_7.SetData(userData.GetItemsFromShortcut(6), new Vector2Int(6, 0));
        ui_ShortcutItem_8.SetData(userData.GetItemsFromShortcut(7), new Vector2Int(7, 0));
        ui_ShortcutItem_9.SetData(userData.GetItemsFromShortcut(8), new Vector2Int(8, 0));
        ui_ShortcutItem_10.SetData(userData.GetItemsFromShortcut(9), new Vector2Int(9, 0));

        for (int i = 0; i < listShortcut.Count; i++)
        {
            UIViewItemContainer uiViewItem = listShortcut[i];
            if (uiViewItem.viewIndex.x == userData.indexForShortcuts)
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