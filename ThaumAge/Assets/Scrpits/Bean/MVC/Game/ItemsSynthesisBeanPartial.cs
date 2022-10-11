using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class ItemsSynthesisBean
{
    protected List<ItemsArrayBean> listMaterials;

    /// <summary>
    /// 检测是否已经解锁该合成
    /// </summary>
    /// <returns></returns>
    public bool CheckIsUnlockSynthesis()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        //获取合成材料
        List<ItemsArrayBean> listMaterials = GetSynthesisMaterials();
        for (int i = 0; i < listMaterials.Count; i++)
        {
            var itemMaterial = listMaterials[i];
            bool isUnlockMaterail = userData.userAchievement.CheckUnlockGetItemsForOr(itemMaterial.itemIds);
            if (isUnlockMaterail == false)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 检测是否能合成
    /// </summary>
    public bool CheckSynthesis()
    {
        //获取合成材料
        List<ItemsArrayBean> listMaterials = GetSynthesisMaterials();
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        for (int i = 0; i < listMaterials.Count; i++)
        {
            ItemsArrayBean itemMaterials = listMaterials[i];
            bool hasMaterial = false;
            for (int f = 0; f < itemMaterials.itemIds.Length; f++)
            {
                long itemsId = itemMaterials.itemIds[f];
                //有其中一种素材
                bool hasEnoughItem = userData.HasEnoughItem(itemsId, itemMaterials.itemNumber);
                if (hasEnoughItem)
                {
                    hasMaterial = true;
                    break;
                }   
            }
            if (hasMaterial == false)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 检测合成类型 
    /// </summary>
    /// <param name="types"></param>
    public bool CheckSynthesisType(ItemsSynthesisTypeEnum itemsSynthesisType,out int[] currentTypes)
    {
        currentTypes = null;
        if (type_synthesis.IsNull())
            return false;
        currentTypes = type_synthesis.SplitForArrayInt('|');
        int itemType = (int)itemsSynthesisType;
        for (int f = 0; f < currentTypes.Length; f++)
        {
            if (itemType == currentTypes[f])
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取合成的道具
    /// </summary>
    /// <returns></returns>
    public List<ItemsArrayBean> GetSynthesisMaterials()
    {
        return ItemsArrayBean.GetListItemsArrayBean(materials);
    }

    /// <summary>
    /// 获取合成结果
    /// </summary>
    public void GetSynthesisResult(out long itemId, out int itemNumber)
    {
        long[] itemData = results.SplitForArrayLong(':');
        if (itemData.Length == 1)
        {
            itemId = itemData[0];
            itemNumber = 1;
        }
        else
        {
            itemId = itemData[0];
            itemNumber = (int)itemData[1];
        }
    }
}
