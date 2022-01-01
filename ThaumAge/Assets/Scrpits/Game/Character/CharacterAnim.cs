using UnityEditor;
using UnityEngine;

public class CharacterAnim : CharacterBase
{
    public CreatureAnim creatureAnim;

    public CharacterAnim(CreatureCptCharacter character) : base(character)
    {
        creatureAnim = character.creatureAnim;
    }
}