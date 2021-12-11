using UnityEditor;
using UnityEngine;

public class CharacterAnim : CharacterBase
{

    public AICreatureAnim aiCreatureAnim;

    public CharacterAnim(Character character) : base(character)
    {
        Animator  animatorCharacter = character.GetComponentInChildren<Animator>();
        aiCreatureAnim =new AICreatureAnim(animatorCharacter);
    }
}