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
    public string icon_color;

    public int items_type;
    public int type_id; //�������͵�ID
    public int max_number;  //����������

    public string model_name;//ģ������
    public string model_remark_name;//����ģ������

    public string tex_name;//��ͼ����
    public float cd_use;//ʹ�ü��

    public string attribute_data;//��������
    public string range_damage;//�˺���Χ ��(����Ŀ��ľ���) ��������ȣ��ߣ������߶ȣ�
    public string anim_use;//ʹ�ö���
    public string hold_data;//��ס���ߵ����ݣ�λ����ת�ȣ�
    public string link_class;//�������ࣨ����ж�Ӧ�� ��Ҫ���������������ߣ�
    public string fire_items;//���߱�����֮������ݣ�����ID,����,ʱ�䣩

    public string sound_use;//����ʹ������

    public int elemental_metal;    //Ԫ��
    public int elemental_wood;
    public int elemental_water;
    public int elemental_fire;
    public int elemental_earth;

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
            listName.Add(itemData.GetName());
        }

        return listName;
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

    /// <summary>
    /// ��ȡ���Ƶ���Ʒ
    /// </summary>
    public void GetFireItems(out int[] fireItemsId, out int[] fireItemsNum, out int[] fireTime)
    {
        fireItemsId = null;
        fireItemsNum = null;
        fireTime = null;
        if (fire_items.IsNull())
            return;
        string[] itemsDataStr = fire_items.SplitForArrayStr('|');
        fireItemsId = new int[itemsDataStr.Length];
        fireItemsNum = new int[itemsDataStr.Length];
        fireTime = new int[itemsDataStr.Length];

        for (int i = 0; i < itemsDataStr.Length; i++)
        {
            int[] itemData = itemsDataStr[i].SplitForArrayInt(',');
            fireItemsId[i] = itemData[0];
            fireItemsNum[i] = itemData[1];
            fireTime[i] = itemData[2];
        }
    }

    /// <summary>
    /// ��ȡ����ģ������
    /// </summary>
    /// <param name="path"></param>
    public List<string> GetModelRemarkName(string path)
    {
        List<string> listName = new List<string>();
        if (model_remark_name.IsNull())
            return listName;
        string[] arrayName = model_remark_name.Split("|");
        for (int i = 0; i < arrayName.Length; i++)
        {
            var itemData = arrayName[i];
            listName.Add($"{path}/{itemData}.prefab");
        }
        return listName;
    }

    protected DamageBean damageData;//�˺�����
    /// <summary>
    /// ��ȡ�˺�����
    /// </summary>
    /// <returns></returns>
    public DamageBean GetDamageData()
    {
        if (damageData == null)
        {
            damageData = new DamageBean(attribute_data);
        }
        return damageData;
    }

    protected AttributeBean attributeData;
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public AttributeBean GetAttributeData()
    {
        if (attributeData == null)
        {
            attributeData = new AttributeBean(attribute_data);
        }
        return attributeData;
    }
   
}