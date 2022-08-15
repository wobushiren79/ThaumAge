/*
* FileName: AudioInfo 
* Author: AppleCoffee 
* CreateTime: 2022-08-15-18:21:16 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioInfoModel : BaseMVCModel
{
    protected AudioInfoService serviceAudioInfo;

    public override void InitData()
    {
        serviceAudioInfo = new AudioInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<AudioInfoBean> GetAllAudioInfoData()
    {
        List<AudioInfoBean> listData = serviceAudioInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public AudioInfoBean GetAudioInfoData()
    {
        AudioInfoBean data = serviceAudioInfo.QueryData();
        if (data == null)
            data = new AudioInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<AudioInfoBean> GetAudioInfoDataById(long id)
    {
        List<AudioInfoBean> listData = serviceAudioInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetAudioInfoData(AudioInfoBean data)
    {
        serviceAudioInfo.UpdateData(data);
    }

}