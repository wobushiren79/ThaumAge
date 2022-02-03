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

    public string range_damage;//�˺���Χ �����

    public string anim_use;//ʹ�ö���

    public string hold_data;//�� ����
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


    /// <summary>
    /// ��ȡ�˺���Χ
    /// </summary>
    /// <param name="length"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void GetRangeDamage(out float length, out float width, out float height)
    {
        length = 0;
        width = 0;
        height = 0;

        float[] data = range_damage.SplitForArrayFloat(',');
        if (data.Length >= 1)
        {
            length = data[0];
        }
        if (data.Length >= 2)
        {
            width = data[1];
        }
        if (data.Length >= 3)
        {
            height = data[2];
        }
    }

    /// <summary>
    /// ��ȡ��ס����
    /// </summary>
    /// <param name="rotate"></param>
    public bool GetHoldData(out Vector3 rotate)
    {
        if (hold_data.IsNull())
        {
            rotate = Vector3.zero;
            return false;
        }
        float[] rotateData = hold_data.SplitForArrayFloat(',');
        rotate = new Vector3(rotateData[0], rotateData[1], rotateData[2]);
        return true;
    }
}