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

    [Header("角色裤子左（需要指定）")]
    public GameObject characterTrousersL;
    [Header("角色裤子右（需要指定）")]
    public GameObject characterTrousersR;

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
        this.creatureData = characterData;
        characterSkin.SetCharacterData(characterData);
        characterEquip.SetCharacterData(characterData);
    }

    /// <summary>
    /// 获取角色数据
    /// </summary>
    /// <returns></returns>
    public CharacterBean GetCharacterData(string defCreatureId = "")
    {
        if (creatureData == null)
        {
            creatureData = new CharacterBean(defCreatureId);
        }
        return (CharacterBean)creatureData;
    }

    public override void Dead()
    {
        if (creatureData.GetCreatureType() == CreatureTypeEnum.Player)
        {
            //展示死亡特效
            Player player = GameHandler.Instance.manager.player;
            player.gameObject.ShowObj(false);

            //如果是玩家控制的角色
            Vector3Int worldPos = Vector3Int.FloorToInt(transform.position + Vector3.up * 0.5f);
            //创建一个坟墓
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPos, out Block block, out Chunk chunk);
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            if (chunk != null)
            {
                float randmType = Random.Range(0f, 1f);
                //一定概率生成稀有墓碑
                BlockTypeEnum tombType = BlockTypeEnum.Tombstone;
                if (randmType <= 0.01f)
                {
                    tombType = BlockTypeEnum.TombstoneRare;
                }
                //随机墓碑的方向
                int direction = Random.Range(11, 15);
                ItemsBean[] allShortcutItems = userData.GetAllItemsFromShortcut();
                BlockMetaChest blockMetaData = new BlockMetaChest(2 * 7, allShortcutItems);
                chunk.SetBlockForLocal(worldPos - chunk.chunkData.positionForWorld, tombType, (BlockDirectionEnum)direction, blockMetaData.ToJson());
                //清除所有的道具
                userData.ClearAllItemsFromShortcut();
                //刷新UI
                UIHandler.Instance.RefreshUI();
            }
            //弹出死亡UI
            UIHandler.Instance.OpenUI<UIGameDead>();
            //关闭控制
            GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
            GameDataHandler.Instance.WaitExecuteEndOfFrame(30, () =>
            {
                //回复所有状态
                userData.characterData.GetCreatureStatus().ReplyAllStatus();
                //获取世界位置
                userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
                //保存数据
                GameDataHandler.Instance.manager.SaveUserData(worldType, worldPosition);
            });
        }
    }
}