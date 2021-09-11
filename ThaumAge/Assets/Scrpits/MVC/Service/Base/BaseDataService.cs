using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataService : BaseDataRead<BaseInfoBean>
{

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BaseInfoBean> QueryAllData()
    {
        List<BaseInfoBean> listData = BaseLoadDataForList($"{dataStoragePath}.txt");
        return listData;
    }

}