using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Item
{
    public ItemsBean itemsData;
    public ItemsInfoBean _itemsInfo;

    public ItemsInfoBean itemsInfo 
    {
        get
        {
            if (_itemsInfo == null)
            {
                _itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            }
            return _itemsInfo;
        }
    }


    public void SetItemData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
    }

    /// <summary>
    /// 使用
    /// </summary>
    public virtual void Use()
    {

    }

    /// <summary>
    /// 使用目标
    /// </summary>
    public virtual void UseTarget()
    {
        Player player = GameHandler.Instance.manager.player;
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            //展示目标位置
            GameHandler.Instance.manager.playerTargetBlock.Show(targetBlockPosition);
        }
        else
        {
            //展示目标位置
            GameHandler.Instance.manager.playerTargetBlock.Hide();
        }
    }

    /// <summary>
    /// 破碎目标
    /// </summary>
    public virtual void BreakTarget(Vector3Int targetPosition)
    {
        //获取原位置方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block oldBlock, out DirectionEnum oldBlockDirection, out Chunk targetChunk);
        if (targetChunk)
        {
            //如果原位置是空则不做处理
            if (oldBlock != null && oldBlock.blockType != BlockTypeEnum.None)
            {
                BlockBreak blockBreak = BlockHandler.Instance.BreakBlock(targetPosition, oldBlock, GetBreakDamage());
                if (blockBreak.blockLife <= 0)
                {
                    //移除破碎效果
                    BlockHandler.Instance.DestroyBreakBlock(targetPosition);

                    BlockShapeEnum oldBlockShape = oldBlock.blockInfo.GetBlockShape();
                    
                    if (oldBlockShape == BlockShapeEnum.PlantCross
                        || oldBlockShape == BlockShapeEnum.PlantCrossOblique
                        || oldBlockShape == BlockShapeEnum.PlantWell)
                    {
                        //如果是种植类物品
                        //首先判断生长周期
                        BlockBean blockData = targetChunk.GetBlockData(targetPosition);
                        if (blockData == null)
                        {
                            BlockPlantExtension.FromMetaData(blockData.meta,out int growPro,out bool isStartGrow);
                        }
                        else
                        {

                        }


                        ItemsHandler.Instance.CreateItemDrop(oldBlock.blockType, 1, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                    }
                    else
                    {
                        //创建掉落物
                        ItemsHandler.Instance.CreateItemDrop(oldBlock.blockType, 1, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                    }
     
                    //移除该方块
                    targetChunk.RemoveBlockForWorld(targetPosition);

                    Block nullBlock = BlockHandler.Instance.manager.GetRegisterBlock(BlockTypeEnum.None);

                    WorldCreateHandler.Instance.HandleForUpdateChunk(targetChunk, targetPosition - targetChunk.chunkData.positionForWorld, oldBlock, nullBlock);
                }
            }
        }
    }

    /// <summary>
    /// 获取道具破坏的伤害
    /// </summary>
    public virtual int GetBreakDamage()
    {
        return 1;
    }
}