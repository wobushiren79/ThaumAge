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
    public string data;

    [NonSerialized]
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

    /// <summary>
    /// 检测是否能放置下这个建筑
    /// </summary>
    public bool CheckCanSetLinkLargeBuilding(Vector3Int basePosition)
    {
        if (basePosition.y < 0)
            return false;
        for (int i = 0; i < listBuildingData.Count; i++)
        {
            var itemBuildingData = listBuildingData[i];
            Vector3Int itemWorldPosition = itemBuildingData.position + basePosition;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
            if (itemChunk == null || itemBlock == null)
            {
                return false;
            }
            if (itemBuildingData.blockId != itemBlock.blockInfo.id)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 放置这个大型方块
    /// </summary>
    public void SetLinkLargeBuilding(Vector3Int basePosition)
    {
        for (int i = 0; i < listBuildingData.Count; i++)
        {    
            var itemBuildingData = listBuildingData[i];
            //如果是0,0,0处 再放置的时候已经设置过了 这里就不处理了
            if (itemBuildingData.position == Vector3Int.zero)
                continue;
            Vector3Int itemWorldPosition = itemBuildingData.position + basePosition;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
            if (itemChunk == null || itemBlock == null)
            {
                return;
            }
            itemChunk.SetBlockForLocal(itemWorldPosition - itemChunk.chunkData.positionForWorld, BlockTypeEnum.None);
        }
    }
}