using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class ItemsSynthesisBean
{
    protected List<ItemsArrayBean> listMaterials;
    protected Dictionary<ElementalTypeEnum, int> dicElemental;

    /// <summary>
    /// 获取元素
    /// </summary>
    public Dictionary<ElementalTypeEnum, int> GetElemental()
    {
        if (dicElemental == null)
        {
            dicElemental = new Dictionary<ElementalTypeEnum, int>();
            string[] listElementalData = elemental.Split('&');
            for (int i = 0; i < listElementalData.Length; i++)
            {
                string itemElementalDataStr = listElementalData[i];
                string[] elementalData = itemElementalDataStr.Split(':');
                if (elementalData.Length == 1)
                {
                    dicElemental.Add(EnumExtension.GetEnum<ElementalTypeEnum>(elementalData[0]), 1);
                }
                else if (elementalData.Length == 2)
                {
                    dicElemental.Add(EnumExtension.GetEnum<ElementalTypeEnum>(elementalData[0]), int.Parse(elementalData[1]));
                }
            }
            
        }
        return dicElemental;
    }

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
    /// 检测坩埚的合成
    /// </summary>
    /// <returns></returns>
    public void CheckSynthesisForCrucible(List<NumberBean> listHasElemental, ItemsBean itemData, out int numberSynthesis)
    {
        numberSynthesis = 0;
        //如果不是这个道具
        if (itemData.itemId != int.Parse(materials))
        {
            return;
        }
        Dictionary<ElementalTypeEnum, int> dicElemental = GetElemental();

        for (int n = 0; n < itemData.number; n++)
        {
            bool hasEnoughElemental = true;
            foreach (var itemElemental in dicElemental)
            {
                bool hasEnoughElementalItem = false;
                for (int i = 0; i < listHasElemental.Count; i++)
                {
                    NumberBean itemHasElemental = listHasElemental[i];
                    if (itemHasElemental.id == (int)itemElemental.Key)
                    {
                        if (itemHasElemental.number >= itemElemental.Value * (numberSynthesis + 1))
                        {
                            hasEnoughElementalItem = true;
                            break;
                        }
                    }
                }
                if (!hasEnoughElementalItem)
                {
                    hasEnoughElemental = false;
                    break;
                }
            }
            if (hasEnoughElemental)
                numberSynthesis++;
            else
                break;
        }
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
    public bool CheckSynthesisType(ItemsSynthesisTypeEnum itemsSynthesisType, out int[] currentTypes)
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

public partial class ItemsSynthesisCfg
{   
    //坩埚的数据
    protected static List<ItemsSynthesisBean> listItemsSynthesisForCrucible;

    /// <summary>
    /// 通过类型获取道具合成数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<ItemsSynthesisBean> GetItemsSynthesisByType(ItemsSynthesisTypeEnum itemsSynthesisType)
    {
        List<ItemsSynthesisBean> listData = new List<ItemsSynthesisBean>();
        var dicItemsSynthesis = GetAllData();
        foreach (var itemData in dicItemsSynthesis)
        {
            ItemsSynthesisBean itemValue = itemData.Value;
            //检测是否包含该类型的合成
            if (itemValue.CheckSynthesisType(itemsSynthesisType, out int[] itemSynthesisTypes))
            {
                bool hasSelf = false;
                foreach (var itemDataType in itemSynthesisTypes)
                {
                    //如果是默认的 那不用判断是否解锁 默认解锁
                    if (itemDataType == (int)ItemsSynthesisTypeEnum.Self)
                    {
                        hasSelf = true;
                        break;
                    }
                }
                if (hasSelf)
                {

                }
                else
                {
                    //检测是否解锁该合成
                    bool isUnlockSynthesis = itemValue.CheckIsUnlockSynthesis();
                    if (isUnlockSynthesis == false)
                        continue;
                }
                listData.Add(itemValue);
            }
        }
        return listData;
    }

    public static List<ItemsSynthesisBean> GetItemsSynthesisForCrucible()
    {
        if (listItemsSynthesisForCrucible.IsNull())
        {
            listItemsSynthesisForCrucible = new List<ItemsSynthesisBean>();
            var dicItemsSynthesis = GetAllData();
            foreach (var itemData in dicItemsSynthesis)
            {
                ItemsSynthesisBean itemValue = itemData.Value;
                if (itemValue.type_synthesis.Equals("21"))
                {
                    listItemsSynthesisForCrucible.Add(itemValue);
                }
            }
        }
        return listItemsSynthesisForCrucible;
    }
}
