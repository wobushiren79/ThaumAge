using UnityEditor;
using UnityEngine;

public class AIAnimalEntity : AIBaseEntity
{
    public AINavigation aiNavigation;

    public override void Awake()
    {
        base.Awake();
        InitIntent<AIAnimalIntentEnum>();
        aiNavigation = new AINavigation(this);
    }

}