using UnityEditor;
using UnityEngine;

public partial class UIGameFurnaces : UIGameCommonNormal
{
    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
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