using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CreatureHandler : BaseHandler<CreatureHandler, CreatureManager>
{
    //事件更新事件
    protected float eventUpdateTime = 0;
    //动物的刷新时间
    public float creatureDelay = 5;

    //动物的最大数量
    public Dictionary<int,int> creatureMaxNum = new Dictionary<int, int>() 
    {
        {(int)CreatureTypeEnum.Animal,10 },
        {(int)CreatureTypeEnum.Monster,10 },
    };

    public void InitData()
    {

    }

    public void Update()
    {
        HandleForCreatureCreate();
    }

    /// <summary>
    /// 处理生物的创建
    /// </summary>
    public void HandleForCreatureCreate()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        //如果游戏正在进行中
        if (gameState == GameStateEnum.Gaming)
        {
            eventUpdateTime += Time.deltaTime;
            if (eventUpdateTime >= creatureDelay)
            {
                WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;

                if (worldType == WorldTypeEnum.Test)
                {
                    //获取控制角色
                    Player player = GameHandler.Instance.manager.player;
                    if (player != null)
                    {
                        List<Chunk> listTargetChunk = WorldCreateHandler.Instance.manager.GetChunkForRange(Vector3Int.CeilToInt(player.transform.position), 2, 4);
                        for (int i = 0; i < listTargetChunk.Count; i++)
                        {
                            Chunk targetChunk = listTargetChunk[i];
                            if (!targetChunk.isInit || targetChunk.isDrawMesh || !targetChunk.chunkComponent.gameObject.activeSelf)
                                continue;
                            //获取该区块的生物信息
                            BiomeInfoBean biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(targetChunk.chunkData.biomeType);
                            var listCreateData = biomeInfo.GetCreatureCreateData();
                            if (listCreateData.IsNull())
                                continue;
                            var targetCreateData = listCreateData[UnityEngine.Random.Range(0, listCreateData.Count)];
                            var targetCreateInfo = CreatureInfoCfg.GetItemData(targetCreateData.creatureId);  

                            //选取该区块中心 随机高度的一点
                            int centerPosOffset = targetChunk.chunkData.chunkWidth / 2;
                            int randomPosY = UnityEngine.Random.Range(0, targetChunk.chunkData.chunkHeight / 2);
                            //改中心点范围内
                            int itemCreateNum = 0;
                            for (int rangeX = -targetCreateData.createRange; rangeX <= targetCreateData.createRange; rangeX++)
                            {
                                if (itemCreateNum >= targetCreateData.createRnageMaxNum)
                                    break;
                                for (int rangeZ = -targetCreateData.createRange; rangeZ <= targetCreateData.createRange; rangeZ++)
                                {
                                    if (itemCreateNum >= targetCreateData.createRnageMaxNum)
                                        break;
                                    if (CheckCanCreatureCreate(targetChunk, targetCreateInfo.creature_type, centerPosOffset + rangeX, randomPosY, centerPosOffset + rangeZ))
                                    {
                                        Vector3Int targetPos = targetChunk.chunkData.positionForWorld + new Vector3Int(centerPosOffset + rangeX, randomPosY, centerPosOffset + rangeZ);
                                        CreateCreature(targetCreateData.creatureId, new Vector3(targetPos.x + 0.5f, targetPos.y + 0.1f, targetPos.z + 0.5f));
                                        LogUtil.Log($"HandleForCreatureCreate targetPos_{targetPos}");
                                        itemCreateNum++;
                                    }
                                }
                            }
                        }
                    }
                }
                eventUpdateTime = 0;
            }
        }
    }

    /// <summary>
    /// 检测是否能创建生物
    /// </summary>
    public bool CheckCanCreatureCreate(Chunk targetChunk,int creatureType, int targetX, int targetY, int targetZ)
    {
        var listTargetCreature = manager.GetCreature(creatureType);
        var targetCreatureMaxNum = creatureMaxNum[creatureType];
        //判断是否超过该类型生物的生成上限
        if (!listTargetCreature.IsNull() && listTargetCreature.Count > targetCreatureMaxNum)
            return false;
        targetChunk.chunkData.GetBlockForLocal(targetX, targetY, targetZ, out Block targetBlock, out BlockDirectionEnum targetDirection);
        if (targetBlock.blockType != BlockTypeEnum.None)
            return false;
        targetChunk.chunkData.GetBlockForLocal(targetX, targetY - 1, targetZ, out Block targetBlockDown, out BlockDirectionEnum targetDirectionDown);
        targetChunk.chunkData.GetBlockForLocal(targetX, targetY + 1, targetZ, out Block targetBlockUp, out BlockDirectionEnum targetDirectionUp);
        //如果上方是空气 下方可以站人 则刷新
        if (targetBlockDown.blockType != BlockTypeEnum.None && targetBlockDown.blockInfo.collider_state == 1 && targetBlockUp.blockType == BlockTypeEnum.None)
        {
            return true;
        }
        return false;
    }



    /// <summary>
    /// 创建生物
    /// </summary>
    public void CreateCreature(long creatureId, Vector3 position, Action<CreatureCptBase> callBackForComplete = null)
    {
        CreatureInfoBean creatureInfo = CreatureInfoCfg.GetItemData(creatureId);
        if (creatureInfo == null)
            return;
        manager.GetCreatureModel(creatureInfo.model_name, (data) =>
        {
            //创建生物
            GameObject objCreature = Instantiate(gameObject, data);
            //设置生物位置
            objCreature.transform.position = position;
            //获取生物组件
            CreatureCptBase creatureCpt = objCreature.GetComponent<CreatureCptBase>();
            //设置生物信息
            creatureCpt.SetData(creatureInfo);
            //回调
            callBackForComplete?.Invoke(creatureCpt);
            //添加到列表里
            manager.AddCreature(creatureCpt);
        });
    }


    /// <summary>
    /// 创建生物血条
    /// </summary>
    public CreatureCptLifeProgress CreateCreatureLifeProgress(GameObject creatureObj)
    {
        //获取模型
        GameObject objLifeProgressModel = manager.GetCreatureLifeProgressModel();
        //实例化
        GameObject objLifeProgress = Instantiate(creatureObj, objLifeProgressModel);
        //获取控件
        CreatureCptLifeProgress creatureCptLife = objLifeProgress.GetComponent<CreatureCptLifeProgress>();
        //设置位置
        objLifeProgress.transform.localPosition = new Vector3(0, 1.5f, 0);
        return creatureCptLife;
    }

    /// <summary>
    /// 删除生物
    /// </summary>
    /// <param name="creatureCpt"></param>
    public void DestoryCreature(CreatureCptBase creatureCpt)
    {
        manager.RemoveCreature(creatureCpt);
        Destroy(creatureCpt.gameObject);
    }
}