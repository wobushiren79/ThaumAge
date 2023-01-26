using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class BlockTypeInfusionAltar : BlockBaseLinkLarge
{

    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
        RefreshObjModel(chunk, localPosition, blockMetaData.itemsShow);
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        //首先获取该方块的位置（有可能是子方块）
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        GetBlockMetaData(targetChunk, blockLocalPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        Vector3Int basePosition = blockMetaData.GetBasePosition();
        //如果是基础点（基座的位置 则执行基座逻辑）
        if (basePosition == targetWorldPosition)
        {
            return TargetUseBlockForBase(user, itemData, targetChunk, targetWorldPosition);
        }
        //如果是矩阵
        else if ((basePosition + Vector3Int.up * 2) == targetWorldPosition)
        {
            return TargetUseBlockForStarWork(user, itemData, targetChunk, targetWorldPosition);
        }
        return false;
    }

    /// <summary>
    /// 对着基座使用 放置道具
    /// </summary>
    /// <returns></returns>
    public virtual bool TargetUseBlockForBase(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        GetBlockMetaData(targetChunk, blockLocalPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
        //如果基座上没有物品
        if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
        {
            //如果是空手
            if (itemData == null || itemData.itemId == 0)
            {
                return false;
            }
            //如果不是空手
            else
            {
                //如果能放置
                blockMetaData.itemsShow = new ItemsBean(itemData.itemId, 1, itemData.meta);
                //扣除道具
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                userData.AddItems(itemData, -1);
                EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
            }
        }
        //如果基座上有物品
        else
        {
            //先让基座上的物品掉落
            ItemDropBean itemDropData = new ItemDropBean(blockMetaData.itemsShow, ItemDropStateEnum.DropPick, targetWorldPosition + new Vector3(0.5f, 1.5f, 0.5f), Vector3.up * 1.5f);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
            blockMetaData.itemsShow = null;
        }

        //保存数据
        blockData.SetBlockMeta(blockMetaData);
        targetChunk.SetBlockData(blockData);

        RefreshObjModel(targetChunk, blockLocalPosition, blockMetaData.itemsShow);
        return true;
    }

    /// <summary>
    /// 对着矩阵使用 放置道具
    /// </summary>
    /// <returns></returns>
    public virtual bool TargetUseBlockForStarWork(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        //如果是法杖
        if (itemsInfo.GetItemsType() == ItemsTypeEnum.Wand)
        {
            Vector3Int basePosition = targetWorldPosition - Vector3Int.up * 2;
            //首先检测基座上是否有物品
            GetBlockMetaData(targetChunk, basePosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
            //没有道具
            if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
            {
                return true;
            }
            //播放注魔开始特效
            SetInfusionAltarState(targetChunk, basePosition, 1);
            //判断该道具是否可以注魔
            //开始注魔
            targetChunk.RegisterEventUpdate(basePosition, TimeUpdateEventTypeEnum.Sec);
        }
        return true;
    }

    /// <summary>
    /// 注魔流程
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
        //没有道具
        if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            SetInfusionAltarState(chunk, localPosition, 0);
            return;
        }
    }

    /// <summary>
    /// 设置展示的物品
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, ItemsBean itemsData)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfItemShow = objBlock.transform.Find("ItemShow");
        if (itemsData == null || itemsData.itemId == 0)
        {
            tfItemShow.ShowObj(false);
        }
        else
        {
            tfItemShow.ShowObj(true);
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            ItemCptShow itemCpt = tfItemShow.GetComponent<ItemCptShow>();
            itemCpt.SetItem(itemsData, itemsInfo, 1);
        }
    }


    /// <summary>
    /// 设置注魔祭坛状态
    /// </summary>
    /// <param name="state">0默认普通 1开始注魔</param>
    public void SetInfusionAltarState(Chunk chunk, Vector3Int localPosition, int state)
    {
        if (chunk == null)
            return;
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfEffect = objBlock.transform.Find("Effect_Show_1");
        Transform tfMatrix = objBlock.transform.Find("BlockInfusionAltar_2");

        MeshRenderer mrMatrix = tfMatrix.GetComponent<MeshRenderer>();
        VisualEffect veEffect = tfEffect.GetComponent<VisualEffect>();
        float matrixRotateSpeed = 1;
        float matrixEmissionText = 10;
        mrMatrix.material.SetFloat("_EmissionText1", matrixEmissionText);
        switch (state)
        {
            case 1:
                matrixRotateSpeed = 5;
                matrixEmissionText = 200;
                tfEffect.gameObject.SetActive(true);
                break;
            default:
                tfEffect.gameObject.SetActive(false);
                break;
        }

        mrMatrix.material.SetFloat("_RotateSpeed", matrixRotateSpeed);
        mrMatrix.material.DOFloat(matrixEmissionText, "_EmissionText1", 1f);

        tfMatrix.DOKill();
        tfMatrix.DOShakePosition(0.2f,0.1f,50);
    }
}