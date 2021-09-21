using System.Collections;
using UnityEngine;

public class VolumeHandler : BaseHandler<VolumeHandler, VolumeManager>
{

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameConfigBean gameConfig =  GameDataHandler.Instance.manager.GetGameConfig();
        manager.SetShadowsDistance(gameConfig.shadowDis);
    }

}