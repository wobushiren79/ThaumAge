using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameSetting : UIGameCommonNormal, IRadioGroupCallBack
{
    //游戏设置内容
    protected UIChildGameSettingGameContent settingGameContent;
    protected UIChildGameSettingDisplayContent settingDisplayContent;
    protected UIChildGameSettingAudioContent settingAudioContent;
    protected UIChildGameSettingControlContent settingControlContent;

    //选中的下标
    protected int index = 0;

    public override void Awake()
    {
        base.Awake();
        settingGameContent = new UIChildGameSettingGameContent(ui_ListSettingContent.gameObject);
        settingDisplayContent = new UIChildGameSettingDisplayContent(ui_ListSettingContent.gameObject);
        settingAudioContent = new UIChildGameSettingAudioContent(ui_ListSettingContent.gameObject);
        settingControlContent = new UIChildGameSettingControlContent(ui_ListSettingContent.gameObject);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(false);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
    }

    protected void Start()
    {
        ui_Labels.SetCallBack(this);
        ui_Labels.SetPosition(0, true);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);

        ui_LabelGameContent.text = TextHandler.Instance.GetTextById(21);
        ui_LabelDisplayContent.text = TextHandler.Instance.GetTextById(22);
        ui_LabelAudioContent.text = TextHandler.Instance.GetTextById(23);
        ui_LabelControlContent.text = TextHandler.Instance.GetTextById(24);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void HandleForBackGameMain()
    {
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
        }
        else if (SceneUtil.GetCurrentScene() == ScenesEnum.GameScene)
        {
            UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
        }
        GameDataHandler.Instance.manager.SaveGameConfig();
        //播放音效
        AudioHandler.Instance.PlaySound(2);
    }



    #region 选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        this.index = position;
        if (rbview == ui_ViewLabel_Game)
        {
            settingGameContent.Open();
            AudioHandler.Instance.PlaySound(1);
        }
        else if (rbview == ui_ViewLabel_Display)
        {
            settingDisplayContent.Open();
            AudioHandler.Instance.PlaySound(1);
        }
        else if (rbview == ui_ViewLabel_Audio)
        {
            settingAudioContent.Open();
            AudioHandler.Instance.PlaySound(1);
        }
        else if (rbview == ui_ViewLabel_Control)
        {
            settingControlContent.Open();
            AudioHandler.Instance.PlaySound(1);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}