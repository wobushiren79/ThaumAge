using UnityEditor;
using UnityEngine;

public partial class UIGameFurnacesSimple : UIGameCommonNormal
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
    public void SetFurnacesData(Vector3Int worldPosition)
    {
        ui_ViewFurnacesSimple.SetData(worldPosition);
    }
}