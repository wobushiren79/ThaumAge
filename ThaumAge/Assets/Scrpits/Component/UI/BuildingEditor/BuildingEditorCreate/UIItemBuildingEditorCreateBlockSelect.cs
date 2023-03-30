using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIItemBuildingEditorCreateBlockSelect : BaseUIView
{
    protected int index;
    protected BlockInfoBean blockInfo;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ItemBuildingEditorCreateBlockSelect)
        {
            OnClickForSelect();
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(BlockInfoBean blockInfo, int index, int indexSelect)
    {
        this.index = index;
        this.blockInfo = blockInfo;

        //设置图标
        SetBlockIcon(blockInfo);
        //展示选中状态
        ShowSelect(indexSelect == index);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetBlockIcon(BlockInfoBean blockInfo)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockId((int)blockInfo.id);
        Item item = ItemsHandler.Instance.manager.GetRegisterItem(itemsInfo.id, (ItemsTypeEnum)itemsInfo.items_type);
        item.SetItemIcon(null, itemsInfo, ui_Icon);
    }

    /// <summary>
    /// 展示选择
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowSelect(bool isShow)
    {
        ui_Select.ShowObj(isShow);
    }

    /// <summary>
    /// 点击-选中
    /// </summary>
    public void OnClickForSelect()
    {
        TriggerEvent(EventsInfo.UIBuildingEditorCreate_SelectChange, index);
    }
}