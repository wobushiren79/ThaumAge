using UnityEditor;
using UnityEngine;

public class Player : BaseMonoBehaviour
{
    //第一人称点位
    public Transform LookForFirst;
    //第三人称点位
    public Transform LookForThird;

    protected CharacterController characterController;
    protected float timeForWorldUpdate = 0;

    private void LateUpdate()
    {
        timeForWorldUpdate -= Time.deltaTime;
        if (timeForWorldUpdate <= 0)
        {
            timeForWorldUpdate = 0.2f;
            HandleForBeyondBorder();
            HandleForColliderTrigger();
        }

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
        if (!CheckUtil.ArrayIsNull(colliderArray))
        {

        }
    }

}