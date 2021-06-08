/*
* FileName: BuildingInfo 
* Author: AppleCoffee 
* CreateTime: 2021-06-08-14:40:47 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildingInfoBean : BaseBean
{
    public string name;
    public string data;

    protected List<BuildingBean> _listBuildingData;

    public List<BuildingBean> listBuildingData
    {
        get
        {
            if (_listBuildingData == null)
            {
                DataStorageListBean<BuildingBean> dataStorageList = JsonUtil.FromJson<DataStorageListBean<BuildingBean>>(data);
                _listBuildingData = dataStorageList.listData;
            }
            return _listBuildingData;
        }
    }
 
    public void SetListBuildingData(List<BuildingBean> listData)
    {
        if (listData == null)
            return;
        this._listBuildingData = listData;
        DataStorageListBean<BuildingBean> dataStorageList = new DataStorageListBean<BuildingBean>();
        dataStorageList.listData = listData;
        data = JsonUtil.ToJson(dataStorageList);
    }
}