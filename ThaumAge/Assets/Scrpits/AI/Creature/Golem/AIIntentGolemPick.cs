using UnityEditor;
using UnityEngine;

public class AIIntentGolemPick : AIBaseIntent
{
    //拾取间隔
    protected float timeUpdateForSearchItems = 0;
    protected float timeForSearchItems = 1;

    //状态 0搜索附近物品 1走向该物品
    protected int pickStatus = 0;
    //目标拾取物
    protected ItemCptDrop targetItemDrop;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        pickStatus = 0;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        AIGolemEntity aiGolemEntity = aiEntity as AIGolemEntity;
        switch (pickStatus)
        {
            case 0:
                HandleForSearchItems(aiGolemEntity);
                break;
            case 1:
                HandleForMoveTarget(aiGolemEntity);
                break;
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForSearchItems = 0;
        pickStatus = 0;
    }

    /// <summary>
    /// 处理 搜索物品
    /// </summary>
    public void HandleForSearchItems(AIGolemEntity aiGolemEntity)
    {
        timeUpdateForSearchItems += Time.deltaTime;
        if (timeUpdateForSearchItems >= timeForSearchItems)
        {
            timeUpdateForSearchItems = 0;
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
            itemMetaGolem.GetGolemCore(4400002, out ItemsBean itemGolemCore, out ItemMetaGolemCore itemMetaGolemCore);
            if (!itemMetaGolem.CheckGolemCore(aiGolemEntity, itemGolemCore, itemMetaGolemCore))
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }

            float workRange = itemMetaGolem.GetWorkRange();
            //如果傀儡距离核心标记位置太原
            if (Vector3.Distance(itemMetaGolemCore.bindBlockWorldPosition, aiGolemEntity.transform.position) >= workRange * 1.5f)
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //检测核心周围的凋落物
            Collider[] itemsCollider = RayUtil.OverlapToSphere(itemMetaGolemCore.bindBlockWorldPosition, workRange, 1 << LayerInfo.Items);
            if (!itemsCollider.IsNull())
            {
                //前往第一个道具
                Collider targetCollider = itemsCollider[0];
                ItemCptDrop itemDrop = targetCollider.GetComponent<ItemCptDrop>();
                if (itemDrop != null && itemDrop.GetItemCptDropState() != ItemDropStateEnum.Picking)
                {
                    targetItemDrop = itemDrop;
                    aiGolemEntity.aiNavigation.SetMovePosition(targetCollider.gameObject.transform.position);
                    pickStatus = 1;
                    return;
                }
            }
            //如果周围没有道具 进入闲置状态
            aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
        }
    }

    /// <summary>
    /// 处理 移动向物体
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
            //如果背包满了
            if (itemMetaGolem.bagData.CheckIsFull())
            {
                aiGolemEntity.ChangeIntent(AIIntentEnum.GolemIdle);
                return;
            }
            //物体还存在 捡起物体
            if (targetItemDrop != null && targetItemDrop.GetItemCptDropState() != ItemDropStateEnum.Picking)
            {
                float dis = Vector3.Distance(aiGolemEntity.transform.position, targetItemDrop.transform.position);
                //如果物体在傀儡身边 则捡起
                if (dis < 2)
                {
                    //拾取物体
                    itemMetaGolem.bagData.AddItemForBag(targetItemDrop.itemDropData.itemData);
                    //删除物体
                    targetItemDrop.DestroySelf();
                }
            }
            pickStatus = 0;
        }
    }
}