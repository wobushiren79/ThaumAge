using UnityEditor;
using UnityEngine;

public class CreatureCptBaseGolem : CreatureCptBase
{
    protected AIGolemEntity aiEntity;


    public override void Awake()
    {
        base.Awake();
        aiEntity = gameObject.AddComponentEX<AIGolemEntity>();
        aiEntity.SetData(this);
    }
}