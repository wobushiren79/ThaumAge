using UnityEditor;
using UnityEngine;

public partial class UIGameMagicInstrumentAssembly : UIGameCommonNormal
{
    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
    }

}