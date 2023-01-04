using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    public virtual void UseForAnim(GameObject user, ItemsBean itemsData, ItemUseTypeEnum useType)
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
                    TargetUseL(player.gameObject, itemsData, targetPosition);
                }
                else
                {
                    TargetUseR(player.gameObject, itemsData, targetPosition, closePosition, direction);
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
                tagetBlock.ItemUseForSightTarget(targetBlockPosition);
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
    /// 破碎目标(鼠标左键)
    /// </summary>
    public virtual void TargetUseL(GameObject user, ItemsBean itemsData, Vector3Int targetPosition)
    {
        //获取原位置方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        //如果原位置是空则不做处理
        if (targetBlock == null || targetBlock.blockType == BlockTypeEnum.None)
            return;
        //如果是链接方块 则用链接方块的基础方块代替
        if (targetBlock.blockType == BlockTypeEnum.LinkChild)
        {
            BlockBean oldBlockData = targetChunk.GetBlockData(targetPosition - targetChunk.chunkData.positionForWorld);
            BlockMetaBaseLink oldeBlockMetaLinkData = Block.FromMetaData<BlockMetaBaseLink>(oldBlockData.meta);
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(oldeBlockMetaLinkData.GetBasePosition(), out targetBlock, out targetChunk);
            targetPosition = oldeBlockMetaLinkData.GetBasePosition();
        }

        //获取破坏值
        int breakDamage = GetBreakDamage(itemsData, targetBlock);
        //扣除道具耐久
        if (breakDamage > 0 && this is ItemBaseTool itemTool)
        {
            ItemMetaTool itemsDetailsTool = itemsData.GetMetaData<ItemMetaTool>();
            //如果已经没有耐久了 则不造成伤害
            if (itemsDetailsTool.curDurability <= 0)
            {
                breakDamage = 0;
            }
            itemsDetailsTool.AddLife(-breakDamage);
            //保存数据
            itemsData.SetMetaData(itemsDetailsTool);
            //回调
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemsData);
        }
        //通知
        targetBlock.TargetBreakBlock(user, itemsData, targetChunk, targetPosition);

        BlockCptBreak BlockCptBreak = BlockHandler.Instance.BreakBlock(targetPosition, targetBlock, breakDamage);
        if (BlockCptBreak.blockLife <= 0)
        {
            //移除破碎效果
            BlockHandler.Instance.DestroyBreakBlock(targetPosition);
            //创建掉落
            ItemsHandler.Instance.CreateItemCptDrop(targetBlock, targetChunk, targetPosition);
            //移除该方块
            targetChunk.RemoveBlockForWorld(targetPosition);
            //创建粒子特效
            BlockCptBreak.PlayBlockCptBreakEffect(targetBlock, targetPosition);
            //播放破碎音效
            PlayItemDropSound(itemsData);
        }
        else
        {
            PlayItemSoundUseL(itemsData);
        }
    }

    /// <summary>
    /// 使用道具（鼠标右键）
    /// </summary>
    public virtual bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk targetChunk);
        //触发被使用方块的事件调用
        bool blockUseStop = false;
        if (targetChunk != null && targetBlock != null)
        {
            blockUseStop = targetBlock.TargetUseBlock(user, itemData, targetChunk, targetPosition);
        }
        PlayItemSoundUseR(itemData);
        //返回是否被方块的右键点击事件阻挡
        return blockUseStop;
    }



    /// <summary>
    /// 播放方块掉落声音
    /// </summary>
    public virtual void PlayItemDropSound(ItemsBean itemsData)
    {
        //播放掉落音效
        AudioHandler.Instance.PlaySound(301);
    }

    /// <summary>
    /// 播放道具破坏音效（鼠标左键） 
    /// </summary>
    public virtual void PlayItemSoundUseL(ItemsBean itemsData)
    {
        //播放破坏音效
        if (itemsData == null)
        {
            AudioHandler.Instance.PlaySound(351);
            return;
        }
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        if (itemsInfo != null && !itemsInfo.sound_use.IsNull())
        {
            int soundId = int.Parse(itemsInfo.sound_use);
            AudioHandler.Instance.PlaySound(soundId);
        }
        else
        {
            AudioHandler.Instance.PlaySound(351);
        }
    }

    /// <summary>
    /// 播放道具使用声音（鼠标右键）
    /// </summary>
    public virtual void PlayItemSoundUseR(ItemsBean itemsData)
    {
        //播放破坏音效
        if (itemsData == null)
        {
            AudioHandler.Instance.PlaySound(351);
            return;
        }
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        if (itemsInfo != null && !itemsInfo.sound_use.IsNull())
        {
            int soundId = int.Parse(itemsInfo.sound_use);
            AudioHandler.Instance.PlaySound(soundId);
        }
        else
        {
            AudioHandler.Instance.PlaySound(351);
        }
    }

    /// <summary>
    /// 获取道具破坏的伤害
    /// </summary>
    public virtual int GetBreakDamage(ItemsBean itemsData, Block breakBlock)
    {
        //检测是否能造成伤害 
        breakBlock.blockInfo.CheckCanBreak(itemsData.itemId, out bool canBreak, out bool isAdditionBreak);
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        AttributeBean attributeData = itemsInfo.GetAttributeData();

        //伤害默认为1
        int breakSpeed = 1;
        if (attributeData.HasAttributeValue(AttributeTypeEnum.BreakSpeed) && isAdditionBreak)
        {
            breakSpeed = attributeData.GetAttributeValue(AttributeTypeEnum.BreakSpeed);
        }

        if (canBreak)
            return breakSpeed;
        else
            return 0;
    }

    /// <summary>
    /// 获取初始化meta数据
    /// </summary>
    /// <returns></returns>
    public virtual ItemBaseMeta GetInitMetaData<T>(long itemId) where T : ItemBaseMeta
    {
        ItemBaseMeta data = ReflexUtil.CreateInstance<T>();
        data.itemId = itemId;
        return data;
    }

    /// <summary>
    /// 设置道具图标
    /// </summary>
    public virtual void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        SetItemIcon(this, itemData, itemsInfo, ivTarget, srTarget);
    }

    public static void SetItemIcon(Item item, ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(itemsInfo.icon_key, (spIcon) =>
        {
            if (spIcon == null)
            {
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    if (ivTarget != null)
                    {
                        ivTarget.sprite = spIcon;
                    }
                    if (srTarget != null)
                    {
                        srTarget.sprite = spIcon;
                    }
                });
            }
            else
            {
                if (ivTarget != null)
                {
                    ivTarget.sprite = spIcon;
                }
                if (srTarget != null)
                {
                    srTarget.sprite = spIcon;
                }
            }
        });
        if (ivTarget != null)
        {
            ivTarget.color = item.GetItemIconColor(itemData, itemsInfo);
        }
        if (srTarget != null)
        {
            srTarget.color = item.GetItemIconColor(itemData, itemsInfo);
        }
    }

    /// <summary>
    /// 设置道具名字
    /// </summary>
    public virtual void SetItemName(Text tvTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        SetItemName(tvTarget, itemsInfo);
    }

    public static void SetItemName(Text tvTarget, ItemsInfoBean itemsInfo)
    {
        tvTarget.text = itemsInfo.GetName();
    }

    /// <summary>
    /// 获取道具图标贴图
    /// </summary>
    public virtual void GetItemIconTex(ItemsBean itemData, ItemsInfoBean itemsInfo, Action<Texture2D> callBack)
    {
        ItemsHandler.Instance.manager.GetItemsIconById(itemsInfo.id, (data) =>
        {
            if (data == null)
            {
                IconHandler.Instance.GetUnKnowSprite((unknowSprite)=>
                {
                    Texture2D itemTex = TextureUtil.SpriteToTexture2D(unknowSprite);
                    callBack?.Invoke(itemTex);
                });
            }
            else
            {
                Texture2D itemTex = TextureUtil.SpriteToTexture2D(data);
                callBack?.Invoke(itemTex);
            }
        });
    }

    /// <summary>
    /// 获取道具图标颜色
    /// </summary>
    /// <returns></returns>
    public virtual Color GetItemIconColor(ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        return itemsInfo.GetItemsColor();
    }
}