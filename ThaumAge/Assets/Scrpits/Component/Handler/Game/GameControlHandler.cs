using UnityEditor;
using UnityEngine;

public class GameControlHandler : BaseHandler<GameControlHandler, GameControlManager>
{
    /// <summary>
    /// 设置角色控制开关
    /// </summary>
    /// <param name="enabled"></param>
    public void SetPlayerControlEnabled(bool enabled)
    {
        manager.controlForPlayer?.EnabledControl(enabled);
        manager.controlForCamera?.EnabledControl(enabled);
    }
}
