using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaInfusionAltar : BlockMetaBaseLink 
{
    //展示的物品
    public ItemsBean itemsShow;
    //注魔执行时间
    public int infusionTime = 0;
    //注魔执行剩余材料
    public List<long> listInfusionMat;
    //注魔执行剩余元素
    public List<NumberBean> listInfusionElemental;

    public void InitData(InfusionAltarInfoBean infusionAltarInfo)
    {   
        infusionTime = 0;
        if (infusionAltarInfo == null)
            return;
        listInfusionMat = infusionAltarInfo.GetMaterials();
        listInfusionElemental = infusionAltarInfo.GetElementals();
    }
}