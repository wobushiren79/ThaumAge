using UnityEditor;
using UnityEngine;

public class BuildingEditorLauncher : BaseLauncher
{
    public override void Launch()
    {
        IconHandler.Instance.InitData(null);
        //关闭控制
        CameraHandler.Instance.EnabledCameraMove(false, 1);
        //打开UI
        UIHandler.Instance.OpenUIAndCloseOther<UIBuildingEditorMain>(UIEnum.BuildingEditorMain);
    }
}