using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIMainMaker : BaseUIComponent
{
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Back)
        {
            HandleForBack();
        }
    }

    public void HandleForBack()
    {
        UIMainStart uiMainStart = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
    }
}