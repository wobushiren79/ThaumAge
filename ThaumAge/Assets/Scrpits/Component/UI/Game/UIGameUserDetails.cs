using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameUserDetails : UIGameCommonNormal
{
    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();

    }
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);

    }

}