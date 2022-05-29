using UnityEditor;
using UnityEngine;

public partial class UIPopupText : PopupShowView
{
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(string textContent)
    {
        ui_ItemDetails.text = textContent;
    }
}