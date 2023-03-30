using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewItemResearch : BaseUIView
{
    //列表下表
    public int listIndex;
    public ResearchInfoBean researchInfo;

    protected Button ui_BtnSelect;
    protected UIPopupTextButton popupContent;

    public override void Awake()
    {
        base.Awake();
        ui_Select.ShowObj(false);
        popupContent = GetComponent<UIPopupTextButton>();
        ui_BtnSelect = GetComponent<Button>();
        RegisterButton(ui_BtnSelect);

        Material matIcon = new Material(ui_Icon.material);
        ui_Icon.material = matIcon;
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BtnSelect)
        {
            OnClickForSelect();
        }
    }

    /// <summary>
    /// 点击选择
    /// </summary>
    public void OnClickForSelect()
    {
        this.TriggerEvent(EventsInfo.UIGameResearch_ChangeSelect, listIndex);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(int listIndex, ResearchInfoBean researchInfo)
    {
        this.researchInfo = researchInfo;
        this.listIndex = listIndex;
        this.RegisterEvent(EventsInfo.CharacterStatus_ResearchChange, CallBackForProgressChange);

        SetIcon(researchInfo.icon_key);
        SetSelectState(-1);
        SetPopupContent(researchInfo.GetContent());
        SetUnlockPro();
    }

    /// <summary>
    /// 设置提示窗内容
    /// </summary>
    /// <param name="content"></param>
    public void SetPopupContent(string content)
    {
        popupContent.SetText(content);
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    public void SetSelectState(int currentSelectIndex)
    {
        if (listIndex == currentSelectIndex)
        {
            ui_Select.ShowObj(true);
        }
        else
        {
            ui_Select.ShowObj(false);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    public void SetIcon(string iconName)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconName, (spriteIcon) =>
         {
             ui_Icon.sprite = spriteIcon;
             ui_IconPro.sprite = spriteIcon;
         });
    }


    /// <summary>
    /// 设置解锁进度
    /// </summary>
    public void SetUnlockPro()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        bool isUnlock = userData.userAchievement.CheckUnlockResearch((int)researchInfo.id);
        if (isUnlock)
        {
            ui_Icon.materialForRendering.SetFloat("_EffectAmount", 0);
            ui_IconPro.ShowObj(false);
            ui_ResearchPro.ShowObj(false);
            return;
        }
        ProgressBean progressData = userData.userAchievement.GetResearchProgressData((int)researchInfo.id);
        if (progressData == null || progressData.progress == 0)
        {
            ui_Icon.materialForRendering.SetFloat("_EffectAmount", 1);
            ui_IconPro.ShowObj(false);
            ui_ResearchPro.ShowObj(true);
            ui_ResearchPro.text = $"{0}%";
            return;
        }

        ui_IconPro.ShowObj(true);
        ui_ResearchPro.ShowObj(true);

        ui_Icon.materialForRendering.SetFloat("_EffectAmount", 1);
        ui_IconPro.fillAmount = progressData.progress;
        ui_ResearchPro.text = $"{Mathf.FloorToInt(progressData.progress * 100)}%";
    }

    /// <summary>
    /// 状态改变回调
    /// </summary>
    public void CallBackForProgressChange()
    {
        if (gameObject != null && gameObject.activeSelf)
        {
            SetUnlockPro();
        }
    }
}