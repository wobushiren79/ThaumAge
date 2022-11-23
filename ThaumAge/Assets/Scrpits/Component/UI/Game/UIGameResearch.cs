using UnityEditor;
using UnityEngine;

public partial class UIGameResearch : UIGameCommonNormal, IRadioGroupCallBack
{
    protected UIPopupTextButton popupTypeFocalManipulator;
    public override void Awake()
    {
        base.Awake();
        popupTypeFocalManipulator = ui_TypeFocalManipulator.GetComponent<UIPopupTextButton>();

        ui_ViewItemResearch.ShowObj(false);
        ui_ResearchType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_ViewMaterialsShow.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ResearchType.SetPosition(0, true);

        popupTypeFocalManipulator.SetText(TextHandler.Instance.GetTextById(601));
    }

    #region 回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}