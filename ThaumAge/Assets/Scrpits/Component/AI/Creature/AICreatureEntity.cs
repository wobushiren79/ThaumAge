using UnityEditor;
using UnityEngine;

public class AICreatureEntity : AIBaseEntity
{
    public override void Awake()
    {
        base.Awake();
        InitIntent<AICreatureIntentEnum>();
    }

    public override void Start()
    {
        base.Start();
        ChangeIntent(AICreatureIntentEnum.Idle);
    }
}