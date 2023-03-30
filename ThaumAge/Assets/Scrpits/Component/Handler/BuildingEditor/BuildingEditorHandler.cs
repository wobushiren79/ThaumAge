using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingEditorHandler : BaseHandler<BuildingEditorHandler, BuildingEditorManager>
{
    /// <summary>
    /// 建造方块
    /// </summary>
    /// <param name="blockPosition"></param>
    public void BuildBlock(Vector3Int blockPosition,int blockId,BlockDirectionEnum blockDirection)
    {
        if (manager.curCreateType == 0)
        {
            //放置方块
            //先删除有的
            if (manager.dicBlockBuild.TryGetValue(blockPosition, out BuildingEditorModel blockEditor))
            {
                Destroy(blockEditor.gameObject);
                manager.dicBlockBuild.Remove(blockPosition);
            }

            GameObject objItem = Instantiate(manager.objBlockContainer, manager.objBlockModel.gameObject);
            objItem.transform.position = blockPosition;
            BuildingEditorModel itemBlock = objItem.GetComponent<BuildingEditorModel>();
            BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockId);
            itemBlock.SetData(blockInfo, blockDirection);
            manager.dicBlockBuild.Add(blockPosition, itemBlock);
        }
        else if (manager.curCreateType == 1)
        {
            //移除方块
            if (manager.dicBlockBuild.TryGetValue(blockPosition, out BuildingEditorModel blockEditor))
            {
                Destroy(blockEditor.gameObject);
                manager.dicBlockBuild.Remove(blockPosition);
            }
        }

    }

    /// <summary>
    /// 建造建筑
    /// </summary>
    public void BuildBuilding(BuildingInfoBean buildingInfo)
    {
        List<BuildingBean> buildingBeans = buildingInfo.listBuildingData;
        for (int i = 0; i < buildingBeans.Count; i++)
        {
            BuildingBean itemData = buildingBeans[i];
            //纠错处理
            if (itemData.direction < 10)
            {
                itemData.direction = (int)BlockDirectionEnum.UpForward;
            }
            BuildBlock(itemData.position, itemData.blockId, (BlockDirectionEnum)itemData.direction);
        }
    }

    /// <summary>
    /// 清除所有方块
    /// </summary>
    public void ClearAllBlock()
    {
        manager.objBlockContainer.transform.DestroyAllChild();
        manager.dicBlockBuild.Clear();
    }

    /// <summary>
    /// 保存建筑数据
    /// </summary>
    /// <param name="buildingInfo"></param>
    public void SaveBuildingData(int buildId,string buildName)
    {
        //获取场中的方块数据 设置
        List<BuildingBean> listBlockData = new List<BuildingBean>();
        foreach (var itemData in manager.dicBlockBuild)
        {
            BuildingEditorModel blockEditor = itemData.Value;
            BuildingBean itemBlockData = new BuildingBean();
            itemBlockData.blockId = (int)blockEditor.blockInfo.id;
            itemBlockData.direction = (int)blockEditor.blockDirection;
            itemBlockData.position = itemData.Key;
            itemBlockData.randomRate = blockEditor.randomRate;
            listBlockData.Add(itemBlockData);
        }
        manager.curBuildingInfo.SetListBuildingData(listBlockData);
        manager.curBuildingInfo.id = buildId;
        manager.curBuildingInfo.name_cn = buildName;
        manager.curBuildingInfo.name_en = buildName;
        manager.curBuildingInfo.valid = 1;
        manager.controllerForBuildingInfo.SetBuildingInfoData(manager.curBuildingInfo);
    }

    /// <summary>
    /// 加载建筑
    /// </summary>
    public void LoadBuilding(long buildingId,Action<BuildingInfoBean> callBack)
    {
        manager.controllerForBuildingInfo.GetBuildingInfoDataById(buildingId, (data) =>
        {
            manager.curBuildingInfo = data;
            BuildBuilding(data);
            callBack?.Invoke(data);
        });
    }
}