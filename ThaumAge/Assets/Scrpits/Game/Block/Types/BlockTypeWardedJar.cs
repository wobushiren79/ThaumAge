using UnityEditor;
using UnityEngine;

public class BlockTypeWardedJar : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaWardedJar blockMetaData);
        RefreshObjModel(chunk, localPosition, blockMetaData);
    }


    /// <summary>
    /// 设置液体进度
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, BlockMetaWardedJar blockMetaWardedJar)
    {
        RefreshObjModel(chunk, localPosition, blockMetaWardedJar.GetElementalType(), blockMetaWardedJar.GetElementalPro(), blockMetaWardedJar.elementalTypeForLabel);
    }

    /// <summary>
    /// 设置液体进度
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, ElementalTypeEnum elementalType, float liquidPro, int labelType)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfLiquid = objBlock.transform.Find("Liquid");
        Transform tfLabel = objBlock.transform.Find("Label");
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

        //没有标签
        if (labelType == 0)
        {
            tfLabel.gameObject.SetActive(false);
        }
        else
        {
            tfLabel.gameObject.SetActive(true);
            MeshRenderer meshRenderer = tfLabel.GetComponent<MeshRenderer>();
            ElementalInfoBean elementalInfo =  ElementalInfoCfg.GetItemData((ElementalTypeEnum)labelType);
            meshRenderer.material.SetColor("_ColorElemental",Color.black);

            IconHandler.Instance.manager.GetUISpriteByName(elementalInfo.icon_key, (iconSP) =>
            {
                Texture2D iconTex = TextureUtil.SpriteToTexture2D(iconSP);
                meshRenderer.material.SetTexture("_TexElemental", iconTex);
            });
        }
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        //如果空手
        if (itemData.itemId == 0)
        {
            var controlPlayer = GameControlHandler.Instance.manager.controlForPlayer;
            float shiftInput = controlPlayer.inputActionShift.ReadValue<float>();
            //如果同时按住了shift 
            if (shiftInput != 0)
            {
                //清空元素
                Vector3Int targetLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
                GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaWardedJar blockMetaData);
                blockMetaData.curElemental = 0;
                blockData.SetBlockMeta(blockMetaData);
                targetChunk.SetBlockData(blockData);

                RefreshObjModel(targetChunk, targetLocalPosition, blockMetaData);
                return true;
            }
        }
        //如果是空白标签
        else if (itemData.itemId == 993001)
        {
            var controlPlayer = GameControlHandler.Instance.manager.controlForPlayer;
            float shiftInput = controlPlayer.inputActionShift.ReadValue<float>();

            Vector3Int targetLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
            GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaWardedJar blockMetaData);
            //如果同时按住了shift 则去掉标签
            if (shiftInput != 0)
            {
                if (blockMetaData.elementalTypeForLabel != 0)
                {
                    blockMetaData.elementalTypeForLabel = 0;
                }
            }
            //如果没有按住shift 则安装标签
            else
            {
                //如果瓶子没有标签 并且瓶内有元素
                if(blockMetaData.elementalTypeForLabel == 0 && blockMetaData.elementalType != 0 && blockMetaData.curElemental != 0)
                {
                    blockMetaData.elementalTypeForLabel = blockMetaData.elementalType;
                }
            }

            blockData.SetBlockMeta(blockMetaData);
            targetChunk.SetBlockData(blockData);
            RefreshObjModel(targetChunk, targetLocalPosition, blockMetaData);
            return true;
        }
        return base.TargetUseBlock(user, itemData, targetChunk, targetWorldPosition);
    }

}