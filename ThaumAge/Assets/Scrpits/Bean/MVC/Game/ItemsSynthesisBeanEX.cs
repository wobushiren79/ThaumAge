using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class ItemsSynthesisBean
{

    protected List<ItemsSynthesisMaterialsBean> listMaterials;

    /// <summary>
    /// 检测是否能合成
    /// </summary>
    public bool CheckSynthesis()
    {
        //获取合成材料
        List<ItemsSynthesisMaterialsBean> listMaterials = GetSynthesisMaterials();
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        for (int i = 0; i < listMaterials.Count; i++)
        {
            ItemsSynthesisMaterialsBean itemMaterials = listMaterials[i];
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
    public bool CheckSynthesisType(int[] types)
    {
        if (type_synthesis.IsNull())
            return true;
        int[] currentTypes = type_synthesis.SplitForArrayInt('|');
        for (int i = 0; i < types.Length; i++)
        {
            int itemType = types[i];
            for (int f = 0; f < currentTypes.Length; f++)
            {
                if (itemType == currentTypes[f])
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取合成的道具
    /// </summary>
    /// <returns></returns>
    public List<ItemsSynthesisMaterialsBean> GetSynthesisMaterials()
    {
        if (listMaterials == null)
        {
            listMaterials = new List<ItemsSynthesisMaterialsBean>();
            string[] listItemsData = materials.SplitForArrayStr('&');
            for (int i = 0; i < listItemsData.Length; i++)
            {
                string itemData1 = listItemsData[i];
                string[] itemData2 = itemData1.SplitForArrayStr(':');
                long[] itemData3 = itemData2[0].SplitForArrayLong('|');

                if (itemData2.Length == 1)
                {
                    listMaterials.Add(new ItemsSynthesisMaterialsBean(itemData3, 1));
                }
                else
                {
                    listMaterials.Add(new ItemsSynthesisMaterialsBean(itemData3, long.Parse(itemData2[1])));
                }
            }
        }
        return listMaterials;
    }

    /// <summary>
    /// 获取合成结果
    /// </summary>
    public void GetSynthesisResult(out long itemId, out long itemNumber)
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
            itemNumber = itemData[1];
        }
    }
}

public class ItemsSynthesisMaterialsBean
{
    public long[] itemIds;
    public long itemNumber;

    public ItemsSynthesisMaterialsBean(long[] itemIds, long itemNumber)
    {
        this.itemIds = itemIds;
        this.itemNumber = itemNumber;
    }

}