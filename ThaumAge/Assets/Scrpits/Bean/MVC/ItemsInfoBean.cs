/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ItemsInfoBean : BaseBean
{
    public int link_id;
    public string icon_key;
    public string name;
    public int items_type;
    public int type_id; //关联类型的ID
    public int max_number;  //最大格子数量

    public ItemsTypeEnum GetItemsType()
    {
        return (ItemsTypeEnum)items_type;
    }

}