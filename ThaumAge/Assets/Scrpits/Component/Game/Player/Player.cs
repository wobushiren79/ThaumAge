using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Player : BaseMonoBehaviour
{
    public PlayerPickUp playerPickUp;
    public PlayerRay playerRay;

    [HideInInspector]
    public CreatureCptCharacter character;

    public GameObject objFirstLook;
    public GameObject objThirdLook;
    public GameObject objThirdFollow;

    public void Awake()
    {
        character = GetComponentInChildren<CreatureCptCharacter>();

        playerPickUp = new PlayerPickUp(this);
        playerRay = new PlayerRay(this);

        InvokeRepeating("UpdatePlayerData", 0.2f, 0.2f);
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
        playerPickUp.UpdatePick();

        HandleForBeyondBorder();
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