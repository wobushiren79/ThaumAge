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
        ui_ViewSynthesisMaterial.ShowObj(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        this.RegisterEvent<int>(EventsInfo.UIViewSynthesis_SetSelect, SetSelect);
        SetSelect(indexSelect);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        listSynthesisData = ItemsHandler.Instance.manager.GetItemsSynthesisByType(ItemsSynthesisTypeEnum.Self);
        ui_SynthesisList.SetCellCount(listSynthesisData.Count);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if(viewButton== ui_BtnSynthesis)
        {
            OnClickForStartSynthesis();
        }
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
        itemSynthesis.SetData(itemData, itemView.index, isSelect);
    }

    /// <summary>
    /// 点击-开始合成
    /// </summary>
    public void OnClickForStartSynthesis()
    {
        //当前选中的合成道具
        ItemsSynthesisBean itemsSynthesis = listSynthesisData[indexSelect];
        //检测当前道具是否能合成
        bool canSynthesis= itemsSynthesis.CheckSynthesis();

        if (!canSynthesis)
        {
            UIHandler.Instance.ToastHint<ToastView>("素材不足，无法合成");
        }

        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
    }

    /// <summary>
    /// 设置选中第几项
    /// </summary>
    /// <param name="indexSelect"></param>
    public void SetSelect(int indexSelect)
    {
        this.indexSelect = indexSelect;
        //刷新列表
        ui_SynthesisList.RefreshAllCells();
        //刷新结果
        ui_SynthesisResults.SetData(listSynthesisData[indexSelect], -1, false);
        //刷新素材
        SetSynthesisMaterials();
    }

    /// <summary>
    /// 设置素材
    /// </summary>
    public void SetSynthesisMaterials()
    {
        //先删除所有素材
        ui_SynthesisMaterials.DestroyAllChild();
        //获取当前选中合成道具
        ItemsSynthesisBean itemsSynthesis = listSynthesisData[indexSelect];
        List<ItemsSynthesisMaterialsBean> listMaterials = itemsSynthesis.GetSynthesisMaterials();
        //获取起始点位置
        Vector2[] listCirclePosition = VectorUtil.GetListCirclePosition(listMaterials.Count, 0, Vector2.zero, 95);
        //创建所有素材
        int itemAngle = 360 / listMaterials.Count;
        for (int i = 0; i < listMaterials.Count; i++)
        {
            GameObject objMaterial = Instantiate(ui_SynthesisMaterials.gameObject, ui_ViewSynthesisMaterial.gameObject);
            UIViewSynthesisMaterial itemMaterial = objMaterial.GetComponent<UIViewSynthesisMaterial>();

            ItemsSynthesisMaterialsBean itemData = listMaterials[i];
            itemMaterial.SetData(itemData, itemAngle * i);
            itemMaterial.rectTransform.anchoredPosition = listCirclePosition[i];
        }
    }
}