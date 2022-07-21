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


    //�Ƿ�����ת 
    //0������ת(��Զֻ��UpForward)
    //1����ת(��������)
    //2ֻ��LRFB������ת(ֻ���� UpLeft,UpRight,UpForward,UpBack��4������)
    //3ֻ��UD ����ֻ����Up,Down��
    public int rotate_state;

    public int life;//����ֵ

    public int material_type = 1;//��������
    public int material_type2 = 1;//��������

    public int collider_state;//�Ƿ���ײ 0û����ײ 1����ײ

    public int trigger_state;//�Ƿ��� 0û�д��� 1�д���

    public int plough_state;//����״̬ 0���ܸ��� 1�ܸ���

    public int plant_state;//��ֲ״̬ 0������ֲ 1����ֲ

    public string items_drop;//����

    public string break_type;//ָ���ƻ�����

    public int interactive_state;//����״̬ 0���ɻ��� 1�ɻ���(F��)

    public int remark_int;//��עint

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
    public BlockMaterialEnum GetBlockMaterialType2()
    {
        return (BlockMaterialEnum)material_type2;
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
    public MeshDataCustom GetBlockMeshData()
    {
        BlockTypeEnum blockType = GetBlockType();
        TextAsset textAsset = LoadAddressablesUtil.LoadAssetSync<TextAsset>($"Assets/Prefabs/BlockMeshData/Block{blockType.GetEnumName()}.txt");
        return JsonUtil.FromJson<MeshDataCustom>(textAsset.text);
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