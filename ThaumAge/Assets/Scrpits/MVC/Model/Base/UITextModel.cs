using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITextModel : BaseMVCModel
{
    protected UITextService serviceUIText;

    public override void InitData()
    {
        serviceUIText = new UITextService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<UITextBean> GetAllData()
    {
        List<UITextBean> listData = serviceUIText.QueryAllData();
        return listData;
    }

}