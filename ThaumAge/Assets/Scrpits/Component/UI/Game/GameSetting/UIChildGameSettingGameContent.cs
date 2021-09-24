using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildGameSettingGameContent : UIChildGameSettingBaseContent
{
    //����ѡ��
    protected UIListItemGameSettingSelect settingSelectLanguage;

    public List<string> listLanguageData;

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
        settingSelectLanguage = CreateItemForSelect("����", listLanguageData, HandleForSelectLanguage);
        settingSelectLanguage.SetIndex((int)gameConfig.GetLanguage());
    }

    /// <summary>
    /// ����-����ѡ��
    /// </summary>
    /// <param name="index"></param>
    public void HandleForSelectLanguage(int index)
    {
        gameConfig.SetLanguage((LanguageEnum)index);
        UIHandler.Instance.manager.RefreshAllUI();
    }
}
