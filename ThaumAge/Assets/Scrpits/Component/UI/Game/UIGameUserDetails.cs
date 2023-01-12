using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameUserDetails : UIGameCommonNormal, IRadioGroupCallBack
{
    public int labelIndex = 0;

    public override void Awake()
    {
        base.Awake();
        ui_Labels.SetCallBack(this);
        ui_ViewBackPack.isCloseClearAllCell = false;
    }

    protected void Start()
    {
        ui_Labels.SetCallBack(this);
        ui_Labels.SetPosition(labelIndex, true);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_Labels.SetPosition(0, true);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_Shortcuts.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit)
    {
        base.RefreshUI(isOpenInit);
        ui_LabelEquipContent.text = TextHandler.Instance.GetTextById(301);
        ui_LabelSynthesisContent.text = TextHandler.Instance.GetTextById(302);

        if (isOpenInit)
            return;
        ui_ViewBackPack.RefreshUI(isOpenInit);
        ui_Shortcuts.RefreshUI(isOpenInit);
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputName, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputName, callback);
        switch (inputName)
        {
            case InputActionUIEnum.B:
                HandleForBackGameMain();
                break;
        }
    }

    /// <summary>
    /// 设置选中的类型
    /// </summary>
    /// <param name="selectType"></param>
    public void SetSelectType(int selectType)
    {
        ui_Labels.SetPosition(selectType, true);
    }

    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        this.labelIndex = position;
        if (rbview == ui_ViewLabel_Equip)
        {
            ui_ViewCharacterEquip.OpenUI();
            ui_ViewSynthesis.CloseUI();
        }
        else if (rbview == ui_ViewLabel_Synthesis)
        {
            ui_ViewCharacterEquip.CloseUI();
            ui_ViewSynthesis.OpenUI();
        }
        AudioHandler.Instance.PlaySound(1);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
}