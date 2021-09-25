using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildGameSettingGameContent : UIChildGameSettingBaseContent
{
    //语言选择
    protected UIListItemGameSettingSelect settingSelectLanguage;

    public List<string> listLanguageData;

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
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        settingSelectLanguage.SetTitle(TextHandler.Instance.GetTextById(101));
    }


    /// <summary>
    /// 处理-语言选择
    /// </summary>
    /// <param name="index"></param>
    public void HandleForSelectLanguage(int index)
    {
        gameConfig.SetLanguage((LanguageEnum)index);
        UIHandler.Instance.manager.RefreshAllUI();
        RefreshUI();
    }
}
