using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Player : BaseMonoBehaviour
{
    public PlayerPickUp playerPickUp;
    public PlayerRay playerRay;

    protected Rigidbody rbPlayer;

    [HideInInspector]
    public CreatureCptCharacter character;

    public GameObject objFirstLook;
    public GameObject objThirdLook;
    public GameObject objThirdFollow;

    //角色数据更新时间
    protected float timeUpdateForPlayerData = 0;
    protected float timeUpdateMaxForPlayerData = 1f;


    public void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
        character = GetComponentInChildren<CreatureCptCharacter>();
        character.creatureData.SetCreatureType(CreatureTypeEnum.Player);

        playerPickUp = new PlayerPickUp(this);
        playerRay = new PlayerRay(this);
    }

    public void Update()
    {
        timeUpdateForPlayerData += Time.deltaTime;
        if (timeUpdateForPlayerData > timeUpdateMaxForPlayerData)
        {
            UpdatePlayerData();
            timeUpdateForPlayerData = 0;
        }
    }

    public void OnDestroy()
    {
        CancelInvoke();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void UpdatePlayerData()
    {
        //角色拾取处理
        playerPickUp.UpdatePick();
        //角色边界处理
        HandleForBeyondBorder();
        //角色状态处理
        HandleForUserData();
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    public void InitPosition()
    {
        int maxHeight = WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 position);
        SetPosition(new Vector3(position.x, maxHeight + 2, position.z));
    }

    /// <summary>
    /// 超出边界位置处理
    /// </summary>
    public void BeyondBorderPosition()
    {
        int maxHeight = WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        SetPosition(new Vector3(transform.position.x, maxHeight + 2, transform.position.z));
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(false);
        transform.position = position;
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
    }

    /// <summary>
    /// 处理超出边界
    /// </summary>
    public void HandleForBeyondBorder()
    {
        if (transform.position.y <= -1)
        {
            BeyondBorderPosition();
        }
    }

    /// <summary>
    /// 处理-用户数据
    /// </summary>
    public void HandleForUserData()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterStatusBean characterStatus = userData.characterData.GetCharacterStatus();
        //增加耐力
        characterStatus.StaminaChange(0.5f);
        //减少饥饿度
        float saturation = characterStatus.SaturationChange(-0.001f);
        //如果没有饥饿度了则减少生命值
        if (saturation <= 0)
        {
            //按比例减少
            characterStatus.HealthChange(Mathf.RoundToInt(-characterStatus.maxHealth * 0.1f));
        }
        //刷新UI
        EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    public CreatureCptCharacter GetCharacter()
    {
        return character;
    }

    /// <summary>
    /// 刷新角色
    /// </summary>
    public void RefreshCharacter()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        character.SetCharacterData(userData.characterData);
    }
}