using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewSynthesis : BaseUIView
{
    //合成数据
    protected List<ItemsSynthesisBean> listSynthesisData;

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

    public void OnCellForItemSynthesis(ScrollGridCell itemView)
    {

    }
}