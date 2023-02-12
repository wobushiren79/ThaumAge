using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class BlockTypeGolemPress : BlockBaseLinkLarge
{
    /// <summary>
    /// 获取对应的建筑类型
    /// </summary>
    /// <returns></returns>
    public override BuildingTypeEnum GetBuildingType()
    {
        return BuildingTypeEnum.GolemPress;
    }

    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        var uiGameGolemPress = UIHandler.Instance.OpenUIAndCloseOther<UIGameGolemPress>();
        uiGameGolemPress.SetData(worldPosition);
    }

    public void PlayWorkAnim(Vector3Int worldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        if (chunk == null || block == null || block.blockType != BlockTypeEnum.GolemPress)
        {
            return;
        }
        Vector3Int localPosition = worldPosition - chunk.chunkData.positionForWorld;
        GameObject objItem = chunk.GetBlockObjForLocal(localPosition);
        if (objItem == null)
            return;
        Transform tfGolemPress = objItem.transform.Find("BlockGolemPress");
        if (tfGolemPress == null)
            return;
        MeshRenderer meshRenderer = tfGolemPress.GetComponent<MeshRenderer>();
        meshRenderer.material.DOComplete();
        meshRenderer.material.SetFloat("_WorkPro", 0);
        meshRenderer.material.DOFloat(-20, "_WorkPro", 1).SetLoops(2, LoopType.Yoyo);
    }
}