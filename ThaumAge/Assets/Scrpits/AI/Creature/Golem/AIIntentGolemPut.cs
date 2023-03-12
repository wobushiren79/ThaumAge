using UnityEditor;
using UnityEngine;

public class AIIntentGolemPut : AIBaseIntent
{
    //拾取间隔
    protected float timeUpdateForPutItems = 0;
    protected float timeForPutItems = 1;

    //状态 1走向标记容器 2放入容器 
    protected int putStatus = 0;
    //目标拾取物
    protected ItemCptDrop targetItemDrop;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIGolemEntity aiGolemEntity = aiEntity as AIGolemEntity;
        //播放闲置动画
        aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);

        putStatus = 0;
        timeUpdateForPutItems = 0;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        AIGolemEntity aiGolemEntity = aiEntity as AIGolemEntity;
        switch (putStatus)
        {
            case 0:
                HandleForStart(aiGolemEntity);
                break;
            case 1:
                HandleForMoveTarget(aiGolemEntity);
                break;
            case 2:
                HandleForPutItems(aiGolemEntity);
                break;
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForPutItems = 0;
        putStatus = 0;
    }

    /// <summary>
    /// 处理-开始
    /// </summary>
    public void HandleForStart(AIGolemEntity aiGolemEntity)
    {
        //傀儡实物
        CreatureCptBaseGolem creatureGolem = aiGolemEntity.creatureCpt as CreatureCptBaseGolem;
        ItemMetaGolem itemMetaGolem = creatureGolem.golemMetaData;
        //如果背包空了
        if (itemMetaGolem.bagData.CheckIsEmpty())
        {
            aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
            return;
        }
        //如果没有这个核心
        itemMetaGolem.GetGolemCore(4400003, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
        if (!itemMetaGolem.CheckGolemCore(aiGolemEntity, itemGolemCore, itemMetaGolemCore))
        {
            aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
            return;
        }
        //向绑定的位置移动
        aiGolemEntity.aiNavigation.SetMovePosition(itemMetaGolemCore.bindBlockWorldPosition+new Vector3(0.5f,0.5f,0.5f));

        //播放闲置动画
        aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
        putStatus = 1;
    }

    /// <summary>
    /// 处理-走向容器
    /// </summary>
    public void HandleForMoveTarget(AIGolemEntity aiGolemEntity)
    {
        bool isMove = aiGolemEntity.aiNavigation.IsMove();
        //如果已经停止移动 检测目标物体是否还存在
        if (!isMove)
        {
            //傀儡实物
            CreatureCptBaseGolem creatureGolem = aiGolemEntity.creatureCpt as CreatureCptBaseGolem;
            ItemMetaGolem itemMetaGolem = creatureGolem.golemMetaData;
            //如果背包空了
            if (itemMetaGolem.bagData.CheckIsEmpty())
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //如果没有这个核心
            itemMetaGolem.GetGolemCore(4400003, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
            if (!itemMetaGolem.CheckGolemCore(aiGolemEntity, itemGolemCore, itemMetaGolemCore))
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            float dis = Vector3.Distance(aiGolemEntity.transform.position, itemMetaGolemCore.bindBlockWorldPosition);
            //如果距离绑定位置还很远
            if (dis > 1.5f)
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }            
            //播放闲置动画
            aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
            putStatus = 2;
        }
    }

    /// <summary>
    /// 处理 放置道具
    /// </summary>
    public void HandleForPutItems(AIGolemEntity aiGolemEntity)
    {
        timeUpdateForPutItems += Time.deltaTime;
        if (timeUpdateForPutItems >= timeForPutItems)
        {
            timeUpdateForPutItems = 0;
            //傀儡实物
            CreatureCptBaseGolem creatureGolem = aiGolemEntity.creatureCpt as CreatureCptBaseGolem;
            ItemMetaGolem itemMetaGolem = creatureGolem.golemMetaData;
            //如果背包空了
            if (itemMetaGolem.bagData.CheckIsEmpty())
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //如果没有这个核心
            itemMetaGolem.GetGolemCore(4400003, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
            if (!itemMetaGolem.CheckGolemCore(aiGolemEntity, itemGolemCore, itemMetaGolemCore))
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //获取目标位置的方块
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(itemMetaGolemCore.bindBlockWorldPosition, out Block targetBlock, out Chunk targetChunk);
            if (targetChunk == null || targetBlock == null)
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //是否有放入接口
            if (targetBlock is IBlockForItemsPutOut blockPutOut)
            {

                Vector3Int localBlockPosition = itemMetaGolemCore.bindBlockWorldPosition - targetChunk.chunkData.positionForWorld;
                ItemsBean itemPut = itemMetaGolem.bagData.GetItem();
                //检测是否能放下
                bool canPut = blockPutOut.ItemsPutCheck(targetChunk, localBlockPosition, itemPut);
                if (canPut)
                {
                    blockPutOut.ItemsPut(targetChunk, localBlockPosition, itemPut);
                    itemMetaGolem.bagData.RemoveItem(itemPut);

                    //播放动画
                    aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Take);
                }
                //如果箱子满了 不能放下
                else
                {
                    aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                    return;
                }
            }
            else
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
        }
    }
}