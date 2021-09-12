using UnityEditor;
using UnityEngine;

public class CharacterEquip
{
    //衣服容器
    public GameObject objClothesContainer;

    public CharacterEquip(GameObject objClothesContainer)
    {
        this.objClothesContainer = objClothesContainer;
    }

    /// <summary>
    /// 改变衣服
    /// </summary>
    /// <param name="clothesId"></param>
    public void ChangeClothes(long clothesId)
    {
        CptUtil.RemoveChild(objClothesContainer.transform);
        if (clothesId == 0)
        {
            //没有衣服
            return;
        }
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(clothesId);
        if (itemsInfo == null)
        {
            LogUtil.LogError($"查询道具数据失败，没有ID为 {clothesId} 的服装数据");
        }
        else
        {
            ItemsHandler.Instance.manager.GetItemsObjById(clothesId,(itemsObj)=> 
            {
                if (itemsObj == null)
                {
                    LogUtil.LogError($"查询道具模型失败，没有ID为 {clothesId} 的道具模型");
                }
                else
                {
                    GameObject objHair = ItemsHandler.Instance.Instantiate(objClothesContainer, itemsObj);
                    objHair.transform.localPosition = Vector3.zero;
                    objHair.transform.localEulerAngles = Vector3.zero;
      
                    ItemsHandler.Instance.manager.GetItemsTexById(itemsInfo.id,(itemTex)=> 
                    {
                        MeshRenderer hairMeshRebderer = objHair.GetComponent<MeshRenderer>();
                        hairMeshRebderer.material.mainTexture = itemTex;
                    });
          
                }
            });
        }
    }
}