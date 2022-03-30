using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class GameInfoManager : BaseManager,
    IBookModelInfoView, IBookModelDetailsInfoView
{

    protected BookModelInfoController controllerBookModel;
    protected BookModelDetailsInfoController controllerBookModelDetails;

    protected List<BookModelInfoBean> listBookModelInfo;
    protected void Awake()
    {
        controllerBookModel = new BookModelInfoController(this, this);
        controllerBookModelDetails = new BookModelDetailsInfoController(this, this);
        controllerBookModel.GetAllBookModelInfoData(InitBookModelInfo);
        controllerBookModelDetails.GetAllBookModelDetailsInfoData(InitBookModelDetailsInfo);
    }

    /// <summary>
    /// 获取解锁的模块
    /// </summary>
    public List<BookModelInfoBean> GetUnLockBookModelInfo()
    {
        return listBookModelInfo;
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="listData"></param>
    protected void InitBookModelInfo(List<BookModelInfoBean> listData)
    {
        this.listBookModelInfo = listData;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="listData"></param>
    protected void InitBookModelDetailsInfo(List<BookModelDetailsInfoBean> listData)
    {

    }

    #region 数据回调
    public void GetBookModelDetailsInfoFail(string failMsg, Action action)
    {
        action?.Invoke();
    }

    public void GetBookModelDetailsInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetBookModelInfoFail(string failMsg, Action action)
    {
        action?.Invoke();
    }

    public void GetBookModelInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }
    #endregion
}