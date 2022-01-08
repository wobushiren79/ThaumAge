using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewSynthesisItem : BaseUIView
{
    protected ItemsSynthesisBean itemsSynthesis;

    public void SetData(ItemsSynthesisBean itemsSynthesis,bool isSelect)
    {
        this.itemsSynthesis = itemsSynthesis;
        itemsSynthesis.GetSynthesisResult(out long resultId, out long resultNum);

        bool canSynthesis = itemsSynthesis.CheckSynthesis();
        SetItemIcon(resultId);
        SetSynthesisState(canSynthesis);
        SetSelectState(isSelect);
    }

    /// <summary>
    /// 设置合成物品图标
    /// </summary>
    /// <param name="itemsId"></param>
    public void SetItemIcon(long itemsId)
    {
        ItemsHandler.Instance.SetItemsIconById(ui_ItemIcon, itemsId);
    }

    /// <summary>
    /// 设置合成状态
    /// </summary>
    public void SetSynthesisState(bool canSynthesis)
    {
        if (canSynthesis)
        {
            ui_ItemIcon.material.SetFloat("_GreyLerp", 0);
        }
        else
        {
            ui_ItemIcon.material.SetFloat("_GreyLerp", 1);
        }
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    public void SetSelectState(bool isSelect)
    {
        if (isSelect)
        {

        }
        else
        {

        }
    }
}