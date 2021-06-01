using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsManager : BaseManager, IItemsInfoView
{
    protected ItemsInfoController controllerForItems;
    protected Dictionary<long, ItemsInfoBean> dicItemsInfo = new Dictionary<long, ItemsInfoBean>();
    protected List<ItemsInfoBean> listItemsInfo = new List<ItemsInfoBean>();

    protected void Awake()
    {
        controllerForItems = new ItemsInfoController(this, this);
        controllerForItems.GetAllItemsInfoData(InitItemsInfo);
    }

    public void InitItemsInfo(List<ItemsInfoBean> listItemsInfo)
    {
        this.listItemsInfo = listItemsInfo;
        dicItemsInfo = TypeConversionUtil.ListToMap(listItemsInfo);
    }

    /// <summary>
    /// 获取所有物体信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetAllItemsInfo()
    {
        return listItemsInfo;
    }

    /// <summary>
    /// 根据ID获取物品数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfoById(long id)
    {
        if (dicItemsInfo.TryGetValue(id,out ItemsInfoBean itemsInfo))
        {
            return itemsInfo;
        }
        return null;
    }

    #region 数据回调
    public void GetItemsInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetItemsInfoFail(string failMsg, Action action)
    {

    }
    #endregion
}