using UnityEditor;
using UnityEngine;

public class UIPopupTextButton : PopupButtonView<UIPopupText>
{
    protected string textContent;
    public override void Awake()
    {
        base.Awake();
        popupType = PopupEnum.Text;
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    public void SetText(string textContent)
    {
        this.textContent = textContent;
    }

    public override void PopupHide()
    {

    }

    public override void PopupShow()
    {
        popupShow.SetData(textContent);
    }
}