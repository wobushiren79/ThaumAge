using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataService : BaseMVCService
{
    public BaseDataService() : base("base_data", "base_data")
    {

    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BaseDataBean> QueryAllData()
    {
        List<BaseDataBean> listData = BaseQueryAllData<BaseDataBean>();
        return listData;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(BaseDataBean data)
    {
        bool deleteState = BaseDeleteDataById(data.id);
        if (deleteState)
        {
            bool insertSuccess = BaseInsertData(tableNameForMain, data);
            return insertSuccess;
        }
        return false;
    }
}