using UnityEditor;
using UnityEngine;

public class BlockTypeChestHungryComponent : BlockTypeComponent
{

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerInfo.Items)
        {
            GetBlock(out Block targetBlock, out Chunk targetChunk);
            if (targetChunk != null && targetBlock != null)
            {
                if (targetBlock is BlockTypeChestHungry blockTypeChest)
                {
                    //获取道具数据
                    ItemCptDrop itemCptDrop = collision.gameObject.GetComponent<ItemCptDrop>();
                    if (!itemCptDrop.canInteractiveBlock)
                        return;
                    if (itemCptDrop == null)
                        return;
                    //将道具放入方块
                    blockTypeChest.ItemsPut(targetChunk, blockWorldPosition - targetChunk.chunkData.positionForWorld, itemCptDrop.itemDropData.itemData);
                    //删除当前的丢弃
                    itemCptDrop.DestroyCpt();
                    //播放箱子动画
                    blockTypeChest.TriggerChest(blockWorldPosition);
                }
            }
        }
    }
}