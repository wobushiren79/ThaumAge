using UnityEditor;
using UnityEngine;

public partial class UIGameMagicCore : UIGameCommonNormal
{
    protected ItemsBean itemData;

    public override void OpenUI()
    {
        base.OpenUI();
        UIViewShortcuts.CanChangeItem = false;
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ViewShortcutsMagic.OpenUI();
        ui_ViewMagicCoreExchange.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        UIViewShortcuts.CanChangeItem = true;

        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewShortcutsMagic.CloseUI();
        ui_ViewMagicCoreExchange.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
        ui_ViewShortcutsMagic.RefreshUI();
        ui_ViewMagicCoreExchange.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(ItemsBean itemData)
    {
        this.itemData = itemData;
        ui_ViewMagicCoreExchange.SetData(itemData);
    }

}