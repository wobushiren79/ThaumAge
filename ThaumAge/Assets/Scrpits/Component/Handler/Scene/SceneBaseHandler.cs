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
        //打开加载UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);
        //删除世界数据
        WorldCreateHandler.Instance.ClearWorld();
        //因为场景处理器只在本场景使用，所以跳转之后可以删除
        Destroy(this.gameObject);
        //场景修改
        SceneUtil.SceneChange(scenes);
    }

    /// <summary>
    /// 获取当前场景
    /// </summary>
    /// <returns></returns>
    public ScenesEnum GetCurrentScene()
    {
        return SceneUtil.GetCurrentScene();
    }
}