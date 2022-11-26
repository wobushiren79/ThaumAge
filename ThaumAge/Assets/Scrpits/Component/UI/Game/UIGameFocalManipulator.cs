using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameFocalManipulator : UIGameCommonNormal
{

    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ViewFocalManipulator.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewFocalManipulator.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
        ui_ViewFocalManipulator.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition)
    {
        ui_ViewFocalManipulator.SetData(worldPosition);
    }
}