using UnityEditor;
using UnityEngine;

public class SceneBaseHandler<H, M> : BaseHandler<H, M> 
    where H : BaseHandler<H, M>
    where M : BaseManager
{

    /// <summary>
    ///  改变场景
    /// </summary>
    /// <param name="scenes"></param>
    public void ChangeScene(ScenesEnum scenes)
    {
        SceneUtil.SceneChange(scenes);
        //因为场景处理器只在本场景使用，所以跳转之后可以删除
        Destroy(this.gameObject);
    }
}