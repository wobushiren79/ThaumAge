using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIListItemGameSettingBase : BaseUIView
{
    public Text ui_Title;

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title)
    {
        ui_Title.text = title;
    }
}