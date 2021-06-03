using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICommonNormal : BaseUIComponent
{
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
