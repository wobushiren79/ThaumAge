using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIListItemGameSettingRange : UIListItemGameSettingBase, ProgressView.ICallBack
{
    Action<float> callBack;

    protected void Start()
    {
        //ui_ProContent.SetCallBack(this);
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
       // ui_ProContent.SetData(pro);
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