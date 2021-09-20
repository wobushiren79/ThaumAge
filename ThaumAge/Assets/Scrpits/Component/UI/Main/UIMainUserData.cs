using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIMainUserData : BaseUIComponent
{
    protected List<UserDataBean> listUserData;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ViewClose) HandleForBackStartUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        ShowUserData();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataHandler.Instance.manager.GetAllUserData(SetUserData);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        listUserData = null;
    }

    /// <summary>
    /// 设置用户数据
    /// </summary>
    public void SetUserData(List<UserDataBean> listUserData)
    {
        this.listUserData = listUserData;
        RefreshUI();
    }

    /// <summary>
    /// 展示用户数据
    /// </summary>
    public void ShowUserData()
    {
        UserDataBean userData1 = GetUserDataByIndex(1);
        ui_ItemMainUserData_1.SetData(1, userData1);

        UserDataBean userData2 = GetUserDataByIndex(2);
        ui_ItemMainUserData_2.SetData(2, userData2);

        UserDataBean userData3 = GetUserDataByIndex(3);
        ui_ItemMainUserData_3.SetData(3, userData3);
    }

    /// <summary>
    /// 处理-返回主菜单
    /// </summary>
    public void HandleForBackStartUI()
    {
        UIMainStart uiMainStart = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
    }

    /// <summary>
    /// 通过序号获取用户数据
    /// </summary>
    private UserDataBean GetUserDataByIndex(int index)
    {
        if (listUserData == null || listUserData.Count == 0)
            return null;
        for (int i = 0; i < listUserData.Count; i++)
        {
            UserDataBean userData = listUserData[i];
            if (userData.dataIndex == index)
            {
                return userData;
            }
        }
        return null;
    }
}