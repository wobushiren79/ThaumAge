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

    public float weight;//����

    public int rotate_state;//�Ƿ�����ת 0������ת 1����ת

    public int life;//����ֵ

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

    protected List<Vector2Int> listUVData = new List<Vector2Int>();
    /// <summary>
    /// ��ȡUV�����
    /// </summary>
    /// <returns></returns>
    public List<Vector2Int> GetUVPosition()
    {
        if (listUVData.IsNull())
        {
            listUVData = new List<Vector2Int>();
            if (uv_position.IsNull())
                return listUVData;
            string[] uvArrary = StringUtil.SplitBySubstringForArrayStr(uv_position, '|');
            for (int i = 0; i < uvArrary.Length; i++)
            {
                string uvItemStr = uvArrary[i];
                int[] uvPositionArray = StringUtil.SplitBySubstringForArrayInt(uvItemStr, ',');
                listUVData.Add(new Vector2Int(uvPositionArray[0], uvPositionArray[1]));
            }
        }
        return listUVData;
    }
}