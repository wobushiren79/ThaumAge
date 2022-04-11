using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookShowItemContent : BaseUIView
{
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="contentStr"></param>
    public void SetData(string contentStr)
    {
        SetContent(contentStr);
    }

    /// <summary>
    /// 设置正文内容
    /// </summary>
    /// <param name="contentStr"></param>
    public void SetContent(string contentStr)
    {
        ui_Content.text = contentStr;
    }
}