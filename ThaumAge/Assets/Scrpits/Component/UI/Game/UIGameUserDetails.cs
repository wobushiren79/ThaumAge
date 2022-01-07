using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameUserDetails : UIGameCommonNormal,IRadioGroupCallBack
{
    public int labelIndex = 0;

    public override void Awake()
    {
        base.Awake();
        ui_Labels.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_ViewBackPack.Show();
        ui_Labels.SetPosition(0,true);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();
        SetText();
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputName)
    {
        base.OnInputActionForStarted(inputName);
        switch (inputName)
        {
            case InputActionUIEnum.ESC:
            case InputActionUIEnum.B:
                HandleForBackMain();
                break;
        }
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);

    }
    protected void Start()
    {
        ui_Labels.SetCallBack(this);
        ui_Labels.SetPosition(labelIndex, true);
    }

    public void SetText()
    {
        ui_LabelEquipContent.text = TextHandler.Instance.GetTextById(301);
        ui_LabelSynthesisContent.text = TextHandler.Instance.GetTextById(302);
    }

    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        this.labelIndex = position;
        if (rbview == ui_ViewLabel_Equip)
        {
            ui_ViewCharacterEquip.Show();
            ui_ViewSynthesis.Hide();
        }
        else if (rbview == ui_ViewLabel_Synthesis)
        {
            ui_ViewCharacterEquip.Hide();
            ui_ViewSynthesis.Show();
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
}