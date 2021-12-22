using UnityEditor;
using UnityEngine;

public class AICreatureEntity : AIBaseEntity
{
    public AINavigation aiNavigation;
    public AnimForCreature aiCreatureAnim;

    public bool isInit=false;
    public override void Awake()
    {
        base.Awake();
        InitIntent<AICreatureIntentEnum>();
        aiNavigation = new AINavigation(this);

        Animator aiAnimator = GetComponentInChildren<Animator>();
        aiCreatureAnim = new AnimForCreature(aiAnimator);
    }

    public override void Start()
    {
        base.Start();
        ChangeIntent(AICreatureIntentEnum.Idle);

        aiCreatureAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
    }
}