using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BlockTypeCrucibleComponent : BlockTypeComponent
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerInfo.Items)
        {
            GetBlock(out Block targetBlock, out Chunk targetChunk);
            if (targetChunk != null && targetBlock != null)
            {
                if (targetBlock is BlockTypeCrucible blockTypeCrucible)
                {
                    //获取道具数据
                    ItemCptDrop itemCptDrop = other.gameObject.GetComponent<ItemCptDrop>();
                    if (!itemCptDrop.canInteractiveBlock)
                        return;
                    if (itemCptDrop == null)
                        return;
                    ItemsInfoBean itemsInfo = itemCptDrop.itemsInfo;
                    //首先判断这个道具是否满足转换条件
                    ItemsBean itemData = itemCptDrop.itemDropData.itemData;
                    blockTypeCrucible.StartSynthesis(blockWorldPosition, itemData, out int numberSynthesis);
                    if (numberSynthesis > 0)
                    {
                        if (numberSynthesis >= itemData.number)
                        {
                            Destroy(other.gameObject);
                            return;
                        }
                        else
                        {
                            itemData.number -= numberSynthesis;
                        }
                        blockTypeCrucible.PlaySynthesisEffect(blockWorldPosition + new Vector3(0.5f, 0.5f, 0.5f));
                    }
                    //如果不能转换 则直接变成元素
                    Dictionary<ElementalTypeEnum, int> dicElementalData = itemsInfo.GetAllElemental();
                    List<NumberBean> listElementalData = new List<NumberBean>();

                    foreach (var itemElementalData in dicElementalData)
                    {
                        listElementalData.Add(new NumberBean((int)itemElementalData.Key, itemElementalData.Value * itemData.number));
                    }
                    //添加元素
                    bool isAddElementalSuccess = blockTypeCrucible.AddElemental(blockWorldPosition, listElementalData);
                    if (isAddElementalSuccess)
                    {
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}