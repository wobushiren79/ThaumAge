using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITextService : BaseDataRead<UITextBean>
{

    /// <summary>
    /// 查询所有场景数据
    /// </summary>
    /// <returns></returns>
    public List<UITextBean> QueryAllData()
    {
        return BaseLoadDataForList("UIText");
    }
}