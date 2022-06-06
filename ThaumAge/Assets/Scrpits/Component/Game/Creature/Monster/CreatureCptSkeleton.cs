using UnityEditor;
using UnityEngine;

public class CreatureCptSkeleton : CreatureCptBase
{
    protected AIAnimalEntity aiSkeleton;

    public override void Awake()
    {
        base.Awake();
        aiSkeleton = gameObject.AddComponentEX<AIAnimalEntity>();
    }
}