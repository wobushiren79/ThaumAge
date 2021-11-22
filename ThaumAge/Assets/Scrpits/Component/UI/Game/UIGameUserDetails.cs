using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameUserDetails : UIGameCommonNormal,IRadioGroupCallBack
{
    public int labelIndex = 0;
    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();
        SetText();
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

        }
        else if (rbview == ui_ViewLabel_Synthesis)
        {

        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
}