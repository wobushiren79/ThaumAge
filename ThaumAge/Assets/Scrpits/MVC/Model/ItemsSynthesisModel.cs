/*
* FileName: ItemsSynthesis 
* Author: AppleCoffee 
* CreateTime: 2021-12-28-17:06:25 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsSynthesisModel : BaseMVCModel
{
    protected ItemsSynthesisService serviceItemsSynthesis;

    public override void InitData()
    {
        serviceItemsSynthesis = new ItemsSynthesisService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsSynthesisBean> GetAllItemsSynthesisData()
    {
        List<ItemsSynthesisBean> listData = serviceItemsSynthesis.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public ItemsSynthesisBean GetItemsSynthesisData()
    {
        ItemsSynthesisBean data = serviceItemsSynthesis.QueryData();
        if (data == null)
            data = new ItemsSynthesisBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsSynthesisBean> GetItemsSynthesisDataById(long id)
    {
        List<ItemsSynthesisBean> listData = serviceItemsSynthesis.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetItemsSynthesisData(ItemsSynthesisBean data)
    {
        serviceItemsSynthesis.UpdateData(data);
    }

}