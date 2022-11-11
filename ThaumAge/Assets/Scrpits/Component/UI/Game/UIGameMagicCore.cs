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