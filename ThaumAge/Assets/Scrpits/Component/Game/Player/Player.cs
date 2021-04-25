using UnityEditor;
using UnityEngine;

public class Player : BaseMonoBehaviour
{
    protected float timeForWorldUpdate = 0;

    private void LateUpdate()
    {
        timeForWorldUpdate -= Time.deltaTime;
        if (timeForWorldUpdate <= 0)
        {
            timeForWorldUpdate = 0.2f;
            HandleForWorldUpdate();
            HandleForBeyondBorder();
            HandleForColliderTrigger();
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate()
    {
        WorldCreateHandler.Instance.CreateChunkForRangeForWorldPostion(transform.position, 3,()=> 
        {

        });
    }

    /// <summary>
    /// 处理超出边界
    /// </summary>
    public void HandleForBeyondBorder()
    {
        if (transform.position.y <= -10)
        {
            int maxHeight = WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            transform.position = new Vector3(transform.position.x, maxHeight + 5, transform.position.z);
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