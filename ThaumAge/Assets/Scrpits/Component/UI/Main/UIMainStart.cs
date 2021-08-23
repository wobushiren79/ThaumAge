using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIMainStart : BaseUIComponent
{

    public void Start()
    {
        
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Create) HandleForCreate();
        else if (viewButton == ui_Continue) HandleForContinue();
        else if (viewButton == ui_Setting) HandleForSetting();
        else if (viewButton == ui_Exit) HandleForExit();
    }

    /// <summary>
    /// 设置UI文本
    /// </summary>
    public void SetUIText()
    {

    }

    /// <summary>
    /// 处理-创建游戏
    /// </summary>
    public void HandleForCreate()
    {

    }

    /// <summary>
    /// 处理-继续游戏
    /// </summary>
    public void HandleForContinue()
    {

    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    public void HandleForSetting()
    {

    }

    /// <summary>
    /// 处理-离开游戏
    /// </summary>
    public void HandleForExit()
    {
        GameUtil.ExitGame();
    }
}