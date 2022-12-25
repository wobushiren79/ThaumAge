using UnityEditor;
using UnityEngine;

public class ItemCptShow : BaseMonoBehaviour
{
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 设置ITEM
    /// </summary>
    public virtual void SetItem(ItemsBean itemData, ItemsInfoBean itemsInfo, float rotationSpeed = 0)
    {
        Item item = ItemsHandler.Instance.manager.GetRegisterItem(itemsInfo.id, itemsInfo.GetItemsType());
        item.GetItemIconTex(itemData, itemsInfo, (itemTex) =>
        {
            //设置材质的贴图
            meshRenderer.material.mainTexture = itemTex;
            //获取道具的mesh
            MeshUtil.MeshUtilData meshUtilData;
            Color colorItem = item.GetItemIconColor(itemData, itemsInfo);
            meshUtilData = new MeshUtil.MeshUtilData(itemTex, colorItem);
            Mesh picMesh = MeshUtil.GenerateMeshPicture(meshUtilData);
            meshFilter.mesh = picMesh;
            //设置对应颜色
            meshRenderer.material.SetColor("_BaseColor", meshUtilData.colorMesh);
            meshRenderer.material.SetFloat("_RotationSpeed", rotationSpeed);
        });
    }
}