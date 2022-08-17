using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildGameSettingGameContent : UIChildGameSettingBaseContent
{
    //”Ô—‘—°‘Ò
    protected UIListItemGameSettingSelect settingSelectLanguage;
    //º”‘ÿ∑∂Œß
    protected UIListItemGameSettingRange worldRefreshRange;
    //–∂‘ÿ∑∂Œß
    protected UIListItemGameSettingRange worldDestoryRange;

    public List<string> listLanguageData;

    private bool isInitWorldRefreshRange = false;
    private bool isInitWorldDestoryRange = false;
    public UIChildGameSettingGameContent(GameObject objListContainer) : base(objListContainer)
    {
        listLanguageData = new List<string>()
        {
            "÷–Œƒ",
            "English"
        };
    }

    public override void Open()
    {
        base.Open();

        //”Ô—‘—°‘Ò
        settingSelectLanguage = CreateItemForSelect(TextHandler.Instance.GetTextById(101), listLanguageData, HandleForSelectLanguage);
        settingSelectLanguage.SetIndex((int)gameConfig.GetLanguage());

        //º”‘ÿ∑∂Œß°§
        worldRefreshRange = CreateItemForRange(TextHandler.Instance.GetTextById(116), HandleForWorldRefreshRange);
        worldRefreshRange.SetMinMax(3, 16); 
        isInitWorldRefreshRange = true;
        worldRefreshRange.SetPro(gameConfig.worldRefreshRange);

        //–∂‘ÿ∑∂Œß
        worldDestoryRange = CreateItemForRange(TextHandler.Instance.GetTextById(117), HandleForWorldDestoryRange);
        worldDestoryRange.SetMinMax(3, 10); 
        isInitWorldDestoryRange = true;
        worldDestoryRange.SetPro(gameConfig.worldDestoryRange);

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
    /// ¥¶¿Ì-À¢–¬∑∂Œß
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
    /// ¥¶¿Ì-À¢–¬∑∂Œß
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
    /// ¥¶¿Ì-”Ô—‘—°‘Ò
    /// </summary>
    /// <param name="index"></param>
    public void HandleForSelectLanguage(int index)
    {
        gameConfig.SetLanguage((LanguageEnum)index);
        UIHandler.Instance.RefreshAllUI();
        RefreshUI();
    }
}
