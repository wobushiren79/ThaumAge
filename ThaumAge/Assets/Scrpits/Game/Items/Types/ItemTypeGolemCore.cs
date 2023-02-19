using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemTypeGolemCore : Item
{
    public static List<GameObject> listSelectTargetObj = new List<GameObject>();

    public ItemTypeGolemCore()
    {
        EventHandler.Instance.RegisterEvent<int>(EventsInfo.UIViewShortcuts_ChangeSelect, CallBackForShortcutsChangeSelect);
    }

    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        ClearAllSelect();
        CreateSelectObj(targetPosition);

        //保存数据
        ItemMetaGolemCore itemMetaGolemCore = itemData.GetMetaData<ItemMetaGolemCore>();
        itemMetaGolemCore.bindBlockWorldPosition = targetPosition;
        itemData.SetMetaData(itemMetaGolemCore);
        return true;
    }

    public void CreateSelectObj(Vector3Int targetPosition)
    {
        GameObject objModelSelect = BlockHandler.Instance.manager.blockGolemCoreSelectModel;
        GameObject objSelect = BlockHandler.Instance.Instantiate(BlockHandler.Instance.gameObject, objModelSelect);
        objSelect.transform.position = targetPosition + new Vector3(0.5f, 0.5f, 0.5f);
        listSelectTargetObj.Add(objSelect);
    }

    /// <summary>
    /// 初始化选择的方块
    /// </summary>
    public void InitSelect(ItemsBean currentItems)
    {
        ClearAllSelect();
        ItemMetaGolemCore itemMetaGolemCore = currentItems.GetMetaData<ItemMetaGolemCore>();
        if(itemMetaGolemCore.bindBlockWorldPosition.y != int.MaxValue)
        {
            CreateSelectObj(itemMetaGolemCore.bindBlockWorldPosition);
        }
    }

    /// <summary>
    /// 清楚所有选择的方块
    /// </summary>
    public void ClearAllSelect()
    {
        for (int i = 0; i < listSelectTargetObj.Count; i++)
        {
            GameObject selectItemObj = listSelectTargetObj[i];
            GameObject.Destroy(selectItemObj);
        }
        listSelectTargetObj.Clear();
    }

    #region 回调
    public void CallBackForShortcutsChangeSelect(int selectIndex)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean currentItems = userData.GetItemsFromShortcut();
        var currentItemsInfo = GetItemsInfo(currentItems.itemId);
        if (currentItemsInfo.GetItemsType() == ItemsTypeEnum.GolemCore)
        {
            InitSelect(currentItems);
        }
        else
        {
            ClearAllSelect();
        }
    }
    #endregion
}