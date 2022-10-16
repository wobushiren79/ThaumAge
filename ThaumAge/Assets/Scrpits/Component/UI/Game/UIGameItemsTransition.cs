using UnityEditor;
using UnityEngine;

public partial class UIGameItemsTransition : UIGameCommonNormal
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
    public void SetData(Vector3Int worldPosition)
    {
        ui_ViewItemsTransition.SetData(worldPosition);
    }
}