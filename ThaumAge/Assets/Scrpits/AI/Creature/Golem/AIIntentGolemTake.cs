using UnityEditor;
using UnityEngine;

public class AIIntentGolemTake : AIBaseIntent
{
    //拾取间隔
    protected float timeUpdateForTakeItems = 0;
    protected float timeForTakeItems = 1;

    //状态 1走向标记容器 2拿取道具 
    protected int takeStatus = 0;
    //目标拾取物
    protected ItemCptDrop targetItemDrop;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIGolemEntity aiGolemEntity = aiEntity as AIGolemEntity;
        //播放闲置动画
        aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);

        takeStatus = 0;
        timeUpdateForTakeItems = 0;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        AIGolemEntity aiGolemEntity = aiEntity as AIGolemEntity;
        switch (takeStatus)
        {
            case 0:
                HandleForStart(aiGolemEntity);
                break;
            case 1:
                HandleForMoveTarget(aiGolemEntity);
                break;
            case 2:
                HandleForTakeItems(aiGolemEntity);
                break;
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForTakeItems = 0;
        takeStatus = 0;
    }

    /// <summary>
    /// 处理-开始
    /// </summary>
    public void HandleForStart(AIGolemEntity aiGolemEntity)
    {
        //傀儡实物
        CreatureCptBaseGolem creatureGolem = aiGolemEntity.creatureCpt as CreatureCptBaseGolem;
        ItemMetaGolem itemMetaGolem = creatureGolem.golemMetaData;
        //如果背包满了
        if (itemMetaGolem.bagData.CheckIsFull())
        {
            aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
            return;
        }
        //如果没有这个核心
        itemMetaGolem.GetGolemCore(4400004, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
        if (!itemMetaGolem.CheckGolemCore(aiGolemEntity, itemGolemCore, itemMetaGolemCore))
        {
            aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
            return;
        }
        //向绑定的位置移动
        aiGolemEntity.aiNavigation.SetMovePosition(itemMetaGolemCore.bindBlockWorldPosition + new Vector3(0.5f, 0.5f, 0.5f));
        //播放动画
        aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);

        takeStatus = 1;
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
            if (itemMetaGolem.bagData.CheckIsFull())
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //如果没有这个核心
            itemMetaGolem.GetGolemCore(4400004, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
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
            takeStatus = 2;
            //播放闲置动画
            aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
        }
    }

    /// <summary>
    /// 处理 放置道具
    /// </summary>
    public void HandleForTakeItems(AIGolemEntity aiGolemEntity)
    {
        timeUpdateForTakeItems += Time.deltaTime;
        if (timeUpdateForTakeItems >= timeForTakeItems)
        {
            timeUpdateForTakeItems = 0;
            //傀儡实物
            CreatureCptBaseGolem creatureGolem = aiGolemEntity.creatureCpt as CreatureCptBaseGolem;
            ItemMetaGolem itemMetaGolem = creatureGolem.golemMetaData;
            //如果背包空了
            if (itemMetaGolem.bagData.CheckIsFull())
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //如果没有这个核心
            itemMetaGolem.GetGolemCore(4400004, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
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
                //检测是否能放下
                ItemsBean outItem = blockPutOut.ItemsOut(targetChunk, localBlockPosition);
                if (outItem != null && outItem.itemId != 0)
                {
                    itemMetaGolem.bagData.AddItemForBag(outItem);
                    //播放动画
                    aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Take);
                }
                //如果箱子空了
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