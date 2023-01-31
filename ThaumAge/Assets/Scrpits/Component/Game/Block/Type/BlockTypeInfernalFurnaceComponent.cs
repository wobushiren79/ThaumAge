using UnityEditor;
using UnityEngine;

public class BlockTypeInfernalFurnaceComponent : BlockTypeComponent
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerInfo.Items)
        {
            GetBlock(out Block targetBlock, out Chunk targetChunk);
            if (targetChunk != null && targetBlock != null)
            {
                if (targetBlock is BlockTypeInfernalFurnace blockTypeInfernalFurnace)
                {
                    //获取道具数据
                    ItemCptDrop itemCptDrop = other.gameObject.GetComponent<ItemCptDrop>();
                    if (!itemCptDrop.canInteractiveBlock)
                        return;
                    if (itemCptDrop == null)
                        return;
                    //将道具放入方块
                    blockTypeInfernalFurnace.ItemsPut(targetChunk,blockWorldPosition - targetChunk.chunkData.positionForWorld, itemCptDrop.itemDropData.itemData);
                    //删除当前的丢弃主题
                    itemCptDrop.DestroyCpt();
                }
            }
        }
    }
}