using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameBook : UIGameCommonNormal,IRadioGroupCallBack
{

    protected List<RadioButtonView> listLabels = new List<RadioButtonView>();
    protected List<BookModelInfoBean> listBookModel;
    public override void Awake()
    {
        base.Awake();
        ui_LabelItem.gameObject.SetActive(false);
        ui_Labels.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void InitData()
    {
        listBookModel = GameInfoHandler.Instance.manager.GetUnLockBookModelInfo();
        SetLabels(listBookModel);
    }

    /// <summary>
    /// 设置标签
    /// </summary>
    public void SetLabels(List<BookModelInfoBean> listBookModel)
    {
        ui_Labels.DestroyAllChild(true);
        listLabels.Clear();
        for (int i = 0; i < listBookModel.Count; i++)
        {
            BookModelInfoBean bookModel = listBookModel[i];
            //创建一个标签
            GameObject objItemLabel = Instantiate(ui_Labels.gameObject, ui_LabelItem.gameObject);
            objItemLabel.SetActive(true);
            //设置文本
            Transform tfTitle = objItemLabel.transform.Find("Title");
            Text tvTitle = tfTitle.GetComponent<Text>();
            tvTitle.text = bookModel.name;
            //获取按钮
            RadioButtonView btLabel = objItemLabel.GetComponent<RadioButtonView>();
            listLabels.Add(btLabel);         
        }
        ui_Labels.InitRadioButton();
        ui_Labels.SetPosition(0, true);
    }

    #region 选中回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        BookModelInfoBean bookModelInfo = listBookModel[position];
        IconHandler.Instance.manager.GetUISpriteByName(bookModelInfo.background,(iconSprite)=> 
        {
            ui_ContentBG.sprite = iconSprite;
        });
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}