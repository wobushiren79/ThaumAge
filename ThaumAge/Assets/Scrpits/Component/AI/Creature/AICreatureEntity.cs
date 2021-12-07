using UnityEditor;
using UnityEngine;

public class AICreatureEntity : AIBaseEntity
{
    public AINavigation aiNavigation;

    public bool isInit=false;
    public override void Awake()
    {
        base.Awake();
        InitIntent<AICreatureIntentEnum>();
        aiNavigation = new AINavigation(this);
    }

    public override void Start()
    {
        base.Start();
        ChangeIntent(AICreatureIntentEnum.Idle);
    }
}