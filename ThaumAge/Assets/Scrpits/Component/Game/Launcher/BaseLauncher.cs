using UnityEditor;
using UnityEngine;

public class BaseLauncher : BaseMonoBehaviour
{

    private void Start()
    {
        Launch();
    }

    /// <summary>
    /// 启动
    /// </summary>
    public virtual void Launch()
    {
        //先清理一下内存
        SystemUtil.GCCollect();        
        //游戏设置初始化
        GameDataHandler.Instance.InitData();
        //场景启动
        GameHandler.Instance.launcher = this;
        GameHandler.Instance.actionForLauncher?.Invoke();
    }

}