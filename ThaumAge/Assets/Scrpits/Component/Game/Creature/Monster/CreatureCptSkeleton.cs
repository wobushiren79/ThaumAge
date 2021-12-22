using UnityEditor;
using UnityEngine;

public class CreatureCptSkeleton : CreatureCptBase
{
    public AICreatureEntity aiSkeleton;

    public override void Awake()
    {
        base.Awake();
        aiSkeleton = GetComponentInChildren<AICreatureEntity>();
    }
}