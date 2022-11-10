using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class GameInfoManager : BaseManager,
    IBookModelInfoView, IBookModelDetailsInfoView, IElementalInfoView,IMagicInstrumentInfoView
{
    protected BookModelInfoController controllerBookModel;
    protected BookModelDetailsInfoController controllerBookModelDetails;
    protected ElementalInfoController controllerElementInfo;
    protected MagicInstrumentInfoController controllerMagicInstrument;

    //流程书大类
    protected List<BookModelInfoBean> listBookModelInfo;
    //流程书详情
    protected Dictionary<long, List<BookModelDetailsInfoBean>> dicBookModelDetailsInfo;
    //元素信息
    protected Dictionary<ElementalTypeEnum, ElementalInfoBean> dicElementalInfo;
    //元素信息
    protected Dictionary<int, MagicInstrumentInfoBean> dicMagicInstrumentInfo;
    protected void Awake()
    {
        controllerBookModel = new BookModelInfoController(this, this);
        controllerBookModelDetails = new BookModelDetailsInfoController(this, this);
        controllerElementInfo = new ElementalInfoController(this, this);
        controllerMagicInstrument = new MagicInstrumentInfoController(this,this);

        controllerBookModel.GetAllBookModelInfoData(InitBookModelInfo);
        controllerBookModelDetails.GetAllBookModelDetailsInfoData(InitBookModelDetailsInfo);
        controllerElementInfo.GetAllElementalInfoData(InitElementalInfo);
        controllerMagicInstrument.GetAllMagicInstrumentInfoData(InitMagicInstrumentInfo);
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
    protected void InitMagicInstrumentInfo(List<MagicInstrumentInfoBean> listData)
    {
        if (dicMagicInstrumentInfo == null)
        {
            dicMagicInstrumentInfo = new Dictionary<int, MagicInstrumentInfoBean>();
        }
        dicMagicInstrumentInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            dicMagicInstrumentInfo.Add(itemData.item_id, itemData);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    protected void InitElementalInfo(List<ElementalInfoBean> listData)
    {
        if (dicElementalInfo == null)
        {
            dicElementalInfo = new Dictionary<ElementalTypeEnum, ElementalInfoBean>();
        }
        dicElementalInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            var itemData = listData[i];
            dicElementalInfo.Add((ElementalTypeEnum)itemData.id, itemData);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    protected void InitBookModelInfo(List<BookModelInfoBean> listData)
    {
        this.listBookModelInfo = listData;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    protected void InitBookModelDetailsInfo(List<BookModelDetailsInfoBean> listData)
    {
        if (dicBookModelDetailsInfo == null)
        {
            dicBookModelDetailsInfo = new Dictionary<long, List<BookModelDetailsInfoBean>>();
        }
        dicBookModelDetailsInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BookModelDetailsInfoBean itemData = listData[i];
            if (dicBookModelDetailsInfo.TryGetValue(itemData.model_id, out List<BookModelDetailsInfoBean> listItemData))
            {
                listItemData.Add(itemData);
            }
            else
            {
                dicBookModelDetailsInfo.Add(itemData.model_id, new List<BookModelDetailsInfoBean>() { itemData });
            }
        }
    }

    /// <summary>
    /// 获取法器数据
    /// </summary>
    /// <returns></returns>
    public MagicInstrumentInfoBean GetMagicInstrumentInfo(int itemId)
    {
        if (dicMagicInstrumentInfo.TryGetValue(itemId, out MagicInstrumentInfoBean magicInstrumentData))
        {
            return magicInstrumentData;
        }
        return null;
    }


    /// <summary>
    /// 通过模块ID获取数据
    /// </summary>
    /// <param name="modelId"></param>
    public List<BookModelDetailsInfoBean> GetBookModelDetailsById(long modelId)
    {
        if (dicBookModelDetailsInfo.TryGetValue(modelId, out List<BookModelDetailsInfoBean> listData))
        {
            return listData;
        }
        return null;
    }

    /// <summary>
    /// 获取元素信息
    /// </summary>
    /// <param name="elementalType"></param>
    /// <returns></returns>
    public ElementalInfoBean GetElementalInfo(ElementalTypeEnum elementalType)
    {
        if (dicElementalInfo.TryGetValue(elementalType, out ElementalInfoBean elementalInfo))
        {
            return elementalInfo;
        }
        return null;
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

    public void GetElementalInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetElementalInfoFail(string failMsg, Action action)
    {
        action?.Invoke();
    }

    public void GetMagicInstrumentInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetMagicInstrumentInfoFail(string failMsg, Action action)
    {
    }
    #endregion
}