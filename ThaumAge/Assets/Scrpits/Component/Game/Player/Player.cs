using UnityEditor;
using UnityEngine;

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

    [HideInInspector]
    public bool isShowHead = false;

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
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userExitPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
        if (worldPosition.y <= 0 && worldPosition.x == 0 && worldPosition.z == 0)
        {
            int maxHeight = WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(0, 0);
            worldPosition.y = maxHeight;
            worldPosition.x = 0.5f;
            worldPosition.z = 0.5f;
        }
        SetPosition(new Vector3(worldPosition.x, worldPosition.y + 0.75f, worldPosition.z));
    }

    /// <summary>
    /// 超出边界位置处理
    /// </summary>
    public void BeyondBorderPosition()
    {
        int maxHeight = WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        SetPosition(new Vector3(transform.position.x, maxHeight + 1, transform.position.z));
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
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
    /// 处理-用户数据（每一秒刷新一次）
    /// </summary>
    public void HandleForUserData()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        CharacterBean characterData = userData.characterData;
        CreatureStatusBean creatureStatus = characterData.GetCreatureStatus();
        //增加耐力
        creatureStatus.StaminaChange(0.5f);
        //减少饥饿度
        float saturation = creatureStatus.SaturationChange(-0.001f);
        //如果没有饥饿度了则减少生命值
        if (saturation <= 0)
        {
            //按比例减少
            int maxHealth = characterData.GetAttributeValue(AttributeTypeEnum.Health);
            creatureStatus.HealthChange(Mathf.RoundToInt(-maxHealth * 0.1f));
        }
        //处理一些debuff 和 buff
        creatureStatus.HanleForStatusChange(timeUpdateMaxForPlayerData);

        //刷新研究进度
        bool hasResearchProgressChange = userData.userAchievement.ResearchProgressChange(1);
        if (hasResearchProgressChange)
        {
            EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_ResearchChange);
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

    /// <summary>
    /// 刷新手上的道具
    /// </summary>
    public void RefreshHandItem()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        character.CharacterItems.ChangeRightHandItem(itemsData);
    }

    /// <summary>
    /// 设置头部显示隐藏
    /// </summary>
    /// <param name="isShow"></param>
    public void SetHeadShow(bool isShow)
    {
        this.isShowHead = isShow;
        MeshRenderer[] listMeshRenderer = character.characterHead.GetComponentsInChildren<MeshRenderer>();
        UnityEngine.Rendering.ShadowCastingMode shadowCastingMode;
        if (isShow)
        {
            shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
        {
            shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        for (int i = 0; i < listMeshRenderer.Length; i++)
        {
            MeshRenderer itemMeshRender = listMeshRenderer[i];
            itemMeshRender.shadowCastingMode = shadowCastingMode;
        }
    }
    public void SetHeadShow()
    {
        SetHeadShow(isShowHead);
    }
}