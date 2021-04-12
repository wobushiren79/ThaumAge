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
            timeForWorldUpdate = 1;
            HandleForWorldUpdate();
            HandleForBeyondBorder();
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
        WorldCreateHandler.Instance.CreateChunkForRange(transform.position, 5);
    }

    /// <summary>
    /// 处理超出边界
    /// </summary>
    public void HandleForBeyondBorder()
    {
        if (transform.position.y <= -10)
        {
            int maxHeight =  WorldCreateHandler.Instance.manager.GetMaxHeightForWorldPosition(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            transform.position = new Vector3(transform.position.x, maxHeight + 5, transform.position.z);
        }
    }
}