using UnityEditor;
using UnityEngine;

public partial class UIItemBuildingEditorCreateBlockSelect : BaseUIView
{
    protected BlockInfoBean blockInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(BlockInfoBean blockInfo)
    {
        this.blockInfo = blockInfo;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockId(blockInfo.id);
        if (itemsInfo != null)
        {
            SetBlockIcon(itemsInfo.icon_key);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetBlockIcon(string iconKey)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconKey, (spIcon)=> 
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
}