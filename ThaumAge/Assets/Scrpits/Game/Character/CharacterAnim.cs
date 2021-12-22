using UnityEditor;
using UnityEngine;

public class CharacterAnim : CharacterBase
{
    public AnimForCreature aiCreatureAnim;

    public CharacterAnim(CreatureCptCharacter character) : base(character)
    {
        aiCreatureAnim = character.animForCreature;
    }
}