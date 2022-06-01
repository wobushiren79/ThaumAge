using UnityEditor;
using UnityEngine;

public class ControlForBuildingEditor : ControlForBase
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraHandler.Instance.EnabledCameraMove(true,1);
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraHandler.Instance.EnabledCameraMove(false,1);
        }
    }
}