using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIChildGameSettingBaseContent
{
    public GameObject objListContainer;
    //游戏设置
    public GameConfigBean gameConfig;

    public UIChildGameSettingBaseContent(GameObject objListContainer)
    {
        this.objListContainer = objListContainer;
    }

    public virtual void Open()
    {
        CptUtil.RemoveChildsByActive(objListContainer);
        gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
    }

    public virtual void RefreshUI()
    {

    }

    /// <summary>
    /// 创建范围选择
    /// </summary>
    public UIListItemGameSettingRange CreateItemForRange(string title, Action<float> callBack)
    {
        GameObject objItem = LoadItem("ListItemGameSettingRange");
        UIListItemGameSettingRange rangeItem = objItem.GetComponent<UIListItemGameSettingRange>();
        rangeItem.SetData(title, callBack);
        return rangeItem;
    }

    /// <summary>
    /// 创建单选
    /// </summary>
    public UIListItemGameSettingRB CreateItemForRB(string title, Action<bool> callBack)
    {
        GameObject objItem = LoadItem("ListItemGameSettingRB");
        UIListItemGameSettingRB rbItem = objItem.GetComponent<UIListItemGameSettingRB>();
        rbItem.SetData(title, callBack);
        return rbItem;
    }

    /// <summary>
    /// 创建多选
    /// </summary>
    /// <param name="title"></param>
    /// <param name="listSelectData"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public UIListItemGameSettingSelect CreateItemForSelect(string title, List<string> listSelectData, Action<int> callBack)
    {
        GameObject objItem = LoadItem("ListItemGameSettingSelect");
        UIListItemGameSettingSelect selectItem = objItem.GetComponent<UIListItemGameSettingSelect>();
        selectItem.SetData(title, listSelectData, callBack);
        return selectItem;
    }

    /// <summary>
    ///  读取控件
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    protected GameObject LoadItem(string itemName)
    {
        GameObject objItemModel = LoadResourcesUtil.SyncLoadData<GameObject>($"UI/Game/GameSetting/{itemName}");
        GameObject objItem = UIHandler.Instance.Instantiate(objListContainer, objItemModel);
        return objItem;
    }
}