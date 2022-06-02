using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIBuildingEditorMain : BaseUIComponent
{
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Create)
        {
            OnClickForCreate();
        }
    }

    /// <summary>
    ///  点击创建
    /// </summary>
    public void OnClickForCreate()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIBuildingEditorCreate>(UIEnum.BuildingEditorCreate);
    }
}