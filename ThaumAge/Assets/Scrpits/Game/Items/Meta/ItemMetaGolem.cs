using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemMetaGolem : ItemBaseMeta
{
    //材质
    public int material;
    //头
    public int head;
    //手
    public int hand;
    //脚
    public int foot;
    //附件
    public int accessory;
    //傀儡核心
    public List<ItemsBean> listGolemCore = new List<ItemsBean>() { new ItemsBean(0, 1), new ItemsBean(0, 1), new ItemsBean(0, 1) };

    //背包数据
    public ItemMetaBag bagData = new ItemMetaBag(3);

    /// <summary>
    /// 获取活动范围
    /// </summary>
    /// <returns></returns>
    public float GetWorkRange()
    {
        return 5;
    }

    /// <summary>
    /// 获取核心
    /// </summary>
    public void GetGolemCore(int itemId, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore)
    {
        itemGolemCore = null;
        itemMetaGolemCore = null;
        for (int i = 0; i < listGolemCore.Count; i++)
        {
            ItemsBean itemData = listGolemCore[i];
            if (itemId == itemData.itemId)
            {
                itemGolemCore = itemData;
                itemMetaGolemCore = itemData.GetMetaData<ItemMetaGolemCore>();
            }
        }
    }

    /// <summary>
    /// 检测核心
    /// </summary>
    /// <param name="aiGolemEntity"></param>
    /// <param name="coreItemId"></param>
    /// <param name="itemGolemCore"></param>
    /// <param name="itemMetaGolemCore"></param>
    /// <returns></returns>
    public bool CheckGolemCore(AIGolemEntity aiGolemEntity, ItemsBean itemGolemCore, ItemMetaGolemCore itemMetaGolemCore)
    {
        //如果没有这个核心
        if (itemGolemCore == null || itemMetaGolemCore == null || itemMetaGolemCore.itemId == 0)
        {
            return false;
        }
        //如果核心没有绑定位置
        if (itemMetaGolemCore.bindBlockWorldPosition.y == int.MaxValue)
        {
            return false;
        }
        return true;
    }
}