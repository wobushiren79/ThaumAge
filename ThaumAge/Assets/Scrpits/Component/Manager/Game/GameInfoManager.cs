using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class GameInfoManager : BaseManager,
    IBookModelInfoView, IBookModelDetailsInfoView
{

    protected BookModelInfoController controllerBookModel;
    protected BookModelDetailsInfoController controllerBookModelDetails;

    protected void Awake()
    {
        controllerBookModel = new BookModelInfoController(this, this);
        controllerBookModelDetails = new BookModelDetailsInfoController(this, this);
        controllerBookModel.GetAllBookModelInfoData(InitBookModelInfo);
        controllerBookModelDetails.GetAllBookModelDetailsInfoData(InitBookModelDetailsInfo);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="listData"></param>
    protected void InitBookModelInfo(List<BookModelInfoBean> listData)
    {

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

    }

    public void GetBookModelDetailsInfoSuccess<T>(T data, Action<T> action)
    {

    }

    public void GetBookModelInfoFail(string failMsg, Action action)
    {

    }

    public void GetBookModelInfoSuccess<T>(T data, Action<T> action)
    {

    }
    #endregion
}