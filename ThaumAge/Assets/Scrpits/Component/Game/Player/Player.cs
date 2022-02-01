using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Player : BaseMonoBehaviour
{
    public PlayerPickUp playerPickUp;
    public PlayerRay playerRay;

    protected CreatureCptCharacter character;

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
        HandleForColliderTrigger();
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    public void InitPosition()
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
            InitPosition();
        }
    }

    /// <summary>
    /// 处理碰撞触发
    /// </summary>
    public void HandleForColliderTrigger()
    {
        Collider[] colliderArray = RayUtil.RayToSphere(transform.position, 1, 1 << LayerInfo.Chunk | 1 << LayerInfo.ChunkTrigger);
        if (!colliderArray.IsNull())
        {

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