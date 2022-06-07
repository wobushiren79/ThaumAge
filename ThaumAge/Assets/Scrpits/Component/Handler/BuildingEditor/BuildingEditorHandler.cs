using System;
using UnityEditor;
using UnityEngine;

public class BuildingEditorHandler : BaseHandler<BuildingEditorHandler, BuildingEditorManager>
{
    /// <summary>
    /// 建造方块
    /// </summary>
    /// <param name="blockPosition"></param>
    public void BuildBlock(Vector3 blockPosition)
    {
        GameObject objItem = Instantiate(manager.objBlockContainer, manager.objBlockModel.gameObject);
        objItem.transform.position = blockPosition;
        BuildingEditorModel itemBlock = objItem.GetComponent<BuildingEditorModel>();
        itemBlock.SetData(manager.curSelectBlockInfo);
    }

    /// <summary>
    /// 清除所有方块
    /// </summary>
    public void ClearAllBlock()
    {
        manager.objBlockContainer.transform.DestroyAllChild();
    }
}