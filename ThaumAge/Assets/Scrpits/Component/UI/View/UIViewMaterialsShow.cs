using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewMaterialsShow : BaseUIView
{

    public override void Awake()
    {
        base.Awake();
        ui_ViewItemShow.ShowObj(false);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="listMaterials"></param>
    public void SetData(List<ItemsBean> listMaterials)
    {
        ui_MaterialContainer.DestroyAllChild(true);
        for (int i = 0; i < listMaterials.Count; i++)
        {
            ItemsBean itemData = listMaterials[i];
            GameObject objItem = Instantiate(ui_MaterialContainer.gameObject, ui_ViewItemShow.gameObject);
            UIViewItemShow viewItem = objItem.GetComponent<UIViewItemShow>();
            viewItem.SetData(itemData);
        }
    }
}