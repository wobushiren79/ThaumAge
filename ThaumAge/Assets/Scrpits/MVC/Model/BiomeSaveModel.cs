/*
* FileName: BiomeSave 
* Author: AppleCoffee 
* CreateTime: 2022-11-01-14:57:35 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BiomeSaveModel : BaseMVCModel
{
    protected BiomeSaveService serviceBiomeSave;

    public override void InitData()
    {
        serviceBiomeSave = new BiomeSaveService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BiomeSaveBean> GetAllBiomeSaveData()
    {
        List<BiomeSaveBean> listData = serviceBiomeSave.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BiomeSaveBean GetBiomeSaveData(string userId, WorldTypeEnum worldType)
    {
        BiomeSaveBean biomeSaveData = serviceBiomeSave.QueryData(userId, worldType);
        if (biomeSaveData == null)
        {
            biomeSaveData = new BiomeSaveBean();
            biomeSaveData.userId = userId;
            biomeSaveData.worldType = (int)worldType;
        }
        return biomeSaveData;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BiomeSaveBean> GetBiomeSaveDataById(long id)
    {
        List<BiomeSaveBean> listData = serviceBiomeSave.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBiomeSaveData(BiomeSaveBean data)
    {
        serviceBiomeSave.UpdateData(data);
    }

}