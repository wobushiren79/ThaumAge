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
    /// ����Ƿ��ܷ������������
    /// </summary>
    public bool CheckCanSetLinkLargeBuilding(Vector3Int basePosition, bool isCheckBase, out BlockDirectionEnum baseBlockDirection)
    {
        baseBlockDirection = BlockDirectionEnum.UpForward;
        if (basePosition.y < 0)
            return false;
        int rotateAngle = 0;
        bool canSet = false;
        while (rotateAngle < 360 && canSet == false)
        {
            canSet = true;
            for (int i = 0; i < listBuildingData.Count; i++)
            {
                var itemBuildingData = listBuildingData[i];
                Vector3 rotatePosition = VectorUtil.GetRotatedPosition(Vector3.zero, itemBuildingData.position, new Vector3(0, rotateAngle, 0)) + basePosition;
                Vector3Int itemWorldPosition = Vector3Int.RoundToInt(rotatePosition);
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
                if (!isCheckBase && itemBuildingData.position == Vector3Int.zero)
                {
                    continue;
                }
                if (itemChunk == null || itemBlock == null)
                {
                    canSet = false;
                    break;
                }
                if (itemBuildingData.blockId != itemBlock.blockInfo.id)
                {
                    canSet = false;
                    break;
                }
            }
            if (!canSet)
            {
                rotateAngle += 90;
            }
        }
        switch (rotateAngle)
        {
            case 0:
                baseBlockDirection = BlockDirectionEnum.UpForward;
                break;
            case 90:
                baseBlockDirection = BlockDirectionEnum.UpLeft;
                break;
            case 180:
                baseBlockDirection = BlockDirectionEnum.UpBack;
                break;
            case 270:
                baseBlockDirection = BlockDirectionEnum.UpRight;
                break;
        }

        return canSet;
    }

    /// <summary>
    /// ����������ͷ���
    /// </summary>
    public void SetLinkLargeBuilding(Vector3Int basePosition, BlockDirectionEnum baseBlockDirection)
    {
        int rotateAngle = 0;
        switch (baseBlockDirection)
        {
            case BlockDirectionEnum.UpForward:
                rotateAngle = 0;
                break;
            case BlockDirectionEnum.UpLeft:
                rotateAngle = 90;
                break;
            case BlockDirectionEnum.UpBack:
                rotateAngle = 180;
                break;
            case BlockDirectionEnum.UpRight:
                rotateAngle = 270;
                break;
        }

        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(basePosition, out Block baseBlock, out Chunk baseChunk);
        for (int i = 0; i < listBuildingData.Count; i++)
        {
            var itemBuildingData = listBuildingData[i];
            //�����0,0,0�� �ٷ��õ�ʱ���Ѿ����ù��� ����Ͳ������� ֻ���û�������
            if (itemBuildingData.position == Vector3Int.zero)
            {
                continue;
            }
            Vector3 rotatePosition = VectorUtil.GetRotatedPosition(Vector3.zero, itemBuildingData.position, new Vector3(0, rotateAngle, 0)) + basePosition;
            Vector3Int itemWorldPosition = Vector3Int.RoundToInt(rotatePosition);
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
            if (itemChunk == null || itemBlock == null)
            {
                return;
            }
            BlockMetaBaseLink blockMetaLinkData = new BlockMetaBaseLink();
            blockMetaLinkData.level = 1;
            blockMetaLinkData.linkBasePosition = new Vector3IntBean(basePosition);
            blockMetaLinkData.isBreakAll = false;
            blockMetaLinkData.isBreakMesh = false;
            blockMetaLinkData.baseBlockType = (int)baseBlock.blockInfo.id;
            itemChunk.SetBlockForLocal(itemWorldPosition - itemChunk.chunkData.positionForWorld, BlockTypeEnum.LinkLargeChild, baseBlockDirection, blockMetaLinkData.ToJson());
        }
    }

    /// <summary>
    /// ��ԭ�෽��ṹ��ԭ��
    /// </summary>
    public void ResetLinkLargeBuilding(Vector3Int basePosition, List<Vector3Int> nullPosition = null)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(basePosition, out Block baseBlock, out Chunk baseChunk);
        for (int i = 0; i < listBuildingData.Count; i++)
        {
            var itemBuildingData = listBuildingData[i];
            Vector3Int itemWorldPosition = itemBuildingData.position + basePosition;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
            if (nullPosition.Contains(itemWorldPosition))
            {
                continue;
            }
            //��������ڵ� Ҫɾ��ģ��
            if (itemBuildingData.position == Vector3Int.zero)
            {
                itemChunk.SetBlockForLocal(itemWorldPosition - itemChunk.chunkData.positionForWorld, (BlockTypeEnum)itemBuildingData.blockId, isDestoryOld: true);
                continue;
            }
            if (itemChunk == null || itemBlock == null)
            {
                continue;
            }
            itemChunk.SetBlockForLocal(itemWorldPosition - itemChunk.chunkData.positionForWorld, (BlockTypeEnum)itemBuildingData.blockId, isDestoryOld: false);
        }
    }
}