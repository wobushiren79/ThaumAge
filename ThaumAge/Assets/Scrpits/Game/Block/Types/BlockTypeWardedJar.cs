using UnityEditor;
using UnityEngine;

public class BlockTypeWardedJar : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaWardedJar blockMetaWardedJar = null;
        if (blockData == null)
        {

        }
        else
        {
            blockMetaWardedJar = blockData.GetBlockMeta<BlockMetaWardedJar>();
        }
        if (blockMetaWardedJar == null)
        {
            blockMetaWardedJar = new BlockMetaWardedJar();
        }
        RefreshObjModel(chunk, localPosition, blockMetaWardedJar);
    }

    /// <summary>
    /// 设置液体进度
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, BlockMetaWardedJar blockMetaWardedJar)
    {
        RefreshObjModel(chunk, localPosition, blockMetaWardedJar.GetElementalType(), blockMetaWardedJar.GetElementalPro());
    }

    /// <summary>
    /// 设置液体进度
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, ElementalTypeEnum elementalType, float liquidPro)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfLiquid = objBlock.transform.Find("Liquid");
        if (liquidPro == 0)
        {
            tfLiquid.ShowObj(false);
        }
        else
        {
            tfLiquid.ShowObj(true);
            var elementalInfo = ElementalInfoCfg.GetItemData(elementalType);
            ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out Color elementalColor);

            MeshRenderer meshRenderer = tfLiquid.GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("_Fill", liquidPro - 0.5f);
            meshRenderer.material.SetFloat("_WobbleX", 0);
            meshRenderer.material.SetFloat("_WobbleZ", 0);
            meshRenderer.material.SetFloat("_Emission", 0f);
            meshRenderer.material.SetFloat("_FresnelPower", 0);
            meshRenderer.material.SetColor("_LiquidColor", elementalColor);
            meshRenderer.material.SetColor("_TopColor", elementalColor * 0.9f);
        }
    }

    //public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    //{
    //    BlockBean blockData = targetChunk.GetBlockData(targetWorldPosition - targetChunk.chunkData.positionForWorld);
    //    BlockMetaWardedJar blockMetaWardedJar = null;
    //    if (blockData == null)
    //    {

    //    }
    //    else
    //    {
    //        blockMetaWardedJar = blockData.GetBlockMeta<BlockMetaWardedJar>();
    //    }
    //    if (blockMetaWardedJar == null)
    //    {
    //        blockMetaWardedJar = new BlockMetaWardedJar();
    //    }
    //    blockMetaWardedJar.curElemental += 10;
    //    blockData.SetBlockMeta(blockMetaWardedJar);
    //    targetChunk.SetBlockData(blockData);

    //    SetLiquidPro(targetChunk, targetWorldPosition - targetChunk.chunkData.positionForWorld, blockMetaWardedJar);

    //    return base.TargetUseBlock(user, itemData, targetChunk, targetWorldPosition);
    //}

}