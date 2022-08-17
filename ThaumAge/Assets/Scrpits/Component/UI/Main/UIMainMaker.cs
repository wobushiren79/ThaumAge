using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIMainMaker : BaseUIComponent
{
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ViewClose)
        {
            HandleForBack();
        }
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        SetUIText();
    }

    /// <summary>
    /// 设置UI文本
    /// </summary>
    public void SetUIText()
    {
        ui_TitleDesign.text = TextHandler.Instance.GetTextById(11);
        ui_TitleProgram.text = TextHandler.Instance.GetTextById(12);
        ui_TitleUI.text = TextHandler.Instance.GetTextById(13);
        ui_TitleModel.text = TextHandler.Instance.GetTextById(14);
        ui_TitleAction.text = TextHandler.Instance.GetTextById(15);
    }

    /// <summary>
    /// 处理-返回
    /// </summary>
    public void HandleForBack()
    {
        UIMainStart uiMainStart = UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
    }
}