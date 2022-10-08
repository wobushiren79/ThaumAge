using UnityEngine;

public class ItemCptHold : BaseMonoBehaviour
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
    /// <param name="itemsInfo"></param>
    public void SetItem(ItemsInfoBean itemsInfo)
    {
        ItemsHandler.Instance.manager.GetItemsIconById(itemsInfo.id,(data)=> 
        {
            Texture2D itemTex = TextureUtil.SpriteToTexture2D(data);
            //设置材质的贴图
            meshRenderer.material.mainTexture = itemTex;
            //获取道具的mesh
            MeshUtil.MeshUtilData meshUtilData = new MeshUtil.MeshUtilData(itemTex, itemsInfo.GetItemsColor());
            Mesh picMesh = MeshUtil.GenerateMeshPicture(meshUtilData);
            meshFilter.mesh = picMesh;
            //设置对应颜色
            meshRenderer.material.SetColor("_BaseColor", meshUtilData.colorMesh);
        });
    }
}