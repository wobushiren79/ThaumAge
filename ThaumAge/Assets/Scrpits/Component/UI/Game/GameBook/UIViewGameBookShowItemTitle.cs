using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookShowItemTitle : BaseUIView
{
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="titleStr"></param>
    public void SetData(string titleStr)
    {
        SetContent(titleStr);
    } 

    /// <summary>
    /// 设置正文标题内容
    /// </summary>
    /// <param name="titleStr"></param>
    public void SetContent(string titleStr)
    {
        ui_Content.text = titleStr;
        UGUIUtil.RefreshUISize(ui_Content.rectTransform);
    }
}