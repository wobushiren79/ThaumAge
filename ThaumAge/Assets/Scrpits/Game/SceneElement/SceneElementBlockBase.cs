using UnityEditor;
using UnityEngine;

public class SceneElementBlockBase
{
    public SceneElementBlockBean sceneElementBlockData;

    /// <summary>
    /// 设置数据
    /// </summary>
    public virtual void SetData(SceneElementBlockBean sceneElementBlockData)
    {
        this.sceneElementBlockData = sceneElementBlockData;
        SceneElementHandler.Instance.AddSceneElementBlock(this);
    }

    /// <summary>
    /// 更新数据（每秒更新一次）
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// 删除
    /// </summary>
    public virtual void Destory()
    {
        SceneElementHandler.Instance.RemoveSceneElementBlock(this);
    }
}