using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameCommonNormal : BaseUIComponent
{
    public Button ui_Exit;

    public override void OpenUI()
    {
        base.OpenUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(false);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Exit)
        {
            HandleForBackMain();
        }
    }

    /// <summary>
    /// 处理返回主界面
    /// </summary>
    public virtual void HandleForBackMain() 
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }
}
