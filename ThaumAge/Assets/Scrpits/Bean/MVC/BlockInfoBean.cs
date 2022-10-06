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

    public string color;//颜色

    public int shape;//形状

    public string uv_position;//uv位置

    public string offset_border;//边偏移

    public float weight;//重量


    //是否能旋转 
    //0不能旋转(永远只能UpForward)
    //1能旋转(各个方向)
    //2只能LRFB方向旋转(只能是 UpLeft,UpRight,UpForward,UpBack　4个方向)
    //3只能UD 方向（只能是Up,Down）
    public int rotate_state;

    public int life;//生命值

    public int material_type = 1;//材质类型
    public int material_type2 = 1;//材质类型

    public int collider_state;//是否碰撞 0没有碰撞 1有碰撞

    public int trigger_state;//是否触碰 0没有触碰 1有触碰

    public int plough_state;//耕地状态 0不能耕地 1能耕地

    public int plant_state;//种植状态 0不能种植 1能种植

    public string items_drop;//掉落

    public string break_type;//指定破坏类型

    public string break_level;//指定破坏等级

    public int interactive_state;//互动状态 0不可互动 1可互动(F键)

    public int remark_int;//备注int

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <returns></returns>
    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)id;
    }

    /// <summary>
    /// 获取方块形状
    /// </summary>
    /// <returns></returns>
    public BlockShapeEnum GetBlockShape()
    {
        return (BlockShapeEnum)shape;
    }
    
    /// <summary>
    /// 获取方块颜色
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
    /// 获取材质类型
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
    /// 获取UV坐标点
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
    /// 获取偏移边坐标
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
    /// 获取方块网格数据
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
            LogUtil.LogError("加载BlockMesh失败：" + e.Message);
        }
        return JsonUtil.FromJson<MeshDataCustom>(textAsset.text);
    }

    /// <summary>
    /// 获取破坏类型(默认使用道具的类型 ，特殊情况特殊处理)
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
    ///  检测是否能被破坏
    /// </summary>
    /// <param name="breakItemId"></param>
    /// <param name="canBreak">是否能挖掘</param>
    /// <param name="isAdditionBreak">是否享受挖掘加成</param>
    public void CheckCanBreak(long breakItemId, out bool canBreak, out bool isAdditionBreak)
    {
        canBreak = false;
        isAdditionBreak = false;
        List<BlockBreakTypeEnum> listBreakType = GetBreakType();

        //并且目标方块可以被任意道具破坏
        if (listBreakType.Contains(BlockBreakTypeEnum.Normal))
        {
            //如果是空手
            if (breakItemId == 0)
            {
                canBreak = true;
                isAdditionBreak = false;
                return;
            }
            //如果不是空手
            else
            {
                ItemsInfoBean useItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(breakItemId);
                AttributeBean attributeData = useItemInfo.GetAttributeData();
                int itemBreakLevel = attributeData.GetAttributeValue(AttributeTypeEnum.BreakLevel);
                ItemsTypeEnum itemsType = useItemInfo.GetItemsType();
                for (int i = 0; i < listBreakType.Count; i++)
                {
                    //如果是当前使用道具
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
        //目标方块不能被破坏
        else if (listBreakType.Contains(BlockBreakTypeEnum.NotBreak))
        {
            canBreak = false;
            isAdditionBreak = false;
        }
        //目标方块只能被指定道具破坏
        else
        {
            for (int i = 0; i < listBreakType.Count; i++)
            {
                BlockBreakTypeEnum blockBreakType = listBreakType[i];
                ItemsInfoBean useItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(breakItemId);
                AttributeBean attributeData = useItemInfo.GetAttributeData();
                int itemBreakLevel = attributeData.GetAttributeValue(AttributeTypeEnum.BreakLevel);
                //如果是当前使用道具
                if (useItemInfo.GetItemsType() == (ItemsTypeEnum)blockBreakType)
                {
                    //如果有破坏等级 并且道具的挖掘等级小于破坏等级 则不可以挖
                    if (!break_level.IsNull() && int.Parse(break_level) > itemBreakLevel)
                    {
                        canBreak = false;
                        isAdditionBreak = false;
                        return;
                    }
                    //其他情况则可以挖掘
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