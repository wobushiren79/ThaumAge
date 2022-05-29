using UnityEditor;
using UnityEngine;

public partial class UIViewItemCharacterStatus : BaseUIView
{
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(string iconKey, string statusStr, string popupShowStr)
    {
        SetIcon(iconKey);
        SetStatusContent(statusStr);
        SetPopupContent(popupShowStr);
    }

    /// <summary>
    /// 设置状态图标
    /// </summary>
    public void SetIcon(string iconKey)
    {
        IconHandler.Instance.manager.GetUISpriteByName(iconKey, (spIcon) =>
        {
             ui_Icon.sprite = spIcon;
        });
    }

    /// <summary>
    /// 设置状态文字
    /// </summary>
    public void SetStatusContent(string statusStr)
    {
        ui_Text.text = statusStr;
    }

    /// <summary>
    /// 设置弹窗文本
    /// </summary>
    public void SetPopupContent(string popupContent)
    {
        UIPopupTextButton uiPopupText = transform.GetComponent<UIPopupTextButton>();
        uiPopupText.SetText(popupContent);
    }
}