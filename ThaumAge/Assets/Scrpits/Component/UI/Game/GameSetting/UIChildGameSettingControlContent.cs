using UnityEditor;
using UnityEngine;

public class UIChildGameSettingControlContent : UIChildGameSettingBaseContent
{
    //X轴移动速度
    protected UIListItemGameSettingRange settingCameraMoveSpeedX;
    //Y轴移动速度
    protected UIListItemGameSettingRange settingCameraMoveSpeedY;

    public UIChildGameSettingControlContent(GameObject objListContainer) : base(objListContainer)
    {

    }

    public override void Open()
    {
        base.Open();
        //X轴移动速度
        settingCameraMoveSpeedX = CreateItemForRange(TextHandler.Instance.GetTextById(114), HandleForCameraMoveSpeedX);
        settingCameraMoveSpeedX.SetMinMax(0.1f, 10f);
        settingCameraMoveSpeedX.SetPro(gameConfig.speedForPlayerCameraMoveX);
        //Y轴移动速度
        settingCameraMoveSpeedY = CreateItemForRange(TextHandler.Instance.GetTextById(115), HandleForCameraMoveSpeedY);
        settingCameraMoveSpeedY.SetMinMax(0.1f, 10f);
        settingCameraMoveSpeedY.SetPro(gameConfig.speedForPlayerCameraMoveY);
    }

    /// <summary>
    /// 处理-X轴移动速度
    /// </summary>
    /// <param name="value"></param>
    public void HandleForCameraMoveSpeedX(float value)
    {
        settingCameraMoveSpeedX.SetContent($"{Mathf.RoundToInt(value * 100)}%");
        gameConfig.speedForPlayerCameraMoveX = value;
        //CameraHandler.Instance.ChangeCameraSpeed(gameConfig.speedForPlayerCameraMoveX, gameConfig.speedForPlayerCameraMoveY);
    }

    /// <summary>
    /// 处理-Y轴移动速度
    /// </summary>
    /// <param name="value"></param>
    public void HandleForCameraMoveSpeedY(float value)
    {
        settingCameraMoveSpeedY.SetContent($"{Mathf.RoundToInt(value * 100)}%");
        gameConfig.speedForPlayerCameraMoveY = value;
        //CameraHandler.Instance.ChangeCameraSpeed(gameConfig.speedForPlayerCameraMoveX, gameConfig.speedForPlayerCameraMoveY);
    }

}