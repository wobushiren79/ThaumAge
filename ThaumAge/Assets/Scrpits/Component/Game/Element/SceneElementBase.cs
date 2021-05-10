using UnityEditor;
using UnityEngine;

public class SceneElementBase : BaseMonoBehaviour
{

    /// <summary>
    /// 位置处理
    /// </summary>
    public virtual void HandleForPosition()
    {
        Transform tfPlayer = GameHandler.Instance.manager.player.transform;
        transform.position = tfPlayer.position;
    }

}