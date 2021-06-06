using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommonNormal : BaseUIComponent
{
    public Button ui_Exit;

    public override void Awake()
    {
        base.Awake();
        ui_Exit.onClick.AddListener(OnClickForExit);
    }

    public virtual void OnClickForExit()
    {
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

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
}
