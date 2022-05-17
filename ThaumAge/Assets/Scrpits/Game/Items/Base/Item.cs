using System.Collections.Generic;
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
            if (int.TryParse(itemsInfo.anim_use, out int reslut))
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
    /// <param name="user"></param>
    /// <param name="itemsData"></param>
    /// <param name="type">0鼠标左键使用 1鼠标右键使用 2F交互</param>
    public virtual void Use(GameObject user, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {
        Player player = user.GetComponent<Player>();
        if (player)
        {
            //如果是F键交互
            if (itemUseType == ItemUseTypeEnum.E)
            {
                UseForInteractive(player);
                return;
            }
            UseForPlayer(player, itemsData, itemUseType);
        }
        else
        {
            UseForOther(user, itemsData, itemUseType);
        }
    }

    /// <summary>
    /// 使用 交互
    /// </summary>
    protected virtual void UseForInteractive(Player player)
    {
        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            ChunkComponent chunkForHit = hit.collider.GetComponentInParent<ChunkComponent>();
            if (chunkForHit != null)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);

                Vector3Int localPosition = targetPosition - chunkForHit.chunk.chunkData.positionForWorld;
                //获取原位置方块
                chunkForHit.chunk.chunkData.GetBlockForLocal(localPosition, out Block tagetBlock, out BlockDirectionEnum targetDirection);
                if (tagetBlock != null && tagetBlock.blockInfo.interactive_state == 1)
                {
                    tagetBlock.Interactive(player.gameObject, targetPosition, targetDirection);
                }
            }
        }
    }


    protected virtual void UseForPlayer(Player player, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {
        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            ChunkComponent chunkForHit = hit.collider.GetComponentInParent<ChunkComponent>();
            if (chunkForHit != null)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);
                //挖掘
                if (itemUseType == ItemUseTypeEnum.Left)
                {
                    TargetBreak(itemsData, targetPosition);
                }
                else
                {
                    TargetUse(itemsData, targetPosition);
                }
            }
        }
    }

    protected virtual void UseForOther(GameObject user, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {

    }

    /// <summary>
    /// 瞄准
    /// </summary>
    public virtual void UseForSightTarget(ItemsBean itemsData)
    {
        Player player = GameHandler.Instance.manager.player;
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            ChunkComponent chunkForHit = hit.collider.GetComponentInParent<ChunkComponent>();
            if (chunkForHit != null && chunkForHit.chunk.isInit)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);
                Vector3Int localPosition = targetPosition - chunkForHit.chunk.chunkData.positionForWorld;
                if (localPosition.x < 0 || localPosition.x > chunkForHit.chunk.chunkData.chunkWidth
                    || localPosition.z < 0 || localPosition.z > chunkForHit.chunk.chunkData.chunkWidth
                    || localPosition.y < 0 || localPosition.y > chunkForHit.chunk.chunkData.chunkHeight)
                    return;
                //获取原位置方块
                Block tagetBlock = chunkForHit.chunk.chunkData.GetBlockForLocal(localPosition);
                if (tagetBlock == null)
                    return;
                //展示目标位置
                GameHandler.Instance.manager.playerTargetBlock.Show(targetBlockPosition, tagetBlock, tagetBlock.blockInfo.interactive_state == 1);
            }
        }
        else
        {
            //展示目标位置
            if (GameHandler.Instance.manager.playerTargetBlock)
                GameHandler.Instance.manager.playerTargetBlock.Hide();
        }
    }

    /// <summary>
    /// 使用道具 (默认是破坏)
    /// </summary>
    public virtual void TargetUse(ItemsBean itemsData, Vector3Int targetPosition)
    {
        //获取原位置方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block oldBlock, out BlockDirectionEnum oldBlockDirection, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        //如果原位置是空则不做处理
        if (oldBlock == null || oldBlock.blockType == BlockTypeEnum.None)
            return;
        ItemUseHandle(itemsData, targetPosition, oldBlock, oldBlockDirection, targetChunk);
    }

    /// <summary>
    /// 道具使用处理
    /// </summary>
    public virtual void ItemUseHandle(ItemsBean itemsData, Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk targetChunk)
    {

    }

    /// <summary>
    /// 破碎目标
    /// </summary>
    public virtual void TargetBreak(ItemsBean itemsData, Vector3Int targetPosition)
    {
        //获取原位置方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block oldBlock, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        //如果原位置是空则不做处理
        if (oldBlock == null || oldBlock.blockType == BlockTypeEnum.None)
            return;
        //如果是链接方块 则用链接方块的基础方块代替
        if (oldBlock.blockType == BlockTypeEnum.LinkChild)
        {
            BlockBean oldBlockData = targetChunk.GetBlockData(targetPosition - targetChunk.chunkData.positionForWorld);
            BlockMetaBaseLink oldeBlockMetaLinkData = Block.FromMetaData<BlockMetaBaseLink>(oldBlockData.meta);
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(oldeBlockMetaLinkData.GetBasePosition(), out oldBlock, out targetChunk);
        }

        //获取破坏值
        int breakDamage = GetBreakDamage(itemsData, oldBlock);
        //扣除道具耐久
        if (breakDamage > 0 && this is ItemBaseTool itemTool)
        {
            ItemsDetailsToolBean itemsDetailsTool = itemsData.GetMetaData<ItemsDetailsToolBean>();
            //如果已经没有耐久了 则不造成伤害
            if (itemsDetailsTool.life <= 0)
            {
                breakDamage = 0;
            }
            itemsDetailsTool.AddLife(-1);
            //保存数据
            itemsData.SetMetaData(itemsDetailsTool);
            //回调
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemsData);
        }

        BlockCptBreak BlockCptBreak = BlockHandler.Instance.BreakBlock(targetPosition, oldBlock, breakDamage);
        if (BlockCptBreak.blockLife <= 0)
        {
            //移除破碎效果
            BlockHandler.Instance.DestroyBreakBlock(targetPosition);
            //创建掉落
            ItemsHandler.Instance.CreateItemCptDrop(oldBlock, targetChunk, targetPosition);
            //移除该方块
            targetChunk.RemoveBlockForWorld(targetPosition);
        }
    }

    /// <summary>
    /// 获取道具破坏的伤害
    /// </summary>
    public virtual int GetBreakDamage(ItemsBean itemsData, Block breakBlock)
    {
        //检测是否能造成伤害
        bool canBreak = breakBlock.blockInfo.CheckCanBreak(itemsData.itemId);
        if (canBreak)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// 获取道具详情数据
    /// </summary>
    /// <returns></returns>
    public virtual ItemsDetailsBean GetItemsDetailsBean(long itemId)
    {
        return new ItemsDetailsBean();
    }
}