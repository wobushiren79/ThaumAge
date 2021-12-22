using UnityEditor;
using UnityEngine;

public class CreatureCptBase : BaseMonoBehaviour
{
    //生物基础动画
    public AnimForCreature animForCreature;

    public virtual void Awake()
    {
        Animator aiAnimator = GetComponentInChildren<Animator>();
        animForCreature = new AnimForCreature(aiAnimator);
    }

    /// <summary>
    /// 遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(int damage)
    {

    }
}