using UnityEditor;
using UnityEngine;

public class CharacterAnim : CharacterBase
{

    public AnimForCreature aiCreatureAnim;

    public CharacterAnim(Character character) : base(character)
    {
        Animator  animatorCharacter = character.GetComponentInChildren<Animator>();
        aiCreatureAnim =new AnimForCreature(animatorCharacter);
    }
}