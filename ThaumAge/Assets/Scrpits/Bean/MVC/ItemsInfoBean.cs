/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class ItemsInfoBean : BaseBean
{
    public string icon_key;

    public int items_type;
    public int type_id; //�������͵�ID
    public int max_number;  //����������
    public string model_name;//ģ������
    public string tex_name;//��ͼ����
    public int life;//�;ö�

    public float cd_use;//ʹ�ü��

    public int atk;//������

    public ItemsTypeEnum GetItemsType()
    {
        return (ItemsTypeEnum)items_type;
    }

    /// <summary>
    /// ��ȡ�����б�
    /// </summary>
    /// <param name="listCharacterInfo"></param>
    /// <returns></returns>
    public static List<string> GetNameList(List<ItemsInfoBean> listInfo)
    {
        List<string> listName = new List<string>(listInfo.Count);

        for (int i = 0; i < listInfo.Count; i++)
        {
            ItemsInfoBean itemData = listInfo[i];
            listName.Add(itemData.name);
        }

        return listName;
    }
}