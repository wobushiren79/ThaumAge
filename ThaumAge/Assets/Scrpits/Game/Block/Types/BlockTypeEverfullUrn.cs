using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class BlockTypeEverfullUrn : Block
{
    //总共的搜索层数
    public static int SearchLayerTotal = 4;

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
        {
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        }
    }

    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaEverfullUrn blockMetaEverfull);
        //是否要保存数据（如果没有设置水的情况下可以不保存 节省一点保存资源）
        bool isSaveData = false;
        //获取周围方块 每次获取一层
        Vector3Int targetWorldPosition = localPosition + chunk.chunkData.positionForWorld;
        HandleRoundBlock(targetWorldPosition, blockMetaEverfull.searchLayer, SearchLayerTotal / 2,
            (itemChunk, itemBlock, itemWorldPosition) =>
            {
                //如果是坩埚 则加水
                if (itemBlock.blockType == BlockTypeEnum.Crucible)
                {
                    Vector3Int itemLocalPosition = itemWorldPosition - itemChunk.chunkData.positionForWorld;
                    GetBlockMetaData(itemChunk, itemLocalPosition, out BlockBean itemBlockData,out BlockMetaCrucible blockMetaCrucible);
                    if (blockMetaCrucible.waterLevel < BlockTypeCrucible.WaterLevelMax)
                    {
                        BlockTypeCrucible blockTypeCrucible = itemBlock as BlockTypeCrucible;
                        blockTypeCrucible.SaveCrucibleData(itemChunk, itemLocalPosition, itemBlockData, blockMetaCrucible, BlockTypeCrucible.WaterLevelMax);
                        PlayAddWaterEffect(targetWorldPosition + new Vector3(0.5f, 0.9f, 0.5f), itemWorldPosition + new Vector3(0.5f, 0.6f, 0.5f));
                        isSaveData = true;
                    }
                }

            });
        //增加搜索层数
        blockMetaEverfull.searchLayer++;
        if (blockMetaEverfull.searchLayer > SearchLayerTotal)
        {
            blockMetaEverfull.searchLayer = 0;
        }
        blockData.SetBlockMeta(blockMetaEverfull);
        chunk.SetBlockData(blockData, isSaveData);
    }

    /// <summary>
    /// 获取某一层的方块
    /// </summary>
    /// <returns></returns>
    public void HandleRoundBlock(Vector3Int worldPosition, int layer, int range = 1, Action<Chunk, Block, Vector3Int> itemHandle = null)
    {
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                int y = layer - range;
                if (x == 0 && y == 0 && z == 0)
                    continue;
                Vector3Int targetPosition = worldPosition + new Vector3Int(x, y, z);
                WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out Chunk targetChunk);
                if (targetChunk != null && targetBlock != null)
                {
                    itemHandle?.Invoke(targetChunk, targetBlock, targetPosition);
                }
            }
        }
    }

    /// <summary>
    /// 播放添加水特效
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="targetPosition"></param>
    public void PlayAddWaterEffect(Vector3 startPosition, Vector3 targetPosition)
    {
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Line_2;
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.timeForShow = 5;
        effectData.effectPosition = startPosition;
        effectData.isPlayInShow = false;

        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
         {
             Vector3 pos1 = Vector3.Lerp(startPosition, targetPosition, 0.2f);
             Vector3 pos2 = Vector3.Lerp(startPosition, targetPosition, 0.9f);

             VisualEffect visualEffect = effect.GetComponent<VisualEffect>();
             visualEffect.SetVector3("PosStart", startPosition);
             visualEffect.SetVector3("Pos1", pos1 + new Vector3(0, 0.5f, 0));
             visualEffect.SetVector3("Pos2", pos2 + new Vector3(0, 1f, 0));
             visualEffect.SetVector3("PosEnd", targetPosition);
             visualEffect.SetFloat("EffectSize", 0.1f);
             visualEffect.SetFloat("EffectStartWaitMin", 0f);
             visualEffect.SetFloat("EffectStartWaitMax", 0.05f);
             visualEffect.SetFloat("EffectLifetimeMin", 1);
             visualEffect.SetFloat("EffectLifetimeMax", 3);
             visualEffect.SetFloat("EffectNum", 80);
             visualEffect.SetVector4("ColorRandom1", new Vector4(0.3f, 0.65f, 1, 1));
             visualEffect.SetVector4("ColorRandom2", new Vector4(0.3f, 0.65f, 1, 1));
             visualEffect.SendEvent("OnPlay");

             EffectHandler.Instance.WaitExecuteSeconds(2, () =>
             {
                 visualEffect.SendEvent("OnStop");
             });
         });
    }
}