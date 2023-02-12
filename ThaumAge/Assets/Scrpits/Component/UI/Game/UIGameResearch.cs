using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameResearch : UIGameCommonNormal, IRadioGroupCallBack
{
    protected UIPopupTextButton popupTypeFocalManipulator;

    //当前分类下所有的数据
    protected List<ResearchInfoBean> listAllResearchData = new List<ResearchInfoBean>();
    //当前分类下展示出来的研究(已解锁)
    protected List<UIViewItemResearch> listViewResearch = new List<UIViewItemResearch>();
    //选择的材料数据
    protected List<ItemsBean> listSelectMaterialsData = new List<ItemsBean>();

    protected int currentSelectIndex;
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
        currentSelectIndex = 0;

        ui_ViewMaterialsShow.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ResearchType.SetPosition(0, true);

        popupTypeFocalManipulator.SetText(TextHandler.Instance.GetTextById(601));
        ui_ResearchBtnTex.text = TextHandler.Instance.GetTextById(602);
        //默认选中第一个
        CallBackForSelectChange(currentSelectIndex);

        this.RegisterEvent<int>(EventsInfo.UIGameResearch_ChangeSelect, CallBackForSelectChange);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        foreach (var itemView in listViewResearch)
        {
            itemView.CloseUI();
        }
        ui_ResearchContent.DestroyAllChild(true);
        listViewResearch.Clear();

        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewMaterialsShow.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
        ui_ViewMaterialsShow.RefreshUI();
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ResearchBtn)
        {
            OnClickForSubmitResearch();
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="index"></param>
    public void InitType(int index)
    {
        switch (index)
        {
            //核心镶嵌研究
            case 0:
                listAllResearchData = ResearchInfoCfg.GetResearchInfoByType(1);
                break;
        }
    }

    /// <summary>
    /// 刷新研究显示数据
    /// </summary>
    public void RefreshResearchShow()
    {
        ui_ResearchContent.DestroyAllChild(true);
        listViewResearch.Clear();
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        int listViewIndex = 0;
        for (int i = 0; i < listAllResearchData.Count; i++)
        {
            var itemData = listAllResearchData[i];
            //如果不需要解锁 直接就有
            if (itemData.need_unlock == 0)
                continue;
            //如果需要解锁
            int[] arrayUnlockPreResearch = itemData.GetUnlockPreResearch();
            //判断前置解锁条件
            bool isUnlockPreResarch = userData.userAchievement.CheckUnlockResearch(arrayUnlockPreResearch);
            if (!isUnlockPreResarch)
                continue;
            GameObject objItem = Instantiate(ui_ResearchContent.gameObject, ui_ViewItemResearch.gameObject);
            UIViewItemResearch viewItem = objItem.GetComponent<UIViewItemResearch>();
            viewItem.SetData(listViewIndex,itemData);
            listViewResearch.Add(viewItem);
            listViewIndex++;
        }
    }

    /// <summary>
    /// 点击-提交
    /// </summary>
    public void OnClickForSubmitResearch()
    {
        var researchView = listViewResearch[currentSelectIndex];
        var researchData = researchView.researchInfo;

        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        var progressData = userData.userAchievement.GetResearchProgressData(researchData.id);
        //判断是否已经解锁
        bool isUnlock = userData.userAchievement.CheckUnlockResearch(researchData.id);
        if (isUnlock)
        {
            //如果已经有数据
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30007));
            return;
        }
        if (progressData == null)
        {
            //如果没有数据
            DialogBean dialogData = new DialogBean();
            dialogData.content = string.Format(TextHandler.Instance.GetTextById(20005), researchData.GetContent());
            dialogData.actionSubmit = (view,data) =>
            {
                bool hasEnoughItem = userData.HasEnoughItem(listSelectMaterialsData);
                if (!hasEnoughItem)
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30002));
                    return;
                }
                userData.userAchievement.AddResearchProgressData(researchData.id, researchData.time);
                //扣除材料
                userData.RemoveItem(listSelectMaterialsData);
                //刷新UI
                UIHandler.Instance.RefreshUI();
            };
            UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
            AudioHandler.Instance.PlaySound(1);
        }
        else
        {
            //如果已经有数据
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30006));
        }
        researchView.SetUnlockPro();
    }

    #region 回调

    /// <summary>
    /// 选择变换回调
    /// </summary>
    public void CallBackForSelectChange(int selectIndex)
    {
        this.currentSelectIndex = selectIndex;
        foreach (var itemView in listViewResearch)
        {
            itemView.SetSelectState(selectIndex);
        }
        var researchView = listViewResearch[currentSelectIndex];
        var researchData = researchView.researchInfo;
        //设置选中的材料
        listSelectMaterialsData = ItemsBean.GetListItemsBean(researchData.unlock_materials);
        ui_ViewMaterialsShow.SetData(listSelectMaterialsData);
    }


    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        if (rbview == ui_TypeFocalManipulator)
        {
            //打开镶嵌类型
        }
        InitType(position);
        RefreshResearchShow(); 
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}