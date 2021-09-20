using System;
using UnityEditor;
using UnityEngine;

public class UIChildGameSettingBaseContent
{
    public GameObject objListContainer;

    public UIChildGameSettingBaseContent(GameObject objListContainer)
    {
        this.objListContainer = objListContainer;
    }

    public virtual void Open()
    {
        CptUtil.RemoveChildsByActive(objListContainer);
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
    public void CreateItemForRB(string title)
    {
        GameObject objItem = LoadItem("ListItemGameSettingRB");
    }

    protected GameObject LoadItem(string itemName)
    {
        GameObject objItemModel = LoadResourcesUtil.SyncLoadData<GameObject>($"UI/GameSetting/{itemName}");
        GameObject objItem = UIHandler.Instance.Instantiate(objListContainer, objItemModel);
        return objItem;
    }
}