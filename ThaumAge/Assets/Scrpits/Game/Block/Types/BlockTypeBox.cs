using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeBox : BlockBaseBox
{
    public override List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        List<ItemsBean> listData = base.GetDropItems(blockData);
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockData.GetBlockType());
        //加一个自己
        listData.Add(new ItemsBean(itemsInfo.id, 1, null));
        //添加箱子里的物品
        if (blockData == null)
            return listData;
        BlockBoxBean blockBoxData = FromMetaData<BlockBoxBean>(blockData.meta);
        if (blockBoxData == null)
            return listData;
        for (int i = 0; i < blockBoxData.items.Length; i++)
        {
            ItemsBean itemData = blockBoxData.items[i];
            listData.Add(itemData);
        }
        return listData;
    }
}