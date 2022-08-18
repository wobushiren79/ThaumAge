using UnityEditor;
using UnityEngine;

public class BuildingEditorLauncher : BaseLauncher
{
    public override void Launch()
    {
        IconHandler.Instance.InitData();
        //关闭控制
        CameraHandler.Instance.EnabledCameraMove(false, 1);
        //加载资源
        GameHandler.Instance.LoadGameResources(() =>
        {
            //打开UI
            UIHandler.Instance.OpenUIAndCloseOther<UIBuildingEditorMain>(UIEnum.BuildingEditorMain);
        });
    }
}