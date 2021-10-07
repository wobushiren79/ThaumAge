using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class UIGameMain : BaseUIComponent
{
    protected InputAction inputGodMain;

    public override void OnInputActionForStarted(InputActionUIEnum inputName)
    {
        base.OnInputActionForStarted(inputName);
        if (inputName == InputActionUIEnum.F12 && ProjectConfigInfo.BUILD_TYPE == ProjectBuildTypeEnum.Debug)
        {
            //打开GM菜单
            UIHandler.Instance.OpenUIAndCloseOther<UIGodMain>(UIEnum.GodMain);
        }
        else if (inputName == InputActionUIEnum.ESC)
        {
            //打开设置
            UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
        }
    }
}
