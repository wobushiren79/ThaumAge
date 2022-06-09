using UnityEditor;
using UnityEngine;

public class ControlForBuildingEditor : ControlForBase
{
    public GameObject objSelect;
    //是否正在移动镜头
    protected bool isMoveCamera = false;

    public void Update()
    {
        if (UGUIUtil.IsPointerUI())
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            CameraHandler.Instance.EnabledCameraMove(true, 1);
            isMoveCamera = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            CameraHandler.Instance.EnabledCameraMove(false, 1);
            isMoveCamera = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnClickForBuild();
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            //减少高度
            ChangeBlockBuildHigh(-1);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            //增加高度
            ChangeBlockBuildHigh(1);
        }
        if (!isMoveCamera)
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

    /// <summary>
    /// 点击建造
    /// </summary>
    public void OnClickForBuild()
    {
        BuildingEditorHandler.Instance.BuildBlock(Vector3Int.RoundToInt(objSelect.transform.position));
    }

    /// <summary>
    /// 修改方块建造高度
    /// </summary>
    public void ChangeBlockBuildHigh(int changeHigh)
    {
        BuildingEditorHandler.Instance.manager.curBuildHigh += changeHigh;
        if (BuildingEditorHandler.Instance.manager.curBuildHigh < 0)
            BuildingEditorHandler.Instance.manager.curBuildHigh = 0;
        BuildingEditorHandler.Instance.manager.objPlane.transform.position = BuildingEditorHandler.Instance.manager.objPlane.transform.position.SetY(BuildingEditorHandler.Instance.manager.curBuildHigh);
    }
}