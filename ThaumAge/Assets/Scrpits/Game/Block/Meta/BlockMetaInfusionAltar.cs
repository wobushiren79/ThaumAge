using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaInfusionAltar : BlockMetaBaseLink 
{
    //展示的物品
    public ItemsBean itemsShow;
    //注魔执行失败时间（超过5秒就失败）
    public int infusionFailTime = 0;
    //注魔执行剩余材料
    public List<long> listInfusionMat;
    //注魔执行剩余元素
    public List<NumberBean> listInfusionElemental;
    //当前注魔吸取元素位置
    public Vector3Int curInfusionElementalPosition;
    //注魔流程 0未开始 1吸取元素 2吸取材料
    public int infusionPro = 0;

    //注魔完成的道具和数据
    public long infusionSuccessItemId;
    public long infusionSuccessItemNum;
    public void InitData(InfusionAltarInfoBean infusionAltarInfo)
    {
        infusionPro = 0;
        infusionFailTime = 0;
        if (infusionAltarInfo == null)
            return;
        listInfusionMat = infusionAltarInfo.GetMaterials();
        listInfusionElemental = infusionAltarInfo.GetElementals();

        var beforeItemData = infusionAltarInfo.item_after.SplitForArrayLong(':');

        infusionSuccessItemId = beforeItemData[0];
        if (beforeItemData.Length == 1)
        {
            infusionSuccessItemNum = 1;
        }
        else
        {
            infusionSuccessItemNum = beforeItemData[1];
        }
    }
}