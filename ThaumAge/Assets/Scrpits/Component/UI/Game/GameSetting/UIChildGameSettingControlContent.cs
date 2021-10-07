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

    private bool isInitCameraMoveSpeedX = false;
    private bool isInitCameraMoveSpeedY = false;
    public override void Open()
    {
        base.Open();
        //X轴移动速度
        isInitCameraMoveSpeedX = false;
        settingCameraMoveSpeedX = CreateItemForRange(TextHandler.Instance.GetTextById(114), HandleForCameraMoveSpeedX);
        settingCameraMoveSpeedX.SetMinMax(0.1f, 10f);
        isInitCameraMoveSpeedX = true;
        settingCameraMoveSpeedX.SetPro(gameConfig.speedForPlayerCameraMoveX);
        //Y轴移动速度
        isInitCameraMoveSpeedY = false;
        settingCameraMoveSpeedY = CreateItemForRange(TextHandler.Instance.GetTextById(115), HandleForCameraMoveSpeedY);
        settingCameraMoveSpeedY.SetMinMax(0.001f, 0.1f);
        isInitCameraMoveSpeedY = true;
        settingCameraMoveSpeedY.SetPro(gameConfig.speedForPlayerCameraMoveY);
    }

    /// <summary>
    /// 处理-X轴移动速度
    /// </summary>
    /// <param name="value"></param>
    public void HandleForCameraMoveSpeedX(float value)
    {
        if (!isInitCameraMoveSpeedX)
            return;
        settingCameraMoveSpeedX.SetContent($"{Mathf.RoundToInt((value / 2f) * 100)}%");
        gameConfig.speedForPlayerCameraMoveX = value;
        //CameraHandler.Instance.ChangeCameraSpeed(gameConfig.speedForPlayerCameraMoveX, gameConfig.speedForPlayerCameraMoveY);
    }

    /// <summary>
    /// 处理-Y轴移动速度
    /// </summary>
    /// <param name="value"></param>
    public void HandleForCameraMoveSpeedY(float value)
    {
        if (!isInitCameraMoveSpeedY)
            return;
        settingCameraMoveSpeedY.SetContent($"{Mathf.RoundToInt((value / 0.02f) * 100)}%");
        gameConfig.speedForPlayerCameraMoveY = value;
        //CameraHandler.Instance.ChangeCameraSpeed(gameConfig.speedForPlayerCameraMoveX, gameConfig.speedForPlayerCameraMoveY);
    }

}