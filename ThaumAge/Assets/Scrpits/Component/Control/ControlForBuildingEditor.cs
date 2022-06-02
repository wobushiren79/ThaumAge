using UnityEditor;
using UnityEngine;

public class ControlForBuildingEditor : ControlForBase
{
    public GameObject objSelect;

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
        UpdateSelectBlockPosition();
    }

    /// <summary>
    /// 更新选中位置
    /// </summary>
    public void UpdateSelectBlockPosition()
    {
        int layerMask = 1 << LayerInfo.BuildingEditor;
        RayUtil.RayToScreenPointForMousePosition(float.MaxValue, layerMask, out bool isCollider, out RaycastHit hit);
        if (isCollider)
        {
            objSelect.ShowObj(true);

            objSelect.transform.position = Vector3Int.RoundToInt(hit.point);
        }
        else
        {
            objSelect.ShowObj(false);
        }
    }
}