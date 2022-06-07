using UnityEditor;
using UnityEngine;

public class AIAnimalEntity : AIBaseEntity
{
    public AINavigation aiNavigation;
    public CreatureCptBase creatureCpt;

    public void SetData(CreatureCptBase creatureCpt)
    {
        this.creatureCpt = creatureCpt;
        //初始化寻路
        aiNavigation = new AINavigation(this);
        //初始化数据
        InitData<AIAnimalIntentEnum>();
        //默认闲置
        ChangeIntent(AIAnimalIntentEnum.Idle);
    }

}