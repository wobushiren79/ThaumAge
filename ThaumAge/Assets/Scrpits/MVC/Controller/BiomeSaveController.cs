/*
* FileName: BiomeSave 
* Author: AppleCoffee 
* CreateTime: 2022-11-01-14:57:35 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeSaveController : BaseMVCController<BiomeSaveModel, IBiomeSaveView>
{

    public BiomeSaveController(BaseMonoBehaviour content, IBiomeSaveView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public BiomeSaveBean GetBiomeSaveData(string userId, WorldTypeEnum worldType,Action<BiomeSaveBean> action)
    {
        BiomeSaveBean data = GetModel().GetBiomeSaveData(userId, worldType);
        if (data == null) {
            GetView().GetBiomeSaveFail("没有数据",null);
            return null;
        }
        GetView().GetBiomeSaveSuccess<BiomeSaveBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBiomeSaveData(Action<List<BiomeSaveBean>> action)
    {
        List<BiomeSaveBean> listData = GetModel().GetAllBiomeSaveData();
        if (listData.IsNull())
        {
            GetView().GetBiomeSaveFail("没有数据", null);
        }
        else
        {
            GetView().GetBiomeSaveSuccess<List<BiomeSaveBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBiomeSaveDataById(long id,Action<BiomeSaveBean> action)
    {
        List<BiomeSaveBean> listData = GetModel().GetBiomeSaveDataById(id);
        if (listData.IsNull())
        {
            GetView().GetBiomeSaveFail("没有数据", null);
        }
        else
        {
            GetView().GetBiomeSaveSuccess(listData[0], action);
        }
    }

    /// <summary>
    /// 保存世界数据
    /// </summary>
    /// <param name="worldData"></param>
    /// <param name="action"></param>
    public void SetBiomeSaveData(BiomeSaveBean biomeSaveData, Action<BiomeSaveBean> action)
    {
        GetModel().SetBiomeSaveData(biomeSaveData);
    }
} 