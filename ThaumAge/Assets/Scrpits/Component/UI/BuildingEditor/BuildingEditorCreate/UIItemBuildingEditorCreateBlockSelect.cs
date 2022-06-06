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
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(BlockInfoBean blockInfo, int index, int indexSelect)
    {
        this.index = index;
        this.blockInfo = blockInfo;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockId(blockInfo.id);
        if (itemsInfo != null)
        {
            SetBlockIcon(itemsInfo.icon_key);
        }
        //展示选中状态
        ShowSelect(indexSelect == index);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetBlockIcon(string iconKey)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconKey, (spIcon) =>
        {
            ui_Icon.sprite = spIcon;
        });
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