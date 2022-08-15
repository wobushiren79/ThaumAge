/*
* FileName: AudioInfo 
* Author: AppleCoffee 
* CreateTime: 2022-08-15-18:21:16 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioInfoController : BaseMVCController<AudioInfoModel, IAudioInfoView>
{

    public AudioInfoController(BaseMonoBehaviour content, IAudioInfoView view) : base(content, view)
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
    public AudioInfoBean GetAudioInfoData(Action<AudioInfoBean> action)
    {
        AudioInfoBean data = GetModel().GetAudioInfoData();
        if (data == null) {
            GetView().GetAudioInfoFail("没有数据",null);
            return null;
        }
        GetView().GetAudioInfoSuccess<AudioInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllAudioInfoData(Action<List<AudioInfoBean>> action)
    {
        List<AudioInfoBean> listData = GetModel().GetAllAudioInfoData();
        if (listData.IsNull())
        {
            GetView().GetAudioInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetAudioInfoSuccess<List<AudioInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAudioInfoDataById(long id,Action<AudioInfoBean> action)
    {
        List<AudioInfoBean> listData = GetModel().GetAudioInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetAudioInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetAudioInfoSuccess(listData[0], action);
        }
    }
} 