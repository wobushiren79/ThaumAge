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
    public List<ItemsBean> listGolemCore = new List<ItemsBean>() { new ItemsBean(0,1) };

    //背包数据
    public ItemMetaBag bagData = new ItemMetaBag(3);
}