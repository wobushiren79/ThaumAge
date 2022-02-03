using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewSynthesisItem : BaseUIView
{
    protected ItemsSynthesisBean itemsSynthesis;
    protected int index = 0;
    protected Button configBtn;

    protected Material matIcon;

    public override void Awake()
    {
        gameObject.name = "ViewSynthesisItem";
        base.Awake();
        configBtn = this.GetComponent<Button>();
        SetSelectState(false);

        //重新实例化一份材质球
        matIcon = new Material(ui_ItemIcon.material);
        ui_ItemIcon.material = matIcon;
    }

    public void SetData(ItemsSynthesisBean itemsSynthesis, int index, bool isSelect)
    {
        this.index = index;
        this.itemsSynthesis = itemsSynthesis;
        itemsSynthesis.GetSynthesisResult(out long resultId, out int resultNum);

        bool canSynthesis = itemsSynthesis.CheckSynthesis();
        SetItemIcon(resultId);
        SetSynthesisState(canSynthesis);
        SetSelectState(isSelect);
        SetPopupInfo(resultId);
        SetNumber(resultNum, canSynthesis);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == configBtn)
        {
            this.TriggerEvent(EventsInfo.UIViewSynthesis_SetSelect, index);
        }
    }

    /// <summary>
    /// 设置弹出信息
    /// </summary>
    /// <param name="itemsId"></param>
    public void SetPopupInfo(long itemsId)
    {
        ui_ViewSynthesisItem.SetItemId(itemsId);
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
            ui_ItemIcon.material.SetFloat("_GreyLerp", 1);
        }
        else
        {
            ui_ItemIcon.material.SetFloat("_GreyLerp", 0);
        }
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    public void SetSelectState(bool isSelect)
    {
        if (isSelect)
        {
            ui_Select.ShowObj(true);
        }
        else
        {
            ui_Select.ShowObj(false);
        }
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    public void SetNumber(long number, bool canSynthesis)
    {
        if (canSynthesis)
        {
            ui_TVNumber.color = Color.green;
        }
        else
        {
            ui_TVNumber.color = Color.white;
        }
        ui_TVNumber.text = $"{number}";
    }
}