using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewGolemDetails : BaseUIView
{
    public override void Awake()
    {
        base.Awake();
        ui_ViewItemContainer.ShowObj(false);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_CoreList.DestroyAllChild(true);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(List<ItemsBean> listGolemCore)
    {
        ui_CoreList.DestroyAllChild(true);
        for (int i = 0; i < listGolemCore.Count; i++)
        {
            GameObject objItem = Instantiate(ui_CoreList.gameObject, ui_ViewItemContainer.gameObject);
            objItem.ShowObj(true);
            UIViewItemContainer uiViewItemContainer = objItem.GetComponent<UIViewItemContainer>();

            ItemsBean itemData = listGolemCore[i];
            uiViewItemContainer.SetLimitType(ItemsTypeEnum.GolemCore);
            uiViewItemContainer.SetViewItemByData(UIViewItemContainer.ContainerType.Bag, itemData);

            uiViewItemContainer.SetCallBackForSetViewItem(CallBackForItemChange);
        }
    }

    /// <summary>
    /// 回调 道具改变
    /// </summary>
    /// <param name="uiViewItemContainer"></param>
    /// <param name="itemsData"></param>
    public void CallBackForItemChange(UIViewItemContainer uiViewItemContainer, ItemsBean itemsData)
    {
        //TODO 保存数据
    }
}