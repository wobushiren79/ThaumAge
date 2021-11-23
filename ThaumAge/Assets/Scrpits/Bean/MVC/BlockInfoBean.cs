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

    public int rotate_state;//�Ƿ�����ת 0������ת 1����ת

    public int life;//����ֵ

    public int material_type = 1;//��������

    public int collider_state;//�Ƿ���ײ 0û����ײ 1����ײ

    public int trigger_state;//�Ƿ��� 0û�д��� 1�д���

    public int plough_state;//����״̬ 0���ܸ��� 1�ܸ���

    public int plough_change;//���غ�ı�ķ���

    public int plant_state;//��ֲ״̬ 0������ֲ 1����ֲ

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
            string[] uvArrary = StringUtil.SplitBySubstringForArrayStr(uv_position, '|');
            arrayUVData = new Vector2Int[uvArrary.Length];
            for (int i = 0; i < uvArrary.Length; i++)
            {
                string uvItemStr = uvArrary[i];
                int[] uvPositionArray = StringUtil.SplitBySubstringForArrayInt(uvItemStr, ',');
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
            offsetBorder = StringUtil.SplitBySubstringForArrayFloat(offset_border, '|');
        }
        return offsetBorder;
    }

}