using UnityEditor;
using UnityEngine;

public class AICreatureEntity : AIBaseEntity
{
    public AINavigation aiNavigation;

    public override void Awake()
    {
        base.Awake();
        InitIntent<AICreatureIntentEnum>();
        aiNavigation = new AINavigation(this);
    }

}