using UnityEditor;
using UnityEngine;

public partial class UIGameFurnaces : UIGameCommonNormal
{
    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ViewFurnaces.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewFurnaces.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI(isOpenInit);
        ui_ViewBackPack.RefreshUI(isOpenInit);
        ui_ViewFurnaces.RefreshUI(isOpenInit);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockData"></param>
    public void SetData(Vector3Int worldPosition)
    {
        ui_ViewFurnaces.SetData(worldPosition);
    }
}