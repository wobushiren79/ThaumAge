/*
* FileName: AudioInfo 
* Author: AppleCoffee 
* CreateTime: 2022-08-15-18:21:16 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class AudioInfoService : BaseDataRead<AudioInfoBean>
{
    protected readonly string saveFileName;

    public AudioInfoService()
    {
        saveFileName = "AudioInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<AudioInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<AudioInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public AudioInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(AudioInfoBean data)
    {

    }
}