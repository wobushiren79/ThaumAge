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
            GetBlock(out Block targetBlock,out Chunk targetChunk);
            if (targetChunk != null && targetBlock != null)
            {
                if (targetBlock is BlockTypeCrucible blockTypeCrucible)
                {
                    //获取道具数据
                    ItemCptDrop itemCptDrop = other.gameObject.GetComponent<ItemCptDrop>();
                    if (itemCptDrop == null)
                        return;
                    ItemsInfoBean itemsInfo = itemCptDrop.itemsInfo;
                    Dictionary<ElementalTypeEnum, int> dicElementalData = itemsInfo.GetAllElemental();
                    List<NumberBean> listElementalData = new List<NumberBean>();

                    foreach (var itemData in dicElementalData)
                    {
                        listElementalData.Add(new NumberBean((int)itemData.Key, itemData.Value));
                    }
                    //添加元素
                    bool isAddElementalSuccess =  blockTypeCrucible.AddElemental(blockWorldPosition, listElementalData);
                    if (isAddElementalSuccess)
                    {
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}