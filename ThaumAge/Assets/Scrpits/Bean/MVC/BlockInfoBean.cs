/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BlockInfoBean : BaseBean
{
    public int link_id;

    public string model_name;

    public int shape;//��״

    public string uv_position;//uvλ��

    public string offset_border;//��ƫ��

    public float weight;//����

    public int rotate_state;//�Ƿ�����ת 0������ת 1����ת 2ֻ��LRFB������ת

    public int life;//����ֵ

    public int material_type = 1;//��������

    public int collider_state;//�Ƿ���ײ 0û����ײ 1����ײ

    public int trigger_state;//�Ƿ��� 0û�д��� 1�д���

    public int plough_state;//����״̬ 0���ܸ��� 1�ܸ���

    public int plough_change;//���غ�ı�ķ���

    public int plant_state;//��ֲ״̬ 0������ֲ 1����ֲ

    public string items_drop;//����

    public string break_type;//ָ���ƻ�����

    public int interactive_state;//����״̬ 0���ɻ��� 1�ɻ���(F��)
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)id;
    }

    /// <summary>
    /// ��ȡ������״
    /// </summary>
    /// <returns></returns>
    public BlockShapeEnum GetBlockShape()
    {
        return (BlockShapeEnum)shape;
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public BlockMaterialEnum GetBlockMaterialType()
    {
        return (BlockMaterialEnum)material_type;
    }

    protected Vector2Int[] arrayUVData;

    /// <summary>
    /// ��ȡUV�����
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetUVPosition()
    {
        if (arrayUVData.IsNull())
        {
            if (uv_position.IsNull())
                return arrayUVData;
            string[] uvArrary = uv_position.SplitForArrayStr('|');
            arrayUVData = new Vector2Int[uvArrary.Length];
            for (int i = 0; i < uvArrary.Length; i++)
            {
                string uvItemStr = uvArrary[i];
                int[] uvPositionArray = uvItemStr.SplitForArrayInt(',');
                arrayUVData[i] = new Vector2Int(uvPositionArray[0], uvPositionArray[1]);
            }
        }
        return arrayUVData;
    }

    protected float[] offsetBorder;

    /// <summary>
    /// ��ȡƫ�Ʊ�����
    /// </summary>
    /// <returns></returns>
    public float[] GetOffsetBorder()
    {
        if (offsetBorder.IsNull())
        {
            offsetBorder = offset_border.SplitForArrayFloat('|');
        }
        return offsetBorder;
    }

    /// <summary>
    /// ��ȡ������������
    /// </summary>
    public MeshData GetBlockMeshData()
    {
        BlockTypeEnum blockType = GetBlockType();
        TextAsset textAsset = LoadAddressablesUtil.LoadAssetSync<TextAsset>($"Assets/Prefabs/BlockMeshData/Block{blockType.GetEnumName()}.txt");
        return JsonUtil.FromJson<MeshData>(textAsset.text);
    }

    /// <summary>
    /// ��ȡ���ߵ���
    /// </summary>
    /// <returns></returns>
    public List<ItemsBean> GetItemsDrop()
    {
        List<ItemsBean> itemsData = new List<ItemsBean>();
        string[] itemListStr = items_drop.SplitForArrayStr('|');
        for (int i = 0; i < itemListStr.Length; i++)
        {
            string[] itemDetailsListStr = itemListStr[i].SplitForArrayStr(',');
            long itemsId = long.Parse(itemDetailsListStr[0]);
            int itemsNumber = 1;
            float getRandomRate = 1;
            if (itemDetailsListStr.Length == 1)
            {

            }
            else if (itemDetailsListStr.Length == 2)
            {
                itemsNumber = int.Parse(itemDetailsListStr[1]);
            }
            else if (itemDetailsListStr.Length == 3)
            {
                itemsNumber = int.Parse(itemDetailsListStr[1]);
                getRandomRate = float.Parse(itemDetailsListStr[2]);
                if (UnityEngine.Random.Range(0f, 1f) > getRandomRate) 
                {
                    //��������0��������ֱ�Ӳ����� ����Ϊ������ص�listû�����ݵĻ��������ɱ���ĵ��䡣���ó�0��ֱ�Ӳ�������Ʒ
                    itemsId = 0;
                }
            }
            itemsData.Add(new ItemsBean(itemsId, itemsNumber));
        }
        return itemsData;
    }

    /// <summary>
    /// ��ȡ�ƻ�����
    /// </summary>
    /// <returns></returns>
    public List<BlockBreakTypeEnum> GetBreakType()
    {
        List<BlockBreakTypeEnum> listData = new List<BlockBreakTypeEnum>();
        if (break_type.IsNull())
        {
            listData.Add(BlockBreakTypeEnum.Normal);
        }
        else
        {
            int[] typeArray = break_type.SplitForArrayInt(',');
            for (int i = 0; i < typeArray.Length; i++)
            {
                BlockBreakTypeEnum itemType = (BlockBreakTypeEnum)typeArray[i];
                listData.Add(itemType);
            }
        }
        return listData;
    }

    /// <summary>
    /// ����Ƿ��ܱ��ƻ�
    /// </summary>
    /// <returns></returns>
    public bool CheckCanBreak(long breakItemId)
    {
        List<BlockBreakTypeEnum> listBreakType = GetBreakType();
        for (int i = 0; i < listBreakType.Count; i++)
        {
            BlockBreakTypeEnum blockBreakType = listBreakType[i];
            if (blockBreakType == BlockBreakTypeEnum.NotBreak)
            {
                return false;
            }
            if (blockBreakType == BlockBreakTypeEnum.Normal)
            {
                return true;
            }
            else
            {
                //����ǿ���
                if (breakItemId == 0) 
                {
                  
                }
                else
                {
                    ItemsInfoBean useItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(breakItemId);
                    if (useItemInfo.GetItemsType()== (ItemsTypeEnum)listBreakType[i])
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}