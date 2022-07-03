﻿using UnityEditor;
using UnityEngine;

public class CreatureCptSkeleton : CreatureCptBaseMonster
{
    public override void AttackRemote()
    {
        base.AttackRemote();
        GameObject objTarget = aiEntity.GetChaseTarget();
        Vector3 targetShotPosition = objTarget.transform.position + Vector3.up * 1.5f;
        //发射物体
        ItemLaunchBean itemLaunchData = new ItemLaunchBean();
        itemLaunchData.itemId = 900001;
        itemLaunchData.launchStartPosition = transform.position +  Vector3.up * 1.5f;
        itemLaunchData.launchDirection = targetShotPosition - itemLaunchData.launchStartPosition;
        itemLaunchData.grity = new Vector3(0,-1f,0);
        itemLaunchData.launchPower = 20;
        ItemsHandler.Instance.CreateItemLaunch(itemLaunchData, (itemCptLaunch) =>
        {
            itemCptLaunch.Launch();
        });
    }

    public override void PlayAnimForAttackMelee()
    {
    }

    public override void PlayAnimForAttackRemote()
    {
    }
}