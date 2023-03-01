using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AICreateEntity : AIBaseEntity
{
    public AINavigation aiNavigation;
    public CreatureCptBase creatureCpt;

    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {

    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="creatureCpt"></param>
    public virtual void SetData(CreatureCptBase creatureCpt)
    {
        this.creatureCpt = creatureCpt;
        //初始化意图
        InitIntentEntity();

        //初始化寻路
        aiNavigation = new AINavigation(this);
        //下一帧 初始化寻一次路，到当前点，防止模型下陷
        this.WaitExecuteEndOfFrame(1, () =>
        {
            aiNavigation.SetMovePosition(transform.position, true, 5);
        });
    }
}