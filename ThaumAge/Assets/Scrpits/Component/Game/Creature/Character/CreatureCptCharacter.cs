﻿using UnityEditor;
using UnityEngine;

public class CreatureCptCharacter : CreatureCptBase
{
    [Header("角色头部（需要指定）")]
    public GameObject characterHead;
    [Header("角色发型（需要指定）")]
    public GameObject characterHair;
    [Header("角色身体（需要指定）")]
    public GameObject characterBody;
    [Header("角色身体（需要指定）")]
    public GameObject characterClothes;
    [Header("角色右手（需要指定）")]
    public GameObject characterRightHand;

    [HideInInspector]
    public CharacterSkin characterSkin;
    [HideInInspector]
    public CharacterEquip characterEquip;
    [HideInInspector]
    public CharacterAnim characterAnim;
    [HideInInspector]
    public CharacterItems CharacterItems;

    //角色数据
    protected CharacterBean characterData;

    public override void Awake()
    {
        base.Awake();
        InitData();
    }

    public void InitData()
    {
        characterSkin = new CharacterSkin(this);
        characterEquip = new CharacterEquip(this);
        characterAnim = new CharacterAnim(this);
        CharacterItems = new CharacterItems(this);
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