using UnityEditor;
using UnityEngine;

public class CreatureCptCharacter : CreatureCptBase
{
    [Header("角色头部（需要指定）")]
    public GameObject characterHead;
    [Header("角色帽子（需要指定）")]
    public GameObject characterHat;
    [Header("角色发型（需要指定）")]
    public GameObject characterHair;
    [Header("角色身体（需要指定）")]
    public GameObject characterBody;

    [Header("角色身体衣服（需要指定）")]
    public GameObject characterClothes;
    [Header("角色右手（需要指定）")]
    public GameObject characterRightHand;

    [Header("角色右手衣服（需要指定）")]
    public GameObject characterClothesRight;
    [Header("角色左手衣服（需要指定）")]
    public GameObject characterClothesLeft;

    [Header("角色鞋子左（需要指定）")]
    public GameObject characterShoesL;
    [Header("角色鞋子右（需要指定）")]
    public GameObject characterShoesR;

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

    public override void Dead()
    {
        if (creatureData.GetCreatureType() == CreatureTypeEnum.Player)
        {
            //如果是玩家控制的角色
            Vector3Int worldPos = Vector3Int.RoundToInt(transform.position);
            //创建一个坟墓
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPos, out Block block, out Chunk chunk);
            if (chunk != null)
            {
                float randmType = Random.Range(0f, 1f);
                //一定概率生成稀有墓碑
                BlockTypeEnum tombType = BlockTypeEnum.Tombstone;
                if (randmType <= 0.01f)
                {
                    tombType = BlockTypeEnum.TombstoneRare;
                }
                int direction = Random.Range(11, 15);
                chunk.SetBlockForLocal(worldPos - chunk.chunkData.positionForWorld, tombType, (BlockDirectionEnum)direction);
            }
        }
    }
}