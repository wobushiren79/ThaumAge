﻿using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class Item
{
    /// <summary>
    /// /获取道具信息
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfo(long itemId)
    {
       return ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
    }

    /// <summary>
    /// 播放使用动画
    /// </summary>
    public virtual void UseForAnim(GameObject user, ItemsBean itemsData)
    {
        //播放使用动画
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        CreatureCptBase creature = user.GetComponentInChildren<CreatureCptBase>();
        if (itemsInfo.anim_use.IsNull())
            //如果没有动画 则播放默认的使用动画
            creature.creatureAnim.PlayUse(true);
        else
        {
            //如果可以转换成int 说明是use的另外一种类型
            if (int.TryParse(itemsInfo.anim_use,out int reslut))
            {         
                creature.creatureAnim.PlayUse(true, reslut);
            }
            else
            {
                //如果该道具指定播放指定动画
                creature.creatureAnim.PlayAnim(itemsInfo.anim_use);
            }
        }
    }

    /// <summary>
    /// 使用
    /// </summary>
    public virtual void Use(GameObject user, ItemsBean itemsData)
    {
        Player player = user.GetComponent<Player>();
        if (player)
        {
            UseForPlayer(player, itemsData);
        }
        else
        {
            UseForOther(user, itemsData);
        }
    }

    protected virtual void UseForPlayer(Player player, ItemsBean itemsData)
    {
    }

    protected virtual void UseForOther(GameObject user, ItemsBean itemsData)
    {

    }

    /// <summary>
    /// 使用目标
    /// </summary>
    public virtual void UseTarget(ItemsBean itemsData)
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
                BlockCptBreak BlockCptBreak = BlockHandler.Instance.BreakBlock(targetPosition, oldBlock, GetBreakDamage());
                if (BlockCptBreak.blockLife <= 0)
                {
                    //移除破碎效果
                    BlockHandler.Instance.DestroyBreakBlock(targetPosition);

                    BlockShapeEnum oldBlockShape = oldBlock.blockInfo.GetBlockShape();

                    //如果是种植类物品
                    if (oldBlockShape == BlockShapeEnum.PlantCross
                        || oldBlockShape == BlockShapeEnum.PlantCrossOblique
                        || oldBlockShape == BlockShapeEnum.PlantWell)
                    {
                        //首先判断生长周期
                        BlockBean blockData = targetChunk.GetBlockData(targetPosition - targetChunk.chunkData.positionForWorld);
                        //获取种植收货
                        List<ItemsBean> listHarvest = oldBlock.GetDropItems(blockData);
                        //创建掉落物
                        ItemsHandler.Instance.CreateItemCptDropList(listHarvest, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                    }
                    else
                    {
                        //获取掉落道具
                        List<ItemsBean> listDrop =  oldBlock.GetDropItems(null);
                        //如果没有掉落物，则默认掉落本体一个
                        if (listDrop.IsNull())
                        {
                            //创建掉落物
                            ItemsHandler.Instance.CreateItemCptDrop(oldBlock.blockType, 1, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                        }
                        else
                        {
                            //创建掉落物
                            ItemsHandler.Instance.CreateItemCptDropList(listDrop, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                        }
                    }
                    //移除该方块
                    targetChunk.RemoveBlockForWorld(targetPosition);
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