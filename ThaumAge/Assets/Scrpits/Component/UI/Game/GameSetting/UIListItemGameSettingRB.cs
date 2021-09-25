using System;
using UnityEditor;
using UnityEngine;

public partial class UIListItemGameSettingRB : UIListItemGameSettingBase, IRadioButtonCallBack
{
    protected Action<bool> callBack;

    public override void Awake()
    {
        base.Awake();
        ui_RB.SetCallBack(this);
    }

    public void SetData(string title, Action<bool> callBack)
    {
        this.callBack = callBack;
        SetTitle(title);
    }

    public void SetState(bool isOpen)
    {
        ui_RB.SetStates(isOpen);
    }

    #region 选择回掉
    public void RadioButtonSelected(RadioButtonView view, bool isSelect)
    {
        if (isSelect)
        {
            callBack?.Invoke(true);
            ui_RB.SetText(TextHandler.Instance.GetTextById(10001));
        }
        else
        {
            callBack?.Invoke(false);
            ui_RB.SetText(TextHandler.Instance.GetTextById(10002));
        }

    }
    #endregion
}