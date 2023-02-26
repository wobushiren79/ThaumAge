using UnityEditor;
using UnityEngine;

public partial class UIGameGolem : UIGameCommonNormal
{
    private CreatureCptGolem golem;
    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
        ui_ViewBagList.OpenUI();
        ui_ViewGolemDetails.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
        ui_ViewBagList.CloseUI();
        ui_ViewGolemDetails.CloseUI();

        if (golem != null)
        {
            //初始化这个傀儡的AI意图
            golem.aiEntity.InitIntentEntity();
        }
    }

    public override void RefreshUI(bool isOpenInit)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
        ui_ViewBagList.RefreshUI();
        ui_ViewGolemDetails.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(CreatureCptGolem golem)
    {
        this.golem = golem;

        ItemMetaGolem itemMetaGolem = golem.golemMetaData;
        //设置背包数据
        ui_ViewBagList.SetData(itemMetaGolem.bagData);
        //设置核心数据
        ui_ViewGolemDetails.SetData(itemMetaGolem.listGolemCore);
    }
}