using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewSynthesis : BaseUIView
{
    //合成数据
    protected List<ItemsSynthesisBean> listSynthesisData;
    protected int indexSelect = 0;

    public override void Awake()
    {
        base.Awake();
        ui_SynthesisList.AddCellListener(OnCellForItemSynthesis);
        ui_SynthesisList.SetCellCount(100);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        listSynthesisData = ItemsHandler.Instance.manager.GetItemsSynthesisByType(1);

    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
    }

    /// <summary>
    /// 单个数据
    /// </summary>
    /// <param name="itemView"></param>
    public void OnCellForItemSynthesis(ScrollGridCell itemView)
    {
        UIViewSynthesisItem itemSynthesis = itemView.GetComponent<UIViewSynthesisItem>();
        //item数据
        ItemsSynthesisBean itemData = listSynthesisData[itemView.index];
        //是否选中当前
        bool isSelect = (itemView.index == indexSelect ? true : false);
        //设置数据
        itemSynthesis.SetData(itemData, isSelect);
    }

    /// <summary>
    /// 点击-开始合成
    /// </summary>
    public void OnClickForStartSynthesis()
    {
        BaseUIComponent currentUI =  UIHandler.Instance.GetOpenUI();
        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
    }
}