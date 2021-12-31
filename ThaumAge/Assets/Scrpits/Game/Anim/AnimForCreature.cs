﻿using UnityEditor;
using UnityEngine;

public class AnimForCreature
{
    //角色动画控制器
    public Animator animator;

    public AnimForCreature(Animator animator)
    {
        this.animator = animator;
    }

    /// <summary>
    /// 修改角色的基础动画
    /// </summary>
    /// <param name="animType"></param>
    public void PlayBaseAnim(CharacterAnimBaseState animType)
    {
        animator.SetInteger("state", (int)animType);
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
    public void PlayUse(bool isUse)
    {
        animator.SetBool("use", isUse);
    }

    /// <summary>
    /// 播放指定动画
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnim(string animName) 
    {
        animator.CrossFade(animName,0.1f);
    }

}