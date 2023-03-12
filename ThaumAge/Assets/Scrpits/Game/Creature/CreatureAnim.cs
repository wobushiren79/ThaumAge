using UnityEditor;
using UnityEngine;

public class CreatureAnim : CreatureBase
{
    //角色动画控制器
    public Animator animator;

    public CreatureAnim(CreatureCptBase creature, Animator animator) : base(creature)
    {
        this.animator = animator;
    }

    /// <summary>
    /// 开关动画
    /// </summary>
    /// <param name="enabled"></param>
    public void EnabledAnim(bool enabled)
    {
        animator.enabled = enabled;
    }

    /// <summary>
    /// 修改角色的基础动画
    /// </summary>
    /// <param name="animType"></param>
    public void PlayBaseAnim(CreatureAnimBaseState animType)
    {
        animator.SetInteger("state", (int)animType);
        if(animType== CreatureAnimBaseState.Take)
        {
            PlayAnim("take");
        }
    }

    /// <summary>
    /// 设置动画速度
    /// </summary>
    /// <param name="animSpeed"></param>
    public void SetAnimSpeed(float animSpeed = 1)
    {
        animator.speed = animSpeed;
    }

    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetClimbSpeed(float speed)
    {
        animator.SetFloat("speed_climb", speed);
    }

    /// <summary>
    /// 播放跳跃动画
    /// </summary>
    /// <param name="isJump"></param>
    public void PlayJump(bool isJump)
    {
        animator.SetBool("jump", isJump);
    }

    /// <summary>
    /// 播放使用动画
    /// </summary>
    /// <param name="isUse"></param>
    public void PlayUse(bool isUse, int useType = 0)
    {
        animator.SetInteger("use_type", useType);
        animator.SetBool("use", isUse);
    }

    /// <summary>
    /// 播放指定动画
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnim(string animName)
    {
        animator.CrossFade(animName, 0.05f);
    }

}