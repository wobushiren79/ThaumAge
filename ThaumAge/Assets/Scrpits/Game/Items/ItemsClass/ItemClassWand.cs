using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassWand : Item
{
    /// <summary>
    /// 设置道具图标
    /// </summary>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        //设置杖柄
        if (itemMetaBuckets.rodId != 0)
        {
            //设置图标
            ItemsInfoBean itemsInfoForRod = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.rodId);
            base.SetItemIcon(itemData, itemsInfoForRod, ivTarget, srTarget);
        }
        //设置杖端
        if (itemMetaBuckets.capId != 0)
        {
            GameObject objSomething = null;
            if (ivTarget != null)
            {
                objSomething = ItemsHandler.Instance.Instantiate(ivTarget.gameObject, ivTarget.gameObject);
            }
            if (srTarget != null)
            {
                objSomething = ItemsHandler.Instance.Instantiate(srTarget.gameObject, srTarget.gameObject);
            }
            if (objSomething == null)
                return;

            objSomething.ShowObj(true);

            Image ivSomething = objSomething.GetComponent<Image>();
            SpriteRenderer srSomething = objSomething.GetComponent<SpriteRenderer>();

            //设置图标
            ItemsInfoBean itemsInfoForCap = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
            IconHandler.Instance.manager.GetItemsSpriteByName($"{itemsInfoForCap.icon_key}_1", (spIcon) =>
            {
                if (spIcon == null)
                {
                    IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                    {
                        if (ivSomething != null)
                        {
                            ivSomething.sprite = spIcon;
                        }
                        if (srSomething != null)
                        {
                            srSomething.sprite = spIcon;
                            srSomething.sortingOrder = 1;
                        }
                    });
                }
                else
                {
                    if (ivSomething != null)
                    {
                        ivSomething.sprite = spIcon;
                    }
                    if (srSomething != null)
                    {
                        srSomething.sprite = spIcon;
                        srSomething.sortingOrder = 1;
                    }
                }
            });
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    public override void SetItemName(Text tvTarget, ItemsBean itemData, ItemsInfoBean itemsInfo)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        ItemsInfoBean capInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
        ItemsInfoBean rodInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.rodId);
        string name = $"{capInfo.GetName()}{rodInfo.GetName()}{itemsInfo.GetName()}";
        tvTarget.text = name;
    }


    public override void GetItemIconTex(ItemsBean itemData, ItemsInfoBean itemsInfo, Action<Texture2D> callBack)
    {
        ItemMetaWand itemMetaBuckets = itemData.GetMetaData<ItemMetaWand>();
        ItemsHandler.Instance.manager.GetItemsIconById(itemMetaBuckets.rodId, (rodSprite) =>
        {
            //设置图标
            ItemsInfoBean itemsInfoForCap = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.capId);
            IconHandler.Instance.manager.GetItemsSpriteByName($"{itemsInfoForCap.icon_key}_1", (capSprite) =>
            {
                if (capSprite != null)
                {
                    Texture2D itemTex = TextureUtil.SpriteToTexture2D(new Sprite[] { rodSprite, capSprite });
                    callBack?.Invoke(itemTex);
                }
                else
                {
                    Debug.LogError($"没有找到icon_key为 {itemsInfoForCap.icon_key}_1 的sprite");
                }
            });
        });
    }
    public override void UseForPlayer(Player player, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {
        if (itemUseType == ItemUseTypeEnum.Right)
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
                    TargetUseR(player.gameObject, itemsData, targetPosition, closePosition, direction);
                    return;
                }
            }
            //如果没有方块则也需要触发使用
            TargetUseR(player.gameObject, itemsData, Vector3Int.zero, Vector3Int.zero, BlockDirectionEnum.None);
        }
        else
        {
            base.UseForPlayer(player, itemsData, itemUseType);
        }
    }

    /// <summary>
    /// 右键使用法杖
    /// </summary>
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;
        ItemMetaWand itemMetaWand = itemData.GetMetaData<ItemMetaWand>();
        if (itemMetaWand.rodId == 0 || itemMetaWand.capId == 0)
        {
            return false;
        }
        if (itemMetaWand.listMagicCore.IsNull())
        {
            return false;
        }
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

        int indexSelectMagic = userData.indexForShortcutsMagic;
        if (indexSelectMagic >= itemMetaWand.listMagicCore.Count)
        {
            userData.indexForShortcutsMagic = itemMetaWand.listMagicCore.Count - 1;
            indexSelectMagic = userData.indexForShortcutsMagic;
        }

        ItemsBean itemMagicCoreData = itemMetaWand.listMagicCore[indexSelectMagic];
        ItemMetaMagicCore itemMetaMagicData = itemMagicCoreData.GetMetaData<ItemMetaMagicCore>();

        //首先判断魔力是否足够
        int manaCost = 1;
        bool hasEnoughMagic = itemMetaWand.HasEnoughMana(manaCost);
        if (!hasEnoughMagic)
        {
            hasEnoughMagic = userData.characterData.creatureStatus.HasEnoughMagic(manaCost);
            if (!hasEnoughMagic)
            {
                return true;
            }
            else
            {
                //消耗自身魔力
                userData.characterData.creatureStatus.ManaChange(-manaCost);
                //刷新UI
                EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
            }
        }
        else
        {
            //消耗法杖魔力
            itemMetaWand.ManaChange(-manaCost);
            //设置数据
            itemData.SetMetaData(itemMetaWand);
            //刷新UI
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
        }


        MagicBean magicData = new MagicBean(itemMetaMagicData);
        magicData.createPosition = user.transform.position + Vector3.up * 1.5f;
        magicData.direction = Camera.main.transform.forward;
        magicData.createTargetId = user.GetInstanceID();
        magicData.createTargetObj = user;
        MagicHandler.Instance.CreateMagic(magicData);

        //播放法杖使用音效
        PlayItemSoundUse(itemData, ItemUseTypeEnum.Right);

        return false;
    }
}