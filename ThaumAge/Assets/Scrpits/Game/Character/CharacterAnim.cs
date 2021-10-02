using UnityEditor;
using UnityEngine;

public class CharacterAnim : CharacterBase
{
    //角色动画控制器
    public Animator animatorCharacter;

    public CharacterAnim(Character character) : base(character)
    {
        animatorCharacter = character.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 修改角色的基础动画
    /// </summary>
    /// <param name="animType"></param>
    public void PlayBaseAnim(CharacterAnimBaseState animType)
    {
        animatorCharacter.SetInteger("state", (int)animType);
    }
}