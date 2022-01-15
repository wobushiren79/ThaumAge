using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewSynthesis : BaseUIView
{
    //合成数据
    protected List<ItemsSynthesisBean> listSynthesisData;
    //当前选中项
    protected int indexSelect = 0;
    //素材UI
    protected List<UIViewSynthesisMaterial> listUIMaterial;

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
        RefreshMaterials();
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BtnSynthesis)
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
        bool canSynthesis = itemsSynthesis.CheckSynthesis();

        if (!canSynthesis)
        {
            //素材不足 无法合成
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30002));
            return;
        }
        //首先消耗素材
        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        //获取素材
        List<ItemsSynthesisMaterialsBean> listMaterials = itemsSynthesis.GetSynthesisMaterials();

        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        //扣除素材
        for (int i = 0; i < listMaterials.Count; i++)
        {
            ItemsSynthesisMaterialsBean itemMaterials = listMaterials[i];
            for (int f = 0; f < itemMaterials.itemIds.Length; f++)
            {
                //只要扣除其中一项素材就行
                long itemMaterialId = itemMaterials.itemIds[f];
                if (userData.HasEnoughItem(itemMaterialId, itemMaterials.itemNumber))
                {
                    userData.RemoveItem(itemMaterialId, itemMaterials.itemNumber);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        //添加道具
        itemsSynthesis.GetSynthesisResult(out long itemsId, out int itemNum);
        int moreNum = userData.AddItems(itemsId, itemNum);
        //如果还有多余的道具 则丢出来
        if (moreNum > 0)
        {
            Player player = GameHandler.Instance.manager.player;
            ItemsHandler.Instance.CreateItemCptDrop(itemsId, moreNum, player.transform.position + Vector3.up, ItemDropStateEnum.DropNoPick);
        }

        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        backpackUI.RefreshUI();
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
        shortcutsUI.RefreshUI();
        RefreshUI();
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
        if (listUIMaterial == null)
            listUIMaterial = new List<UIViewSynthesisMaterial>();
        listUIMaterial.Clear();

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
            listUIMaterial.Add(itemMaterial);
        }
    }

    /// <summary>
    /// 刷新素材显示
    /// </summary>
    public void RefreshMaterials()
    {
        if (listUIMaterial == null)
            listUIMaterial = new List<UIViewSynthesisMaterial>();
        for (int i = 0; i < listUIMaterial.Count; i++)
        {
            UIViewSynthesisMaterial itemUIMaterial = listUIMaterial[i];
            itemUIMaterial.RefreshUI();
        }
    }
}