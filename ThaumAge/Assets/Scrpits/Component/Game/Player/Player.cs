using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Player : BaseMonoBehaviour
{
    protected PlayerPickUp playerPickUp;

    protected Character character;

    public void Awake()
    {
        playerPickUp = new PlayerPickUp(this);
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
        transform.position = new Vector3(transform.position.x, maxHeight + 5, transform.position.z);
    }

    /// <summary>
    /// 处理超出边界
    /// </summary>
    public void HandleForBeyondBorder()
    {
        if (transform.position.y <= -10)
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

}