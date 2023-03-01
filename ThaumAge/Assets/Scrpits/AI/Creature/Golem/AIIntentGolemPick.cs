using UnityEditor;
using UnityEngine;

public class AIIntentGolemPick : AIBaseIntent
{
    //拾取间隔
    protected float timeUpdateForSearchItems = 0;
    protected float timeForSearchItems = 1;

    //状态 0搜索附近物品 1走向该物品
    public int pickStatus = 0;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        pickStatus = 0;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        switch (pickStatus)
        {
            case 0:
                HandleForSearchItems();
                break;
            case 1:
                HandleForMoveTarget();
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
    public void HandleForSearchItems()
    {
        timeUpdateForSearchItems += Time.deltaTime;
        if (timeUpdateForSearchItems >= timeForSearchItems)
        {
            timeUpdateForSearchItems = 0;
            Debug.Log("HandleForSearchItems");
        }
    }
    
    /// <summary>
    /// 处理 移动向物体
    /// </summary>
    public void HandleForMoveTarget()
    {

    }
}