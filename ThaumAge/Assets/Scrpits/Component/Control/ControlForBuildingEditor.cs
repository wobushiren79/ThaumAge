using UnityEditor;
using UnityEngine;

public class ControlForBuildingEditor : ControlForBase
{
    public GameObject objBlockContainer;
    public GameObject objPlane;
    public GameObject objSelect;
    public GameObject objBlockModel;

    [HideInInspector]
    public BlockInfoBean curSelectBlockInfo;

    //是否正在移动镜头
    protected bool isMoveCamera = false;
    //建造高度
    protected int curBuildHigh = 0;
    //当前选中的方块信息

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
        GameObject objItem = Instantiate(objBlockContainer, objBlockModel.gameObject);
        objItem.transform.position = objSelect.transform.position;
        BuildingEditorModel itemBlock = objItem.GetComponent<BuildingEditorModel>();
        itemBlock.SetData(curSelectBlockInfo);
    }

    /// <summary>
    /// 修改方块建造高度
    /// </summary>
    public void ChangeBlockBuildHigh(int changeHigh)
    {
        curBuildHigh += changeHigh;
        if (curBuildHigh < 0)
            curBuildHigh = 0;
        objPlane.transform.position = objPlane.transform.position.SetY(curBuildHigh);
    }
}