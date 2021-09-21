using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIListItemGameSettingRange : UIListItemGameSettingBase, ProgressView.ICallBack
{
    //改变回调
    protected Action<float> callBack;

    public override void Awake()
    {
        base.Awake();
        ui_ProContent.SetCallBack(this);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="title"></param>
    /// <param name="callBack"></param>
    public void SetData(string title, Action<float> callBack)
    {
        this.callBack = callBack;
        SetTitle(title);
    }

    /// <summary>
    /// 设置进度
    /// </summary>
    /// <param name="pro"></param>
    public void SetPro(float pro)
    {
        ui_ProContent.SetData(pro);
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="data"></param>
    public void SetContent(string data)
    {
        ui_ProContent.SetContent(data);
    }

    /// <summary>
    /// 进度修改通知
    /// </summary>
    /// <param name="progressView"></param>
    /// <param name="value"></param>
    public void OnProgressViewValueChange(ProgressView progressView, float value)
    {
        callBack?.Invoke(value);
    }

}