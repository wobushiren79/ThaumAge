using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class UIGameBook : UIGameCommonNormal, IRadioGroupCallBack
{
    protected List<RadioButtonView> listLabels = new List<RadioButtonView>();
    protected List<BookModelInfoBean> listBookModel;
    protected int labelIndex = 0;
    public override void Awake()
    {
        base.Awake();
        ui_LabelItem.gameObject.SetActive(false);
        ui_Labels.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_ViewGameBookContentMap.OpenUI();
        ui_ViewGameBookShowDetails.OpenUI();
        labelIndex = 0;
        InitData();
        this.RegisterEvent(EventsInfo.UIGameBook_RefreshLabels, RefreshLabels);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_ViewGameBookContentMap.CloseUI();
        ui_ViewGameBookShowDetails.CloseUI();
    }

    public void InitData()
    {
        listBookModel = GameInfoHandler.Instance.manager.GetUnLockBookModelInfo();
        SetLabels(listBookModel);
        ui_Labels.SetPosition(labelIndex, true);
    }

    /// <summary>
    /// 刷新标签
    /// </summary>
    public void RefreshLabels()
    {
        SetLabels(listBookModel);
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        switch (inputType)
        {
            case InputActionUIEnum.ESC:
                HandleForBackMain();
                break;
        }
    }

    /// <summary>
    /// 设置标签
    /// </summary>
    public void SetLabels(List<BookModelInfoBean> listBookModel)
    {
        ui_Labels.DestroyAllChild(true, 1);
        listLabels.Clear();
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        for (int i = 0; i < listBookModel.Count; i++)
        {
            BookModelInfoBean bookModel = listBookModel[i];
            var isUnlock = userData.userAchievement.CheckUnlockBookModel(bookModel.unlock_model_details_id);
            if (!isUnlock)
                continue;
            //创建一个标签
            GameObject objItemLabel = Instantiate(ui_Labels.gameObject, ui_LabelItem.gameObject);
            objItemLabel.SetActive(true);
            //设置文本
            Transform tfTitle = objItemLabel.transform.Find("Title");
            Text tvTitle = tfTitle.GetComponent<Text>();
            tvTitle.text = bookModel.GetName();
            //获取按钮
            RadioButtonView btLabel = objItemLabel.GetComponent<RadioButtonView>();
            listLabels.Add(btLabel);
        }
        ui_Labels.InitRadioButton();
        ui_Labels.SetPosition(labelIndex, false);
    }

    #region 选中回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        BookModelInfoBean bookModelInfo = listBookModel[position];
        ui_ViewGameBookContentMap.SetData(bookModelInfo);

        //播放音效声音
        AudioHandler.Instance.PlaySound(802);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}