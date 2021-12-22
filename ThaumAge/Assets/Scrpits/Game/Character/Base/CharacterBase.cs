using UnityEditor;
using UnityEngine;

public abstract class CharacterBase
{
    //角色
    public CreatureCptCharacter character;
    //角色数据
    public CharacterBean characterData;

    public CharacterBase(CreatureCptCharacter character)
    {
        this.character = character;
        if (character != null)
        {
            characterData = character.GetCharacterData();
        }
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterData"></param>
    public virtual void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }
}