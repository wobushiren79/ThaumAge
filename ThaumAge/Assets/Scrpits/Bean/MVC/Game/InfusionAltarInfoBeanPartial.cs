using System;
using System.Collections.Generic;
using UnityEngine;

public partial class InfusionAltarInfoBean
{
    protected List<NumberBean> listElemental;
    protected List<long> listMaterial;

    /// <summary>
    /// 获取所有材质
    /// </summary>
    public List<long> GetMaterials()
    {
        if (listMaterial == null)
        {
            listMaterial = materials.SplitForListLong('&');
        }
        return listMaterial;
    }

    /// <summary>
    /// 获取所有元素
    /// </summary>
    /// <returns></returns>
    public List<NumberBean> GetElementals()
    {
        if (listElemental == null)
        {
            listElemental = NumberBean.GetListNumberBean(elements);
        }
        return listElemental;
    }

    /// <summary>
    /// 检测材料是否包含该道具
    /// </summary>
    public bool CheckMaterials(long itemId)
    {
        List<long> allMat = GetMaterials();
        if (allMat.Contains(itemId))
        {
            return true;
        }
        return false;
    }
}
public partial class InfusionAltarInfoCfg
{

    /// <summary>
    /// 检测是否能注魔
    /// </summary>
    /// <param name="itemId"></param>
    public static bool CheckCanInfusion(long itemId)
    {
        var allData = GetAllData();
        foreach (var itemData in allData.Values)
        {
            GetItemData(itemData.item_before, out int beforeItemId, out int beforeItemNum);
            if (beforeItemId == itemId)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取注魔信息
    /// </summary>
    /// <returns></returns>
    public static List<InfusionAltarInfoBean> GetInfusionAltarInfo(long itemId)
    {
        List<InfusionAltarInfoBean> listData = new List<InfusionAltarInfoBean>();
        var allData = GetAllData();
        foreach (var itemData in allData.Values)
        {
            GetItemData(itemData.item_before, out int beforeItemId, out int beforeItemNum);
            if (beforeItemId == itemId)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 获取注魔的数据
    /// </summary>
    /// <returns></returns>
    public static InfusionAltarInfoBean GetInfusionTargetData(long itemId, Chunk targetChunk, Vector3Int basePosition)
    {
        //获取该道具所有的注魔信息
        List<InfusionAltarInfoBean> listInfusionAltarData = GetInfusionAltarInfo(itemId);

        targetChunk.GetBlockForLocal(basePosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out targetChunk);

        InfusionAltarInfoBean targetData = null;

        targetBlock.GetRoundBlock(basePosition + targetChunk.chunkData.positionForWorld, BlockTypeInfusionAltar.RangeMaterial, BlockTypeInfusionAltar.RangeMaterial, BlockTypeInfusionAltar.RangeMaterial, (itemChunk, itemBlock, itemLocalPosition) =>
        {
            //获取基座上的物品
            if (itemBlock.blockType == BlockTypeEnum.ArcanePedestal)
            {
                itemBlock.GetBlockMetaData(itemChunk, itemLocalPosition, out BlockBean itemBlockData, out BlockMetaArcanePedestal blockMetaArcanePedestal);
                if (blockMetaArcanePedestal.itemsShow != null && blockMetaArcanePedestal.itemsShow.itemId != 0)
                {
                    for (int i = 0; i < listInfusionAltarData.Count; i++)
                    {
                        InfusionAltarInfoBean itemInfusionAltarInfoData = listInfusionAltarData[i];
                        //检测是否有这个道具
                        bool checkMat = itemInfusionAltarInfoData.CheckMaterials(blockMetaArcanePedestal.itemsShow.itemId);
                        if (checkMat)
                        {
                            targetData = itemInfusionAltarInfoData;
                            break;
                        }
                    }
                }
            }
        });
        return targetData;
    }

    protected static void GetItemData(string data, out int itemId, out int itemNum)
    {
        int[] dataArray = data.SplitForArrayInt(':');
        itemId = dataArray[0];
        if (dataArray.Length > 1)
        {
            itemNum = dataArray[1];
        }
        else
        {
            itemNum = 1;
        }
    }
}
