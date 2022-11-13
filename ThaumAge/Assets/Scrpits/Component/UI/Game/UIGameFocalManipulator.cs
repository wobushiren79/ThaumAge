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

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition)
    {
        ui_ViewFocalManipulator.SetData(worldPosition);
    }
}