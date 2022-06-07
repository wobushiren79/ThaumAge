using UnityEditor;
using UnityEngine;

public class CreatureCptCow : CreatureCptBase
{
    protected AIAnimalEntity aiCow;

    public override void Awake()
    {
        base.Awake();
        aiCow = gameObject.AddComponentEX<AIAnimalEntity>();
        aiCow.SetData(this);
    }
}