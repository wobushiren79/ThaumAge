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

    public List<ItemsInfoBean> GetAllItemsInfo()
    {
        return listItemsInfo;
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