using System.Collections;
using UnityEngine;

public class VolumeHandler : BaseHandler<VolumeHandler, VolumeManager>
{

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        manager.SetShadowsDistance(gameConfig.shadowDis);
    }

    /// <summary>
    /// 设置远景模糊
    /// </summary>
    /// <param name="worldType"></param>
    public void SetDepthOfField(WorldTypeEnum worldType)
    {
        if (worldType == WorldTypeEnum.Launch)
        {
            manager.SetDepthOfField(2, 3, 10, 20);
        }
        else
        {
            manager.SetDepthOfField(1f, 4f, 60, 80);
        }
    }
}