using UnityEditor;
using UnityEngine;

public class BaseLauncher : BaseMonoBehaviour
{

    private void Start()
    {
        Launch();
    }

    public virtual void Launch()
    {
        //先清理一下内存
        SystemUtil.GCCollect();        
        //设置FPS
        FPSHandler.Instance.SetData(true, 120);
    }
}