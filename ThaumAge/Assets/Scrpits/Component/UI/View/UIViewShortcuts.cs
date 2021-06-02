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

    public override void RefreshUI()
    {
        base.RefreshUI();
        InitShortcuts();
    }

    public void InitShortcuts()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ui_ShortcutItem_1.SetData(userData.GetItemsFromShortcuts(0));
        ui_ShortcutItem_2.SetData(userData.GetItemsFromShortcuts(1));
        ui_ShortcutItem_3.SetData(userData.GetItemsFromShortcuts(2));
        ui_ShortcutItem_4.SetData(userData.GetItemsFromShortcuts(3));
        ui_ShortcutItem_5.SetData(userData.GetItemsFromShortcuts(4));
        ui_ShortcutItem_6.SetData(userData.GetItemsFromShortcuts(5));
        ui_ShortcutItem_7.SetData(userData.GetItemsFromShortcuts(6));
        ui_ShortcutItem_8.SetData(userData.GetItemsFromShortcuts(7));
        ui_ShortcutItem_9.SetData(userData.GetItemsFromShortcuts(8));
        ui_ShortcutItem_10.SetData(userData.GetItemsFromShortcuts(9));
    }
}