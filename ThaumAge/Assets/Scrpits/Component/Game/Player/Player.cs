﻿using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Player : BaseMonoBehaviour
{
    public PlayerPickUp playerPickUp;
    public PlayerRay playerRay;

    protected Character character;

    public GameObject objFirstLook;
    public GameObject objThirdLook;
    public GameObject objThirdFollow;

    public void Awake()
    {
        character = GetComponentInChildren<Character>();

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
        transform.position = new Vector3(transform.position.x, maxHeight + 10, transform.position.z);
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
    public Character GetCharacter()
    {
        return character;
    }
}