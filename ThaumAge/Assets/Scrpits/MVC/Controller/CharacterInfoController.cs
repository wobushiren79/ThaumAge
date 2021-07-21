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
    public void GetAllCharacterInfoData(Action<List<CharacterInfoHairBean>> action)
    {
        List<CharacterInfoHairBean> listData = GetModel().GetAllCharacterInfoHairData();
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetCharacterInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetCharacterInfoSuccess(listData, action);
        }
    }

} 