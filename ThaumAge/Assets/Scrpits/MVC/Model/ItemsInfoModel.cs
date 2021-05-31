/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsInfoModel : BaseMVCModel
{
    protected ItemsInfoService serviceItemsInfo;

    public override void InitData()
    {
        serviceItemsInfo = new ItemsInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetAllItemsInfoData()
    {
        List<ItemsInfoBean> listData = serviceItemsInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfoData()
    {
        ItemsInfoBean data = serviceItemsInfo.QueryData();
        if (data == null)
            data = new ItemsInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsInfoDataById(long id)
    {
        List<ItemsInfoBean> listData = serviceItemsInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetItemsInfoData(ItemsInfoBean data)
    {
        serviceItemsInfo.UpdateData(data);
    }

}