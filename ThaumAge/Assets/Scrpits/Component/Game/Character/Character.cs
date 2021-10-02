using UnityEditor;
using UnityEngine;

public class Character : BaseMonoBehaviour
{
    [Header("角色头部（需要指定）")]
    public GameObject characterHead;
    [Header("角色发型（需要指定）")]
    public GameObject characterHair;
    [Header("角色身体（需要指定）")]
    public GameObject characterBody;
    [Header("角色身体（需要指定）")]
    public GameObject characterClothes;

    [HideInInspector]
    public CharacterSkin characterSkin;
    [HideInInspector]
    public CharacterEquip characterEquip;
    [HideInInspector]
    public CharacterAnim characterAnim;

    //角色数据
    protected CharacterBean characterData;

    private void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        characterSkin = new CharacterSkin(this);
        characterEquip = new CharacterEquip(this);
        characterAnim = new CharacterAnim(this);
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
        characterSkin.SetCharacterData(characterData);
        characterEquip.SetCharacterData(characterData);
    }

    /// <summary>
    /// 获取角色数据
    /// </summary>
    /// <returns></returns>
    public CharacterBean GetCharacterData()
    {
        if (characterData == null)
        {
            characterData = new CharacterBean();
        }
        return characterData;
    }
}