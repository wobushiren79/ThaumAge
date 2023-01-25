﻿using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class ItemClassElementalPowderThaum : Item
{
    /// <summary>
    /// 右键使用
    /// </summary>
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk taragetChunk);
        //如果靠近得方块有区块
        if (taragetChunk != null)
        {
            //如果不是空方块 则不放置(液体则覆盖放置)
            if (targetBlock == null)
                return false;
            //将建议制造台变成奥术制造台

            switch (targetBlock.blockType)
            {
                case BlockTypeEnum.CraftingTableSimple:
                    HandleForCraftingTableArcane(targetPosition, targetBlock, targetBlockDirection, taragetChunk);
                    break;
                case BlockTypeEnum.RunicMatrix:
                    HandleForRunicMatrix(targetPosition, targetBlock, targetBlockDirection, taragetChunk);
                    break;
                default:
                    return false;
            }
            //扣除道具
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.AddItems(itemData, -1);
            //刷新UI
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
            //播放音效
            AudioHandler.Instance.PlaySound(1101, targetPosition);
        }
        return false;
    }

    /// <summary>
    /// 处理-奥术制造台
    /// </summary>
    protected void HandleForCraftingTableArcane(Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk taragetChunk)
    {
        PlayThrowEffect();
        EffectHandler.Instance.WaitExecuteSeconds(2, () =>
        {
            //获取目标方块
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out targetBlock, out targetBlockDirection, out taragetChunk);
            if (taragetChunk != null && targetBlock.blockType == BlockTypeEnum.CraftingTableSimple)
            {
                taragetChunk.SetBlockForLocal(targetPosition - taragetChunk.chunkData.positionForWorld, BlockTypeEnum.CraftingTableArcane, targetBlockDirection);
                //播放音效
                AudioHandler.Instance.PlaySound(3, targetPosition);
            }
        });
        //播放粒子特效
        EffectBean effectSmoke = new EffectBean();
        effectSmoke.effectType = EffectTypeEnum.Visual;
        effectSmoke.effectName = EffectInfo.Effect_Change_1;
        effectSmoke.effectPosition = new Vector3(0.5f + targetPosition.x, 0.5f + targetPosition.y, 0.5f + targetPosition.z);
        effectSmoke.timeForShow = 5;
        EffectHandler.Instance.ShowEffect(effectSmoke, (effect) =>
        {
            VisualEffect itemEffect = effect.listVE[0];
            itemEffect.SetVector3("Size", new Vector3(1, 1, 1));
            itemEffect.SetFloat("EffectSmokeNum", 250);
            itemEffect.SetFloat("EffectStarNum", 100);
        });
    }

    /// <summary>
    /// 处理 奥术矩阵
    /// </summary>
    protected void HandleForRunicMatrix(Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk taragetChunk)
    {
        //检测是否能放下这个多方块结构
        BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(BuildingTypeEnum.InfusionAltar);
        Vector3Int basePosition = targetPosition - Vector3Int.up * 2;
        if (!buildingInfo.CheckCanSetLinkLargeBuilding(basePosition))
        {
            return;
        }
        PlayThrowEffect();
        EffectHandler.Instance.WaitExecuteSeconds(2, () =>
        {
            //获取目标方块
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out targetBlock, out targetBlockDirection, out taragetChunk);
            if (taragetChunk != null && targetBlock.blockType == BlockTypeEnum.RunicMatrix)
            {       
                //检测是否能放下这个多方块结构
                BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(BuildingTypeEnum.InfusionAltar);
                if (!buildingInfo.CheckCanSetLinkLargeBuilding(basePosition))
                {
                    return;
                }
                BlockMetaBaseLink blockMetaLinkData = new BlockMetaBaseLink();
                blockMetaLinkData.level = 0;
                blockMetaLinkData.linkBasePosition = new Vector3IntBean(basePosition);
                blockMetaLinkData.isBreakAll = false;
                blockMetaLinkData.isBreakMesh = false;
                blockMetaLinkData.baseBlockType = targetBlock.blockInfo.id;

                taragetChunk.SetBlockForLocal(basePosition, BlockTypeEnum.InfusionAltar, targetBlockDirection, blockMetaLinkData.ToJson());
                //播放音效
                AudioHandler.Instance.PlaySound(3, targetPosition);
            }
        });
        //播放粒子特效
        EffectBean effectSmoke = new EffectBean();
        effectSmoke.effectType = EffectTypeEnum.Visual;
        effectSmoke.effectName = EffectInfo.Effect_Change_1;
        effectSmoke.effectPosition = new Vector3(0.5f + targetPosition.x, 0.5f + targetPosition.y - 1f, 0.5f + targetPosition.z);
        effectSmoke.timeForShow = 5;
        EffectHandler.Instance.ShowEffect(effectSmoke,(effect)=> 
        {
            VisualEffect itemEffect = effect.listVE[0];
            itemEffect.SetVector3("Size",new Vector3(3,3,3));
            itemEffect.SetFloat("EffectSmokeNum", 250 * 5);
            itemEffect.SetFloat("EffectStarNum", 100 * 5);
        });
    }

    /// <summary>
    /// 播放抛粉特效
    /// </summary>
    protected void PlayThrowEffect()
    {
        Player player = GameHandler.Instance.manager.player;
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 playerPosition = player.transform.position;
        //播放粒子特效
        EffectBean effectSmoke = new EffectBean();
        effectSmoke.effectType = EffectTypeEnum.Visual;
        effectSmoke.effectName = EffectInfo.Effect_Throw_1;
        effectSmoke.effectPosition = playerPosition + new Vector3(0,1.5f,0);
        effectSmoke.timeForShow = 3;
        EffectHandler.Instance.ShowEffect(effectSmoke, (effect) =>
        {
            VisualEffect itemEffect = effect.listVE[0];
            itemEffect.transform.eulerAngles = mainCamera.transform.eulerAngles;
        });
        
    }
}