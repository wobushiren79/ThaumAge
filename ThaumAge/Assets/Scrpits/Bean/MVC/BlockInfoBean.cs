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

    public string color;//��ɫ

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

    public string break_level;//ָ���ƻ��ȼ�

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
    /// ��ȡ������ɫ
    /// </summary>
    /// <returns></returns>
    public Color GetBlockColor()
    {
        if (ColorUtility.TryParseHtmlString($"#{color}", out Color colorBlock))
        {
            return colorBlock;
        }
        return Color.white;
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
        TextAsset textAsset;
        try
        {
             textAsset = LoadAddressablesUtil.LoadAssetSync<TextAsset>($"Assets/Prefabs/BlockMeshData/Block{blockType.GetEnumName()}.txt");
        }
        catch (Exception e)
        {
            textAsset = null;
            LogUtil.LogError("����BlockMeshʧ�ܣ�" + e.Message);
        }
        return JsonUtil.FromJson<MeshDataCustom>(textAsset.text);
    }

    /// <summary>
    /// ��ȡ�ƻ�����(Ĭ��ʹ�õ��ߵ����� ������������⴦��)
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
    ///  ����Ƿ��ܱ��ƻ�
    /// </summary>
    /// <param name="breakItemId"></param>
    /// <param name="canBreak">�Ƿ����ھ�</param>
    /// <param name="isAdditionBreak">�Ƿ������ھ�ӳ�</param>
    public void CheckCanBreak(long breakItemId, out bool canBreak, out bool isAdditionBreak)
    {
        canBreak = false;
        isAdditionBreak = false;
        List<BlockBreakTypeEnum> listBreakType = GetBreakType();

        //����Ŀ�귽����Ա���������ƻ�
        if (listBreakType.Contains(BlockBreakTypeEnum.Normal))
        {
            //����ǿ���
            if (breakItemId == 0)
            {
                canBreak = true;
                isAdditionBreak = false;
                return;
            }
            //������ǿ���
            else
            {
                ItemsInfoBean useItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(breakItemId);
                AttributeBean attributeData = useItemInfo.GetAttributeData();
                int itemBreakLevel = attributeData.GetAttributeValue(AttributeTypeEnum.BreakLevel);
                ItemsTypeEnum itemsType = useItemInfo.GetItemsType();
                for (int i = 0; i < listBreakType.Count; i++)
                {
                    //����ǵ�ǰʹ�õ���
                    if (itemsType == (ItemsTypeEnum)listBreakType[i])
                    {
                        canBreak = true;
                        isAdditionBreak = true;
                        return;
                    }
                }
                canBreak = true;
                isAdditionBreak = false;
                return;
            }
        }
        //Ŀ�귽�鲻�ܱ��ƻ�
        else if (listBreakType.Contains(BlockBreakTypeEnum.NotBreak))
        {
            canBreak = false;
            isAdditionBreak = false;
        }
        //Ŀ�귽��ֻ�ܱ�ָ�������ƻ�
        else
        {
            for (int i = 0; i < listBreakType.Count; i++)
            {
                BlockBreakTypeEnum blockBreakType = listBreakType[i];
                ItemsInfoBean useItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(breakItemId);
                AttributeBean attributeData = useItemInfo.GetAttributeData();
                int itemBreakLevel = attributeData.GetAttributeValue(AttributeTypeEnum.BreakLevel);
                //����ǵ�ǰʹ�õ���
                if (useItemInfo.GetItemsType() == (ItemsTypeEnum)blockBreakType)
                {
                    //������ƻ��ȼ� ���ҵ��ߵ��ھ�ȼ�С���ƻ��ȼ� �򲻿�����
                    if (!break_level.IsNull() && int.Parse(break_level) > itemBreakLevel)
                    {
                        canBreak = false;
                        isAdditionBreak = false;
                        return;
                    }
                    //�������������ھ�
                    else
                    {
                        canBreak = true;
                        isAdditionBreak = true;
                        return;
                    }
                }
            }
            canBreak = false;
            isAdditionBreak = false;
            return;
        }
    }
}