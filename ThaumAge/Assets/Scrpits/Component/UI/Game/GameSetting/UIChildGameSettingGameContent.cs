using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildGameSettingGameContent : UIChildGameSettingBaseContent
{
    //����ѡ��
    protected UIListItemGameSettingSelect settingSelectLanguage;
    //���ط�Χ
    protected UIListItemGameSettingRange worldRefreshRange;
    //ж�ط�Χ
    protected UIListItemGameSettingRange worldDestoryRange;
    //ʵ�巽�鷶Χ
    protected UIListItemGameSettingRange entityShowDis;

    public List<string> listLanguageData;

    private bool isInitWorldRefreshRange = false;
    private bool isInitWorldDestoryRange = false;
    public UIChildGameSettingGameContent(GameObject objListContainer) : base(objListContainer)
    {
        listLanguageData = new List<string>()
        {
            "����",
            "English"
        };
    }

    public override void Open()
    {
        base.Open();

        //����ѡ��
        settingSelectLanguage = CreateItemForSelect(TextHandler.Instance.GetTextById(101), listLanguageData, HandleForSelectLanguage);
        settingSelectLanguage.SetIndex((int)gameConfig.GetLanguage());

        //���ط�Χ��
        worldRefreshRange = CreateItemForRange(TextHandler.Instance.GetTextById(116), HandleForWorldRefreshRange);
        worldRefreshRange.SetMinMax(3, 16); 
        isInitWorldRefreshRange = true;
        worldRefreshRange.SetPro(gameConfig.worldRefreshRange);

        //ж�ط�Χ
        worldDestoryRange = CreateItemForRange(TextHandler.Instance.GetTextById(117), HandleForWorldDestoryRange);
        worldDestoryRange.SetMinMax(3, 10); 
        isInitWorldDestoryRange = true;
        worldDestoryRange.SetPro(gameConfig.worldDestoryRange);

        //ʵ�巽�鷶Χ
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
    /// ����-ˢ�·�Χ
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
    /// ����-ˢ�·�Χ
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
    /// ʵ����ʾ����
    /// </summary>
    public void HandleForEntityShowDis(float value)
    {
        gameConfig.entityShowDis = value * 200;
        entityShowDis.SetContent($"{Math.Round(gameConfig.entityShowDis, 0)}m");
    }

    /// <summary>
    /// ����-����ѡ��
    /// </summary>
    /// <param name="index"></param>
    public void HandleForSelectLanguage(int index)
    {
        gameConfig.SetLanguage((LanguageEnum)index);
        UIHandler.Instance.RefreshUI();
        RefreshUI();
    }
}
