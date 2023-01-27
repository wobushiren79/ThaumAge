using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using System;
using System.Collections.Generic;

public class BlockTypeInfusionAltar : BlockBaseLinkLarge
{
    //元素检测范围
    public static int RangeElemental = 7;
    //基座材料检测范围
    public static int RangeMaterial = 5;
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
        RefreshObjModel(chunk, localPosition, blockMetaData.itemsShow);
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        //首先获取该方块的位置（有可能是子方块）
        Vector3Int targetWorldPosition = targetLocalPosition + targetChunk.chunkData.positionForWorld;
        GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        Vector3Int basePosition = blockMetaData.GetBasePosition();
        //如果是基础点（基座的位置 则执行基座逻辑）
        if (basePosition == targetWorldPosition)
        {
            return TargetUseBlockForBase(user, itemData, targetChunk, targetLocalPosition);
        }
        //如果是矩阵
        else if ((basePosition + Vector3Int.up * 2) == targetWorldPosition)
        {
            return TargetUseBlockForStarWork(user, itemData, targetChunk, targetLocalPosition);
        }
        return false;
    }

    /// <summary>
    /// 对着基座使用 放置道具
    /// </summary>
    /// <returns></returns>
    public virtual bool TargetUseBlockForBase(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
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
            Vector3Int targetWorldPosition = targetLocalPosition + targetChunk.chunkData.positionForWorld;
            //先让基座上的物品掉落
            ItemDropBean itemDropData = new ItemDropBean(blockMetaData.itemsShow, ItemDropStateEnum.DropPick, targetWorldPosition + new Vector3(0.5f, 1.5f, 0.5f), Vector3.up * 1.5f);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
            blockMetaData.itemsShow = null;
        }

        //保存数据
        blockData.SetBlockMeta(blockMetaData);
        targetChunk.SetBlockData(blockData);

        RefreshObjModel(targetChunk, targetLocalPosition, blockMetaData.itemsShow);
        return true;
    }

    /// <summary>
    /// 对着矩阵使用 放置道具
    /// </summary>
    /// <returns></returns>
    public virtual bool TargetUseBlockForStarWork(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        //如果是法杖
        if (itemsInfo.GetItemsType() == ItemsTypeEnum.Wand)
        {
            Vector3Int baseLocalPosition = targetLocalPosition - Vector3Int.up * 2;
            //首先检测基座上是否有物品
            GetBlockMetaData(targetChunk, baseLocalPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
            //没有道具
            if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
            {
                return true;
            }
            //判断该道具是否可以注魔
            bool canInfusion = InfusionAltarInfoCfg.CheckCanInfusion(blockMetaData.itemsShow.itemId);
            if (!canInfusion)
            {
                return true;
            }
            //获取注魔数据 会判断附近基座是否有符合条件的材料 如果没有则放弃
            InfusionAltarInfoBean infusionAltarInfo = InfusionAltarInfoCfg.GetInfusionTargetData(blockMetaData.itemsShow.itemId, targetChunk, baseLocalPosition);
            if (infusionAltarInfo == null)
            {
                return true;
            }
            //播放注魔开始特效
            SetInfusionAltarState(targetChunk, baseLocalPosition, 1);

            //重置并保存数据
            blockMetaData.InitData(infusionAltarInfo);
            blockData.SetBlockMeta(blockMetaData);
            targetChunk.SetBlockData(blockData, false);
            //开始注魔
            targetChunk.RegisterEventUpdate(baseLocalPosition, TimeUpdateEventTypeEnum.Sec);
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
        if (chunk == null)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfusionAltar blockMetaData);
        //没有道具
        if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            SetInfusionAltarState(chunk, localPosition, 0);
            return;
        }
        //每秒处理一次
        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;

        bool isFail = false;
        bool isSaveData = false;
        //首先吸收元素
        switch (blockMetaData.infusionPro)
        {
            case 0://===============================状态 0 未开始===============================
                blockMetaData.infusionPro = 1;
                blockMetaData.curInfusionElementalPosition = worldPosition;
                break;
            case 1://===============================状态 1 吸取元素===============================
                //如果还有需要的元素
                if (blockMetaData.listInfusionElemental.Count > 0)
                {
                    //获取需要的元素数据
                    NumberBean needElementalData = blockMetaData.listInfusionElemental[0];
                    //处理每一个有元素的
                    Action<Chunk, Block, Vector3Int> actionForHandleItemBlock = (itemChunk, itemBlock, itemLocalPosition) =>
                    {
                        //如果是源质罐子
                        if (blockMetaData.curInfusionElementalPosition == worldPosition)
                        {  
                            //=============================== 源质罐子 ===============================
                            if (itemBlock.blockType == BlockTypeEnum.WardedJar)
                            {
                                itemBlock.GetBlockMetaData(itemChunk, itemLocalPosition, out BlockBean itemBlockData, out BlockMetaWardedJar itemBlockMetaData);
                                //如果是需要的元素 并且还有剩余
                                if (itemBlockMetaData.elementalType == needElementalData.id && itemBlockMetaData.curElemental > 0)
                                {
                                    //设置当前吸元素的点
                                    blockMetaData.curInfusionElementalPosition = itemLocalPosition + itemChunk.chunkData.positionForWorld;
                                    //吸取一点元素
                                    needElementalData.number--;
                                    if (needElementalData.number <= 0)
                                    {
                                        blockMetaData.listInfusionElemental.Remove(needElementalData);
                                    }
                                    //减去原来元素容器里的元素
                                    itemBlockMetaData.AddElemental((ElementalTypeEnum)itemBlockMetaData.elementalType, -1, out int leftElementalNum);
                                    itemBlockData.SetBlockMeta(itemBlockMetaData);
                                    itemChunk.SetBlockData(itemBlockData);
                                    //刷新原来的元素容器
                                    BlockTypeWardedJar blockTypeWardedJarl = itemBlock as BlockTypeWardedJar;
                                    blockTypeWardedJarl.RefreshObjModel(itemChunk, itemLocalPosition, itemBlockMetaData);
                                    //创建连线粒子
                                    ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData((ElementalTypeEnum)itemBlockMetaData.elementalType);
                                    ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out Color ivColor);

                                    PlayLineEffect(blockMetaData.curInfusionElementalPosition + new Vector3(0.5f, 0.5f, 0.5f), worldPosition + new Vector3(0.5f, 2.5f, 0.5f), new Vector4(ivColor.r, ivColor.g, ivColor.b, ivColor.a), 5);
                                }
                                else
                                {
                                    blockMetaData.curInfusionElementalPosition = worldPosition;
                                }
                            }
                            //=============================== 奥术蒸馏器 ===============================
                            else if (itemBlock.blockType == BlockTypeEnum.ArcaneAlembic)
                            {
                                itemBlock.GetBlockMetaData(itemChunk, itemLocalPosition, out BlockBean itemBlockData, out BlockMetaArcaneAlembic itemBlockMetaData);
                                //如果是需要的元素 并且还有剩余
                                if (itemBlockMetaData.elementalData != null && itemBlockMetaData.elementalData.id == needElementalData.id &&  itemBlockMetaData.elementalData.number > 0)
                                {
                                    //设置当前吸元素的点
                                    blockMetaData.curInfusionElementalPosition = itemLocalPosition + itemChunk.chunkData.positionForWorld;
                                    //吸取一点元素
                                    needElementalData.number--;
                                    if (needElementalData.number <= 0)
                                    {
                                        blockMetaData.listInfusionElemental.Remove(needElementalData);
                                    }
                                    //减去原来元素容器里的元素
                                    itemBlockMetaData.AddElemental((ElementalTypeEnum)itemBlockMetaData.elementalData.id, -1);
                                    itemBlockData.SetBlockMeta(itemBlockMetaData);
                                    itemChunk.SetBlockData(itemBlockData);
   
                                    //创建连线粒子
                                    ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData((ElementalTypeEnum)itemBlockMetaData.elementalData.id);
                                    ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out Color ivColor);

                                    PlayLineEffect(blockMetaData.curInfusionElementalPosition + new Vector3(0.5f, 0.5f, 0.5f), worldPosition + new Vector3(0.5f, 2.5f, 0.5f), new Vector4(ivColor.r, ivColor.g, ivColor.b, ivColor.a), 5);
                                }
                                else
                                {
                                    blockMetaData.curInfusionElementalPosition = worldPosition;
                                }
                            }
                        }
                    };
                    //如果是还没有找到元素
                    if (blockMetaData.curInfusionElementalPosition == worldPosition)
                    {
                        GetRoundBlock(worldPosition, RangeElemental, RangeElemental - 2, RangeElemental, actionForHandleItemBlock);
                        //如果这个范围内没有找到可以装元素的容器
                        if (blockMetaData.curInfusionElementalPosition == worldPosition)
                        {
                            isFail = true;
                        }
                    }
                    else
                    {
                        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockMetaData.curInfusionElementalPosition, out Block itemElementalBlock, out Chunk itemElementalChunk);
                        if (itemElementalChunk != null)
                        {
                            Vector3Int itemLocalPosition = blockMetaData.curInfusionElementalPosition - itemElementalChunk.chunkData.positionForWorld;
                            //再这里赋值一下。因为再action里面有一个判断
                            blockMetaData.curInfusionElementalPosition = worldPosition;
                            actionForHandleItemBlock.Invoke(itemElementalChunk, itemElementalBlock, itemLocalPosition);
                        }
                        else
                        {
                            blockMetaData.curInfusionElementalPosition = worldPosition;
                        }
                    }
                }
                else
                {
                    //开始吸收材料
                    blockMetaData.infusionPro = 2;
                }
                break;
            case 2://===============================状态 2 吸取道具===============================
                if (blockMetaData.listInfusionMat.Count > 0)
                {
                    long itemInfusionId = blockMetaData.listInfusionMat[0];
                    bool hasInfusionItem = false;
                    Action<Chunk, Block, Vector3Int> actionForHandleItemBlock = (itemChunk, itemBlock, itemLocalPosition) =>
                    {
                        //获取基座上的物品
                        if (!hasInfusionItem && itemBlock.blockType == BlockTypeEnum.ArcanePedestal)
                        {
                            itemBlock.GetBlockMetaData(itemChunk, itemLocalPosition, out BlockBean itemBlockData, out BlockMetaArcanePedestal blockMetaArcanePedestal);
                            if (blockMetaArcanePedestal.itemsShow != null && blockMetaArcanePedestal.itemsShow.itemId != 0)
                            {
                                if (blockMetaArcanePedestal.itemsShow.itemId == itemInfusionId)
                                {
                                    blockMetaData.listInfusionMat.Remove(itemInfusionId);
                                    //如果是这个道具
                                    hasInfusionItem = true;

                                    //保存数据
                                    blockMetaArcanePedestal.itemsShow = null;
                                    itemBlockData.SetBlockMeta(blockMetaArcanePedestal);
                                    itemChunk.SetBlockData(itemBlockData);
                                    //刷新方块
                                    BlockTypeArcanePedestal blockTypeArcanePedestal = itemBlock as BlockTypeArcanePedestal;
                                    blockTypeArcanePedestal.RefreshObjModel(itemChunk, itemLocalPosition, blockMetaArcanePedestal.itemsShow);

                                    //播放粒子特效
                                    GameObject objItemShow = blockTypeArcanePedestal.GetItemShowObj(itemChunk, itemLocalPosition);
                                    MeshRenderer mrItemShow = objItemShow.GetComponent<MeshRenderer>();
                                    Color colorEffect = TextureUtil.GetPixel((Texture2D)mrItemShow.material.mainTexture, new Vector2Int(8, 8));
                                    PlayLineEffect(itemChunk.chunkData.positionForWorld + itemLocalPosition + new Vector3(0.5f, 1.25f, 0.5f), worldPosition + new Vector3(0.5f, 1f, 0.5f), new Vector4(colorEffect.r, colorEffect.g, colorEffect.b, 1), 2);     
                                }
                            }
                        }
                    };
                    GetRoundBlock(worldPosition, RangeMaterial, RangeMaterial, RangeMaterial, actionForHandleItemBlock);
                    if (hasInfusionItem == false)
                    {
                        isFail = true;
                    }
                }
                else
                {
                    //如果没有需要的材料了 说明注魔完成
                    blockMetaData.infusionPro = 0;
                    chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
                    SetInfusionAltarState(chunk, localPosition, 0);

                    blockMetaData.itemsShow.itemId = blockMetaData.infusionSuccessItemId;
                    blockMetaData.itemsShow.number = (int)blockMetaData.infusionSuccessItemNum;
                    isSaveData = true;

                    RefreshObjModel(chunk, localPosition, blockMetaData.itemsShow);
                    //播放粒子特效
                    PlaySuccessEffect(worldPosition + new Vector3(0.5f,0.5f,0.5f));
                }
                break;
        }
        //保存数据
        if (isFail)
        {
            blockMetaData.infusionFailTime++;
            //如果失败时间大于5秒则爆炸
            if (blockMetaData.infusionFailTime > 6)
            {
                chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
                SetInfusionAltarState(chunk, localPosition, 0);
                //播放失败粒子
                PlayFailEffect(localPosition + chunk.chunkData.positionForWorld + new Vector3(0.5f, 1.5f, 0.5f));
                //扣除基座道具
                blockMetaData.itemsShow = null;
                RefreshObjModel(chunk, localPosition, blockMetaData.itemsShow);
                isSaveData = true;
            }
            else
            {
                SetInfusionAltarState(chunk, localPosition, 2);
            }
        }
        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData, isSaveData);
    }

    /// <summary>
    /// 设置展示的物品
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, ItemsBean itemsData)
    {
        GameObject objItemShow = GetItemShowObj(chunk, localPosition);
        if (itemsData == null || itemsData.itemId == 0)
        {
            objItemShow.ShowObj(false);
        }
        else
        {
            objItemShow.ShowObj(true);
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            ItemCptShow itemCpt = objItemShow.GetComponent<ItemCptShow>();
            itemCpt.SetItem(itemsData, itemsInfo, 1);
        }
    }

    /// <summary>
    /// 获取展示的物品
    /// </summary>
    /// <returns></returns>
    public virtual GameObject GetItemShowObj(Chunk chunk, Vector3Int localPosition)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return null;
        Transform tfItemShow = objBlock.transform.Find("ItemShow");
        if (tfItemShow == null)
            return null;
        return tfItemShow.gameObject;
    }

    /// <summary>
    /// 设置注魔祭坛状态
    /// </summary>
    /// <param name="state">0默认普通 1注魔中（正常）  2注魔中（找不到材料和元素）</param>
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
        ColorUtility.TryParseHtmlString("#0F0029", out Color colorText);
        Vector4 colorEffect = new Vector4(0.065f,0,0.339f);
        switch (state)
        {
            case 1:
                matrixRotateSpeed = 5;
                matrixEmissionText = 200;
                tfEffect.gameObject.SetActive(true);
                break;
            case 2:
                matrixRotateSpeed = 5;
                colorText = Color.red;
                colorEffect = new Vector4(1,0,0,1);
                break;
            default:
                tfEffect.gameObject.SetActive(false);
                break;
        }

        mrMatrix.material.SetColor("_TextColor", colorText);
        mrMatrix.material.SetFloat("_RotateSpeed", matrixRotateSpeed);
        mrMatrix.material.DOFloat(matrixEmissionText, "_EmissionText1", 0.5f);

        veEffect.SetVector4("Color", colorEffect);

        tfMatrix.DOKill();
        tfMatrix.DOShakePosition(0.2f, 0.1f, 50);
    }

    /// <summary>
    /// 播放连线粒子
    /// </summary>
    public void PlayLineEffect(Vector3 startPosition, Vector3 targetPosition, Vector4 effectColor, float lifeTime)
    {
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Line_2;
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.timeForShow = 5;
        effectData.effectPosition = startPosition;
        effectData.isPlayInShow = false;

        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {
            Vector3 pos1 = Vector3.Lerp(startPosition, targetPosition, 0.2f);
            Vector3 pos2 = Vector3.Lerp(startPosition, targetPosition, 0.9f);

            VisualEffect visualEffect = effect.GetComponent<VisualEffect>();
            visualEffect.SetVector3("PosStart", startPosition);
            visualEffect.SetVector3("Pos1", pos1 + new Vector3(0, 0.5f, 0));
            visualEffect.SetVector3("Pos2", pos2 + new Vector3(0, 1f, 0));
            visualEffect.SetVector3("PosEnd", targetPosition);
            visualEffect.SetFloat("EffectSize", 0.1f);
            visualEffect.SetFloat("EffectStartWaitMin", 0f);
            visualEffect.SetFloat("EffectStartWaitMax", 0.05f);
            visualEffect.SetFloat("EffectLifetimeMin", lifeTime / 2f);
            visualEffect.SetFloat("EffectLifetimeMax", lifeTime);
            visualEffect.SetFloat("EffectNum", 80);
            visualEffect.SetVector4("ColorRandom1", effectColor);
            visualEffect.SetVector4("ColorRandom2", effectColor);
            visualEffect.SendEvent("OnPlay");

            EffectHandler.Instance.WaitExecuteSeconds(1, () =>
            {
                visualEffect.SendEvent("OnStop");
            });
        });
    }

    /// <summary>
    /// 播放失败粒子
    /// </summary>
    /// <param name="startPosition"></param>
    public void PlayFailEffect(Vector3 startPosition)
    {
        //播放爆炸音效
        AudioHandler.Instance.PlaySound(151, startPosition);
        //播放爆炸粒子特效
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Boom_1;
        effectData.effectPosition = startPosition;
        effectData.timeForShow = 0.5f;
        EffectHandler.Instance.ShowEffect(effectData);
    }

    /// <summary>
    /// 播放成功粒子
    /// </summary>
    /// <param name="startPosition"></param>
    public void PlaySuccessEffect(Vector3 startPosition)
    {
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Smoke_3;
        effectData.effectPosition = startPosition;
        effectData.timeForShow = 5;
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {
            //播放音效
            AudioHandler.Instance.PlaySound(3, startPosition);
        });
    }
}