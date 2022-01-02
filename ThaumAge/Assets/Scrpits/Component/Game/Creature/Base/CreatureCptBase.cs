using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureCptBase : BaseMonoBehaviour
{
    //生物基础动画
    public CreatureAnim creatureAnim;
    //生物基础战斗
    public CreatureBattle creatureBattle;

    //生物信息
    public CreatureInfoBean creatureInfo;
    //生物数据
    public CreatureBean creatureData;

    //刚体-生物
    protected Rigidbody rbCreature;
    //动画-生物
    protected Animator animCreature;

    public virtual void Awake()
    {
        animCreature = GetComponentInChildren<Animator>();
        rbCreature = GetComponentInChildren<Rigidbody>();

        creatureAnim = new CreatureAnim(this, animCreature);
        creatureBattle = new CreatureBattle(this, rbCreature);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(CreatureInfoBean creatureInfo)
    {
        this.creatureInfo = creatureInfo;
        creatureData = new CreatureBean();
        creatureData.maxLife = creatureInfo.life;
        creatureData.currentLife = creatureInfo.life;
    }



    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        //获取所有的骨骼节点
        //List<Transform> listObjBone = CptUtil.GetAllCptInChildrenByContainName<Transform>(gameObject, "Bone");
        //for (int i = 0; i < listObjBone.Count; i++)
        //{
        //    Transform itemBone = listObjBone[i];
        //    Rigidbody boneRB = itemBone.AddComponentEX<Rigidbody>();
        //}

        //消失烟雾
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Dead_1;
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.timeForShow = 5;
        effectData.effectPosition = transform.position;
        EffectHandler.Instance.ShowEffect(effectData,(effect)=> { effect.PlayEffect(); });
    }
}