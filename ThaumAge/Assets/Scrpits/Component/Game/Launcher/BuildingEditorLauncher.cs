using UnityEditor;
using UnityEngine;

public class BuildingEditorLauncher :BaseLauncher
{
    public override void Launch()
    {
        Debug.Log("1");
        //关闭控制
        CameraHandler.Instance.EnabledCameraMove(false,1);
        //打开UI
        UIHandler.Instance.OpenUIAndCloseOther<UIBuildingEditorMain>(UIEnum.GameBook);
    }
}