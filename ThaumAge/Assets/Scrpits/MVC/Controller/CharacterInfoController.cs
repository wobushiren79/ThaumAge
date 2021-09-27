/*
* FileName: CharacterInfo 
* Author: AppleCoffee 
* CreateTime: 2021-07-21-17:20:11 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterInfoController : BaseMVCController<CharacterInfoModel, ICharacterInfoView>
{

    public CharacterInfoController(BaseMonoBehaviour content, ICharacterInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllCharacterInfoHairData(Action<List<CharacterInfoBean>> action)
    {
        List<CharacterInfoBean> listData = GetModel().GetAllCharacterInfoHairData();
        if (listData.IsNull())
        {
            GetView().GetCharacterInfoFail("没有头发数据", null);
        }
        else
        {
            GetView().GetCharacterInfoSuccess(listData, action);
        }
    }

    public void GetAllCharacterInfoEyeData(Action<List<CharacterInfoBean>> action)
    {
        List<CharacterInfoBean> listData = GetModel().GetAllCharacterInfoEyeData();
        if (listData.IsNull())
        {
            GetView().GetCharacterInfoFail("没有眼部数据", null);
        }
        else
        {
            GetView().GetCharacterInfoSuccess(listData, action);
        }
    }

    public void GetAllCharacterInfoMouthData(Action<List<CharacterInfoBean>> action)
    {
        List<CharacterInfoBean> listData = GetModel().GetAllCharacterInfoMouthData();
        if (listData.IsNull())
        {
            GetView().GetCharacterInfoFail("没有嘴巴数据", null);
        }
        else
        {
            GetView().GetCharacterInfoSuccess(listData, action);
        }
    }

    public void GetAllCharacterInfoSkinData(Action<List<CharacterInfoBean>> action)
    {
        List<CharacterInfoBean> listData = GetModel().GetAllCharacterInfoSkinData();
        if (listData.IsNull())
        {
            GetView().GetCharacterInfoFail("没有皮肤数据", null);
        }
        else
        {
            GetView().GetCharacterInfoSuccess(listData, action);
        }
    }
}