using UnityEditor;
using UnityEngine;

public class CreatureCptSkeleton : CreatureCptBase
{
    protected AICreatureEntity aiSkeleton;

    public override void Awake()
    {
        base.Awake();
        aiSkeleton = gameObject.AddComponentEX<AICreatureEntity>();
    }
}