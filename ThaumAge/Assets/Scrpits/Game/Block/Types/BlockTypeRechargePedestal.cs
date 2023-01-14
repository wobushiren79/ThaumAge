using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;

public class BlockTypeRechargePedestal : Block
{
    protected static Color colorRechargeStart = new Color(1f, 0.1f, 0f);
    protected static Color colorRechargeEnd = new Color(0f, 0.5f, 1f);

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        GetMagicInstrumentData(chunk, localPosition, out BlockBean blockData, out BlockMetaRechargePedestal blockMetaRecharge, out ItemMetaMagicInstrument itemMetaMagicInstrument);
        //添加魔力
        if (itemMetaMagicInstrument == null)
        {
            //取消充能
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }

        int curMana = itemMetaMagicInstrument.ManaChange(1);
        //刷新魔力进度
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        float manaPro = itemMetaMagicInstrument.GetManaPro();

        //保存数据 (只修改魔力属性)
        blockMetaRecharge.itemsRecharge.meta = JsonUtil.ChangeJson(blockMetaRecharge.itemsRecharge.meta, "curMana", curMana);
        //blockMetaRecharge.itemsRecharge.SetMetaData(itemMetaMagicInstrument);
        blockData.SetBlockMeta(blockMetaRecharge);
        chunk.SetBlockData(blockData);

        //修改进度
        SetRechargePro(objBlock, manaPro);

        //如果进度大于1就不充能了
        if (manaPro >= 1)
        {
            //取消充能
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
    }

    public override List<ItemsBean> GetDropItems(BlockBean blockData = null)
    {
        List<ItemsBean> listDropItem = base.GetDropItems(blockData);
        //再加上法器
        if (blockData != null)
        {
            BlockMetaRechargePedestal blockMetaRecharge = blockData.GetBlockMeta<BlockMetaRechargePedestal>();
            if (blockMetaRecharge != null && blockMetaRecharge.itemsRecharge != null && blockMetaRecharge.itemsRecharge.itemId != 0)
            {
                listDropItem.Add(blockMetaRecharge.itemsRecharge);
            }
        }
        //加一个自己
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockData.GetBlockType());
        listDropItem.Add(new ItemsBean(itemsInfo.id, 1, null));
        return listDropItem;
    }

    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);

        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaRechargePedestal blockMetaRecharge);
        SetRechargeItem(chunk, localPosition, blockMetaRecharge.itemsRecharge);
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        GetBlockMetaData(targetChunk, blockLocalPosition, out BlockBean blockData, out BlockMetaRechargePedestal blockMetaRecharge);

        //如果基座上没有物品
        if (blockMetaRecharge.itemsRecharge == null || blockMetaRecharge.itemsRecharge.itemId == 0)
        {
            //如果是空手
            if (itemData == null || itemData.itemId == 0)
            {
                return false;
            }
            //如果不是空手
            else
            {
                //是否能放置
                ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
                if (itemsInfo.GetItemsType() != ItemsTypeEnum.Wand)
                {
                    return false;
                }
                //如果能放置
                blockMetaRecharge.itemsRecharge = new ItemsBean(itemData);
                //扣除道具
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                userData.AddItems(itemData, -1);
                EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
                //开始充能
                targetChunk.RegisterEventUpdate(blockLocalPosition, TimeUpdateEventTypeEnum.Sec);
            }
        }
        //如果基座上有物品
        else
        {
            //先让基座上的物品掉落
            ItemDropBean itemDropData = new ItemDropBean(blockMetaRecharge.itemsRecharge, ItemDropStateEnum.DropPick, targetWorldPosition + new Vector3(0.5f, 1.5f, 0.5f), Vector3.up * 1.5f);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
            blockMetaRecharge.itemsRecharge = null;
            //取消充能
            targetChunk.UnRegisterEventUpdate(blockLocalPosition, TimeUpdateEventTypeEnum.Sec);
        }

        //保存数据
        blockData.SetBlockMeta(blockMetaRecharge);
        targetChunk.SetBlockData(blockData);

        SetRechargeItem(targetChunk, blockLocalPosition, blockMetaRecharge.itemsRecharge);
        return true;
    }

    /// <summary>
    /// 设置魔法核心
    /// </summary>
    public void SetRechargeItem(Chunk chunk, Vector3Int localPosition, ItemsBean itemsData)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfItemShow = objBlock.transform.Find("ItemShow");
        Transform tfShield = objBlock.transform.Find("Shield");
        if (itemsData == null || itemsData.itemId == 0)
        {
            tfItemShow.ShowObj(false);
            tfShield.ShowObj(false);
        }
        else
        {
            tfShield.ShowObj(true);
            tfItemShow.ShowObj(true);
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            ItemCptShow itemCpt = tfItemShow.GetComponent<ItemCptShow>();
            itemCpt.SetItem(itemsData, itemsInfo, 1);

            MeshRenderer mrShield = tfShield.GetComponent<MeshRenderer>();
            Material shieldMat = mrShield.material;
            shieldMat.SetColor("_EdgeColor", new Color(0f, 0.6f, 1f));
            shieldMat.SetFloat("_EdgeStrength", 2f);
            shieldMat.SetFloat("_Size", 0f);
            shieldMat.SetFloat("_Emission", 5f);
            shieldMat.SetFloat("_ScaleSpeed", 1f);
            shieldMat.SetFloat("_ScaleSize", 0.02f);

            GetMagicInstrumentData(chunk, localPosition, out BlockBean blockData, out BlockMetaRechargePedestal blockMetaRecharge, out ItemMetaMagicInstrument itemMetaMagicInstrument);
            float manaPro = itemMetaMagicInstrument.GetManaPro();
            SetRechargePro(objBlock, manaPro);
        }
    }

    /// <summary>
    /// 设置充能进度
    /// </summary>
    /// <param name="pro"></param>
    public void SetRechargePro(GameObject objBlock, float pro)
    {
        if (objBlock == null)
            return;
        Transform tfShield = objBlock.transform.Find("Shield");
        MeshRenderer mrShield = tfShield.GetComponent<MeshRenderer>();
        Material shieldMat = mrShield.material;

        Color colorShield = Color.Lerp(colorRechargeStart, colorRechargeEnd, pro);
        shieldMat.SetColor("_EdgeColor", colorShield);
        //shieldMat.DOKill();
        //shieldMat.DOColor(colorShield, "_EdgeColor", 0.5f);
    }

    /// <summary>
    /// 获取魔力道具的魔力数据
    /// </summary>
    /// <returns></returns>
    public bool GetMagicInstrumentData(Chunk chunk, Vector3Int localPosition, out BlockBean blockData, out BlockMetaRechargePedestal blockMetaRecharge, out ItemMetaMagicInstrument itemMetaMagicInstrument)
    {
        blockData = chunk.GetBlockData(localPosition);
        blockMetaRecharge = null;
        itemMetaMagicInstrument = null;
        if (blockData == null)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return false;
        }
        blockMetaRecharge = blockData.GetBlockMeta<BlockMetaRechargePedestal>();
        if (blockMetaRecharge == null || blockMetaRecharge.itemsRecharge == null || blockMetaRecharge.itemsRecharge.itemId == 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return false;
        }
        //获取魔力数据
        itemMetaMagicInstrument = blockMetaRecharge.itemsRecharge.GetMetaData<ItemMetaMagicInstrument>();
        if (itemMetaMagicInstrument == null)
        {
            itemMetaMagicInstrument = new ItemMetaMagicInstrument();
        }
        return true;
    }
}