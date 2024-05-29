using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Animations.AimConstraint;

public class CreatureHandler : BaseHandler<CreatureHandler, CreatureManager>
{
    //事件更新事件
    protected float eventUpdateTimeForCreate = 0;
    protected float eventUpdateTimeForCreateLoadData = 0;
    protected float eventUpdateTimeForSaveCreatureData = 0;
    //动物的刷新时间
    public float creatureDelay = 10;
    public float creatureLoadDataDelay = 5;
    //生物数据保存时间
    public float creatureDataSaveDelay = 1;

    //动物的最大数量
    public Dictionary<int, int> creatureMaxNum = new Dictionary<int, int>()
    {
        {(int)CreatureTypeEnum.Animal,10 },
        {(int)CreatureTypeEnum.Monster,10 },
    };
    //待保存的区块生物信息
    public List<ChunkSaveCreatureBean> listSaveCreatureData = new List<ChunkSaveCreatureBean>();

    public void InitData()
    {

    }

    public void Update()
    {
        UpdateForCreatureCreate();
        HandleForCreatureDataSave();
    }

    /// <summary>
    /// 每隔一段时间生成生物
    /// </summary>
    public void UpdateForCreatureCreate()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        //如果游戏正在进行中
        if (gameState == GameStateEnum.Gaming)
        {
            eventUpdateTimeForCreate += Time.deltaTime;
            if (eventUpdateTimeForCreate >= creatureDelay)
            {
                HandleForCreatureCreate();
                eventUpdateTimeForCreate = 0;
            }

            eventUpdateTimeForCreateLoadData += Time.deltaTime;
            if (eventUpdateTimeForCreateLoadData >= creatureLoadDataDelay)
            {
                HandleForCreatureCreateLoadData();
                eventUpdateTimeForCreateLoadData = 0;
            }
        }
    }

    /// <summary>
    /// 每1秒钟检测并且保存一次
    /// </summary>
    public void UpdateForCreatureDataSave() 
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        if (gameState == GameStateEnum.Gaming)
        {
            eventUpdateTimeForSaveCreatureData += Time.deltaTime;
            if (eventUpdateTimeForSaveCreatureData > creatureDataSaveDelay)
            {
                eventUpdateTimeForSaveCreatureData = 0;
                HandleForCreatureDataSave();
            }
        }
    }

    /// <summary>
    /// 处理生物信息保存
    /// </summary>
    public void HandleForCreatureDataSave()
    {
        //保存区块生物信息
        if (!listSaveCreatureData.IsNull())
            GameDataHandler.Instance.manager.SaveCreatureDataAsync(listSaveCreatureData);
    }

    /// <summary>
    /// 处理生物的创建
    /// </summary>
    public void HandleForCreatureCreate()
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
                    //生成周围生物数据
                    HandleForCreatureCreateRange(worldType,  targetChunk);
                }
            }
        }
    }

    /// <summary>
    /// 处理生物的读取创建
    /// </summary>
    public void HandleForCreatureCreateLoadData()
    {
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        //获取控制角色
        Player player = GameHandler.Instance.manager.player;
        if (player != null)
        {
            List<Chunk> listTargetChunk = WorldCreateHandler.Instance.manager.GetChunkForRange(Vector3Int.CeilToInt(player.transform.position), 4);
            for (int i = 0; i < listTargetChunk.Count; i++)
            {
                Chunk targetChunk = listTargetChunk[i];
                if (!targetChunk.isInit || targetChunk.isDrawMesh || !targetChunk.chunkComponent.gameObject.activeSelf)
                    continue;
                //读取生物数据
                HandleForCreatureCreateLoadData(worldType, targetChunk);
            }
        }
    }

    /// <summary>
    /// 处理创建生物时加载数据
    /// </summary>
    /// <param name="worldType"></param>
    /// <param name="targetChunk"></param>
    public void HandleForCreatureCreateLoadData(WorldTypeEnum worldType,Chunk targetChunk)
    {
        //如果已经加载过生物信息
        if (targetChunk.isLoadCreatureData)
            return;
        targetChunk.isLoadCreatureData = true;
        //首先读取该区块已有的生物 并且加载
        GameDataManager gameDataManager = GameDataHandler.Instance.manager;
        UserDataBean userData = gameDataManager.GetUserData();
        ChunkSaveCreatureBean chunkSaveCreature = GameDataHandler.Instance.manager.GetChunkSaveCreatureData(userData.userId, worldType, targetChunk.chunkData.positionForWorld);
        if (chunkSaveCreature != null)
        {
            for (int f = 0; f < chunkSaveCreature.listCreatureData.Count; f++)
            {
                var itemCreatureData = chunkSaveCreature.listCreatureData[f];
                CreateCreature(itemCreatureData.creatureInfoId, itemCreatureData.posForBirth);
            }

            //LogUtil.Log($"HandleForCreatureCreate LoadData {chunkSaveCreature.listCreatureData.Count}");
            //加载数据之后删除这条数据
            //GameDataHandler.Instance.manager.DeleteChunkSaveCreatureData(userData.userId, worldType, targetChunk.chunkData.positionForWorld);
        }
    }

    /// <summary>
    /// 创建周边生物
    /// </summary>
    public void HandleForCreatureCreateRange(WorldTypeEnum worldType, Chunk targetChunk)
    {
        //获取该区块的生物信息
        BiomeInfoBean biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(targetChunk.chunkData.biomeType);
        var listCreateData = biomeInfo.GetCreatureCreateData();
        if (listCreateData.IsNull())
            return;
        var targetCreatureData = listCreateData[UnityEngine.Random.Range(0, listCreateData.Count)];
        var targetCreatureInfo = CreatureInfoCfg.GetItemData(targetCreatureData.creatureId);

        //选取该区块中心 随机高度的一点
        int centerPosOffset = targetChunk.chunkData.chunkWidth / 2;
        int randomPosY = UnityEngine.Random.Range(0, targetChunk.chunkData.chunkHeight / 2);
        //改中心点范围内
        int itemCreateNum = 0;
        for (int rangeX = -targetCreatureData.createRange; rangeX <= targetCreatureData.createRange; rangeX++)
        {
            if (itemCreateNum >= targetCreatureData.createRnageMaxNum)
                break;
            for (int rangeZ = -targetCreatureData.createRange; rangeZ <= targetCreatureData.createRange; rangeZ++)
            {
                if (itemCreateNum >= targetCreatureData.createRnageMaxNum)
                    break;

                if (CheckCanCreatureCreate(targetChunk, targetCreatureInfo.creature_type, centerPosOffset + rangeX, randomPosY, centerPosOffset + rangeZ))
                {
                    Vector3Int targetPos = targetChunk.chunkData.positionForWorld + new Vector3Int(centerPosOffset + rangeX, randomPosY, centerPosOffset + rangeZ);
                    CreateCreature(targetCreatureData.creatureId, new Vector3(targetPos.x + 0.5f, targetPos.y + 0.1f, targetPos.z + 0.5f), (targetCreature) => 
                    {
                        targetCreature.CreatureSave();
                    });
                    LogUtil.Log($"HandleForCreatureCreate targetPos_{targetPos}");
                    itemCreateNum++;
                }
            }
        }
    }

    /// <summary>
    /// 添加待保存的生物区块数据
    /// </summary>
    public void AddCreatureDataSave(WorldTypeEnum worldType, Vector3Int targetChuinkPos, CreatureCptBase creatureCpt)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        //查询生物原来所在的区块数据 并且删除元的数据
        Vector3Int oldChunkPos = WorldCreateHandler.Instance.GetChunkCenterPositionByWorldPosition(creatureCpt.creatureData.posForBirth);
        ChunkSaveCreatureBean oldTargetSaveData = GameDataHandler.Instance.manager.GetChunkSaveCreatureData(userData.userId, worldType, oldChunkPos);
        if (oldTargetSaveData != null)
        {
            oldTargetSaveData.RemoveCreatureData(creatureCpt.creatureData);
            listSaveCreatureData.Add(oldTargetSaveData);
        }

        //查看待保存的区块信息里面有没有该生物所在的区块数据
        ChunkSaveCreatureBean targetSaveData = null;
        for (int i = 0; i < listSaveCreatureData.Count; i++)
        {
            var itemSaveData = listSaveCreatureData[i];
            if (itemSaveData.worldType == (int)worldType && itemSaveData.position == targetChuinkPos)
            {
                targetSaveData = itemSaveData;
            }
        }
        if (targetSaveData == null)
        {
            targetSaveData = new ChunkSaveCreatureBean();
            targetSaveData.position = targetChuinkPos;
            targetSaveData.worldType = (int)worldType;
            targetSaveData.userId = userData.userId;
            listSaveCreatureData.Add(targetSaveData);
        }
        creatureCpt.creatureData.posForBirth = creatureCpt.transform.position;
        targetSaveData.AddCreatureData(creatureCpt.creatureData);
    }

    /// <summary>
    /// 检测是否能创建生物
    /// </summary>
    public bool CheckCanCreatureCreate(Chunk targetChunk, int creatureType, int targetX, int targetY, int targetZ)
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