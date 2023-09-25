using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildGameSettingGameContent : UIChildGameSettingBaseContent
{
    //语言选择
    protected UIListItemGameSettingSelect settingSelectLanguage;
    //加载范围
    protected UIListItemGameSettingRange worldRefreshRange;
    //卸载范围
    protected UIListItemGameSettingRange worldDestoryRange;
    //实体方块范围
    protected UIListItemGameSettingRange entityShowDis;

    public List<string> listLanguageData;

    private bool isInitWorldRefreshRange = false;
    private bool isInitWorldDestoryRange = false;
    public UIChildGameSettingGameContent(GameObject objListContainer) : base(objListContainer)
    {
        listLanguageData = new List<string>()
        {
            "中文",
            "English"
        };
    }

    public override void Open()
    {
        base.Open();

        //语言选择
        settingSelectLanguage = CreateItemForSelect(TextHandler.Instance.GetTextById(101), listLanguageData, HandleForSelectLanguage);
        settingSelectLanguage.SetIndex((int)gameConfig.GetLanguage());

        //加载范围·
        worldRefreshRange = CreateItemForRange(TextHandler.Instance.GetTextById(116), HandleForWorldRefreshRange);
        worldRefreshRange.SetMinMax(3, 16); 
        isInitWorldRefreshRange = true;
        worldRefreshRange.SetPro(gameConfig.worldRefreshRange);

        //卸载范围
        worldDestoryRange = CreateItemForRange(TextHandler.Instance.GetTextById(117), HandleForWorldDestoryRange);
        worldDestoryRange.SetMinMax(3, 10); 
        isInitWorldDestoryRange = true;
        worldDestoryRange.SetPro(gameConfig.worldDestoryRange);

        //实体方块范围
        entityShowDis = CreateItemForRange(TextHandler.Instance.GetTextById(120), HandleForEntityShowDis);
        entityShowDis.SetPro(gameConfig.entityShowDis / 200);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (settingSelectLanguage)
            settingSelectLanguage.SetTitle(TextHandler.Instance.GetTextById(101));
        if (worldRefreshRange)
            worldRefreshRange.SetTitle(TextHandler.Instance.GetTextById(116));
        if (worldDestoryRange)
            worldDestoryRange.SetTitle(TextHandler.Instance.GetTextById(117));
    }

    /// <summary>
    /// 处理-刷新范围
    /// </summary>
    public void HandleForWorldRefreshRange(float value)
    {
        if (!isInitWorldRefreshRange)
            return;
        gameConfig.worldRefreshRange = (int)value;
        worldRefreshRange.SetContent($"{gameConfig.worldRefreshRange}");
        WorldCreateHandler.Instance.HandleForWorldUpdate(false);
    }

    /// <summary>
    /// 处理-刷新范围
    /// </summary>
    public void HandleForWorldDestoryRange(float value)
    {
        if (!isInitWorldDestoryRange)
            return;
        gameConfig.worldDestoryRange = (int)value;
        worldDestoryRange.SetContent($"{gameConfig.worldDestoryRange}");
        WorldCreateHandler.Instance.HandleForWorldUpdate(false);
    }

    /// <summary>
    /// 实体显示距离
    /// </summary>
    public void HandleForEntityShowDis(float value)
    {
        gameConfig.entityShowDis = value * 200;
        entityShowDis.SetContent($"{Math.Round(gameConfig.entityShowDis, 0)}m");
    }

    /// <summary>
    /// 处理-语言选择
    /// </summary>
    /// <param name="index"></param>
    public void HandleForSelectLanguage(int index)
    {
        gameConfig.SetLanguage((LanguageEnum)index);
        UIHandler.Instance.RefreshUI();
        RefreshUI();
    }
}
