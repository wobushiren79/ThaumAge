using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseDataController : BaseMVCController<BaseDataModel, IBaseDataView>
{
    protected Dictionary<long, BaseInfoBean> dicBaseInfoData;

    public BaseDataController(BaseMonoBehaviour content, IBaseDataView view) : base(content, view)
    {
        InitAllBaseData();
    }
    public override void InitData()
    {

    }

    public void InitAllBaseData()
    {
        dicBaseInfoData = new Dictionary<long, BaseInfoBean>();
        List<BaseInfoBean> listData = GetModel().GetAllBaseData();
        for (int i = 0; i < listData.Count; i++)
        {
            BaseInfoBean itemData = listData[i];
            dicBaseInfoData.Add(itemData.id, itemData);
        }
    }

    public BaseInfoBean GetBaseData(long baseInfoId)
    {
        if (dicBaseInfoData == null)
            return null;
        if (dicBaseInfoData.TryGetValue(baseInfoId, out BaseInfoBean baseData))
        {
            return baseData;
        }
        return null;
    }
}