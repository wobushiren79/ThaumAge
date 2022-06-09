using System;
using UnityEditor;
using UnityEngine;

public class BuildingEditorHandler : BaseHandler<BuildingEditorHandler, BuildingEditorManager>
{
    /// <summary>
    /// 建造方块
    /// </summary>
    /// <param name="blockPosition"></param>
    public void BuildBlock(Vector3Int blockPosition)
    {
        if (manager.curCreateTyp == 0)
        {
            //建造
            //先删除有的
            if (manager.dicBlockBuild.TryGetValue(blockPosition, out BuildingEditorModel blockEditor))
            {
                Destroy(blockEditor.gameObject);
                manager.dicBlockBuild.Remove(blockPosition);
            }

            GameObject objItem = Instantiate(manager.objBlockContainer, manager.objBlockModel.gameObject);
            objItem.transform.position = blockPosition;
            BuildingEditorModel itemBlock = objItem.GetComponent<BuildingEditorModel>();
            itemBlock.SetData(manager.curSelectBlockInfo);
            manager.dicBlockBuild.Add(blockPosition, itemBlock);
        }
        else if (manager.curCreateTyp == 1)
        {
            //删除
            if (manager.dicBlockBuild.TryGetValue(blockPosition, out BuildingEditorModel blockEditor))
            {
                Destroy(blockEditor.gameObject);
                manager.dicBlockBuild.Remove(blockPosition);
            }
        }

    }

    /// <summary>
    /// 清除所有方块
    /// </summary>
    public void ClearAllBlock()
    {
        manager.objBlockContainer.transform.DestroyAllChild();
    }
}