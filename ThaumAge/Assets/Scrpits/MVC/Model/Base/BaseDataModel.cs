using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataModel : BaseMVCModel
{
    protected BaseDataService serviceBaseData;

    public override void InitData()
    {
        serviceBaseData = new BaseDataService();
    }

    public List<BaseDataBean> GetAllBaseData()
    {
        return serviceBaseData.QueryAllData();
    }
}