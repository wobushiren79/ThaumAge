using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    protected Vector3 positionForWorldUpdate = Vector3.zero;

    protected static object lockWorldCreate = new object();
    protected void Update()
    {
        HandleForWorldUpdate();
        HandleForDrawChunk();
        HandleForUpdateChunk();
    }

    /// <summary>
    /// 设置世界类型
    /// </summary>
    /// <param name="worldType"></param>
    public void SetWorldType(WorldTypeEnum worldType)
    {
        manager.worldType = worldType;
        BiomeHandler.Instance.InitWorldBiomeData();
    }

    /// <summary>
    /// 清除世界
    /// </summary>
    public void ClearWorld()
    {
        //清除所有区块
        manager.ClearAllChunk();
        //清除所有寻路
        PathFindingHandler.Instance.manager.InitPathFinding();
        //清理所有生态缓存
        BiomeHandler.Instance.ClearBiomeMapData();
    }

    /// <summary>
    /// 建造区块
    /// </summary>
    public Chunk CreateChunk(Vector3Int position, bool isActive = true, Action<Chunk> callBackForCreateData = null)
    {
        Chunk chunk;
        //===========================如果创建的是激活区块============================
        if (isActive)
        {
            lock (lockWorldCreate)
            {
                //检测当前位置是否有区块
                chunk = manager.GetChunk(position);
                //如果是已经有区块了 并且已经激活了 则不创建了
                if (chunk != null && chunk.isActive)
                {
                    return chunk;
                }
                if (chunk == null)
                {
                    //生成区块
                    chunk = new Chunk();
                    //设置数据
                    chunk.SetData(position, manager.widthChunk, manager.heightChunk);
                    //添加区块
                    manager.AddChunk(position, chunk);
                }
            }
            //创建区块组件(没有创建过组件)
            if (chunk.chunkComponent == null)
            {
                ChunkComponent chunkComponent;
                if (manager.listChunkComponentPool.TryDequeue(out chunkComponent))
                {

                }
                else
                {
                    //生成区块组件
                    GameObject objModel = manager.GetChunkModel();
                    GameObject objChunk = Instantiate(gameObject, objModel);
                    chunkComponent = objChunk.GetComponent<ChunkComponent>();
                }
                chunkComponent.transform.position = position;
                chunkComponent.name = $"Chunk_X:{position.x}_Y:{ position.y}_Z:{position.z}";
                chunkComponent.SetData(chunk);
                chunk.chunkComponent = chunkComponent;
            }
            //开始异步创建方块数据
            chunk.BuildChunkBlockDataForGPUAsync(callBackForCreateData);
            //开始异步创建方块数据
            chunk.isActive = true;
        }
        //===========================如果创建的是不激活的临时方块===========================
        else
        {
            lock (lockWorldCreate)
            {
                //检测当前位置是否有区块
                chunk = manager.GetChunk(position);
                //创建区块
                if (chunk == null)
                {
                    //生成区块
                    chunk = new Chunk();
                    //设置数据
                    chunk.SetData(position, manager.widthChunk, manager.heightChunk);
                    //添加区块
                    manager.AddChunk(position, chunk);
                }
            }
        }
        return chunk;
    }

    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    public void CreateChunkRangeForCenterPosition(Vector3Int centerPosition, int range, bool isInit, Action callBackForComplete)
    {
        manager.worldRefreshRange = range;

        Vector3Int startPosition = -manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int currentPosition = startPosition;
        int totalNumber = 0;
        int rangeNumber = (range * 2) * (range * 2);
        for (int i = 0; i < range * 2; i++)
        {
            for (int f = 0; f < range * 2; f++)
            {
                //如果是初始化创建
                if (isInit)
                {
                    Action<Chunk> callBackForCreateChunk = (successCreateChunk) =>
                    {
                        manager.AddUpdateChunk(successCreateChunk, 0);
                        //创建所有初始化区块数据完成之后 再一次性更新所有区块
                        totalNumber++;
                        if (totalNumber >= rangeNumber)
                        {
                            callBackForComplete?.Invoke();
                        }
                    };
                    CreateChunk(currentPosition, callBackForCreateData: callBackForCreateChunk);
                }
                //如果不是初始化创建
                else
                {
                    Action<Chunk> callBackForCreateChunk = (successCreateChunk) =>
                    {
                        manager.AddUpdateChunk(successCreateChunk, 0);
                    };
                    Chunk createChunk = CreateChunk(currentPosition, callBackForCreateData: callBackForCreateChunk);
                }
                currentPosition += new Vector3Int(0, 0, manager.widthChunk);
            }
            currentPosition.z = startPosition.z;
            currentPosition += new Vector3Int(manager.widthChunk, 0, 0);
        }
    }

    /// <summary>
    /// 删除范围外的所有区块
    /// </summary>
    public void DestroyChunkRangeForCenterPosition(Vector3Int centerPosition, int range, Action callBackForComplete)
    {
        Vector3Int maxPosition = manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int minPosition = -manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;

        //没有初始化的删除范围
        Vector3Int maxNoInitPosition = manager.widthChunk * (range + 10) * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int minNoInitPosition = -manager.widthChunk * (range + 10) * new Vector3Int(1, 0, 1) + centerPosition;

        List<Chunk> listRemoveChunk = new List<Chunk>();
        foreach (var chunk in manager.dicChunk.Values)
        {
            //如果区块已经激活
            if (chunk.isActive)
            {
                if (chunk.chunkData.positionForWorld.x > maxPosition.x
                    || chunk.chunkData.positionForWorld.x < minPosition.x
                    || chunk.chunkData.positionForWorld.z > maxPosition.z
                    || chunk.chunkData.positionForWorld.z < minPosition.z)
                {
                    listRemoveChunk.Add(chunk);
                }
            }
            //如果区块还没有激活
            else
            {
                if (chunk.chunkData.positionForWorld.x > maxNoInitPosition.x
                         || chunk.chunkData.positionForWorld.x < minNoInitPosition.x
                         || chunk.chunkData.positionForWorld.z > maxNoInitPosition.z
                         || chunk.chunkData.positionForWorld.z < minNoInitPosition.z)
                {
                    listRemoveChunk.Add(chunk);
                }
            }
        }

        for (int i = 0; i < listRemoveChunk.Count; i++)
        {
            Chunk chunk = listRemoveChunk[i];
            manager.RemoveChunk(chunk);
        }
        callBackForComplete?.Invoke();
    }

    /// <summary>
    /// 通过角色的坐标 创建一定范围内的区块
    /// </summary>

    public void CreateChunkRangeForWorldPostion(Vector3 position, int range, Action callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkCenterPositionByWorldPosition(position);
        CreateChunkRangeForCenterPosition(centerPosition, range, false, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的坐标 删除一定范围外的区块
    /// </summary>
    public void DestroyChunkRangeForWorldPosition(Vector3 position, int range, Action callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkCenterPositionByWorldPosition(position);
        DestroyChunkRangeForCenterPosition(centerPosition, range, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的世界坐标 获取所在区块的中心坐标
    /// </summary>
    public Vector3Int GetChunkCenterPositionByWorldPosition(Vector3 position)
    {
        int positionX = (int)(position.x / manager.widthChunk) * manager.widthChunk;
        int positionZ = (int)(position.z / manager.widthChunk) * manager.widthChunk;
        Vector3Int centerPosition = new Vector3Int(positionX, 0, positionZ);
        return centerPosition;
    }

    /// <summary>
    /// 处理待绘制的区块
    /// </summary>
    public void HandleForDrawChunk()
    {
        //首先处理初始化的区块
        if (manager.listUpdateDrawChunkInit.Count > 0)
        {
            //因为是更新数据完成之后再加的列表 所以可以直接开始绘制
            while (manager.listUpdateDrawChunkInit.TryDequeue(out Chunk updateDrawChunk))
            {
                if (updateDrawChunk != null && updateDrawChunk.isInit && !updateDrawChunk.isBuildChunk)
                {
                    updateDrawChunk.chunkComponent.DrawMesh();
                }
            }
        }
        //编辑的区块 异步
        if (manager.listUpdateDrawChunkEditorAsync.Count > 0)
        {
            //按照顺序依次渲染 等待数据生成完毕
            manager.listUpdateDrawChunkEditorAsync.TryPeek(out Chunk updateDrawChunk);
            if (updateDrawChunk != null && updateDrawChunk.isInit && !updateDrawChunk.isBuildChunk)
            {
                if (manager.listUpdateDrawChunkEditorAsync.TryDequeue(out updateDrawChunk))
                {
                    //构建修改过的区块
                    updateDrawChunk.chunkComponent.DrawMesh();
                }
            }
        }
        //编辑的区块 同步
        if (manager.listUpdateDrawChunkEditorSync.Count > 0)
        {
            //渲染
            while (manager.listUpdateDrawChunkEditorSync.TryDequeue(out Chunk updateDrawChunk))
            {
                if (updateDrawChunk != null && updateDrawChunk.isInit && !updateDrawChunk.isBuildChunk)
                {
                    //构建修改过的区块
                    updateDrawChunk.chunkComponent.DrawMesh();
                }
            }
        }
    }

    /// <summary>
    ///  处理 待更新区块
    /// </summary>
    public void HandleForUpdateChunk()
    {
        if (manager.listUpdateChunkInit.Count > 0)
        {
            int numberLoop = 0;
            //处理初始化待更新的Chunk
            while (manager.listUpdateChunkInit.TryDequeue(out Chunk updateChunk))
            {
                if (updateChunk == null || !updateChunk.isInit || updateChunk.isDrawMesh)
                {
                    continue;
                }
                Action callBackForComplete = () =>
                {
                    //当更新完数据之后再添加到绘制列表
                    manager.AddUpdateDrawChunk(updateChunk, 0);
                };
                updateChunk.StartBuildChunk(callBackForComplete, true);
                //优化处理 每帧只处理5次的异步生成
                numberLoop++;
                if (numberLoop >= 5)
                    break;
            }
        }
        //处理异步更新的chunk
        if (manager.listUpdateChunkEditorAsync.Count > 0)
        {
            //处理实时更新的chunk
            while (manager.listUpdateChunkEditorAsync.TryDequeue(out Chunk updateChunk))
            {
                if (updateChunk == null || !updateChunk.isInit || updateChunk.isDrawMesh)
                {
                    continue;
                }
                //因为需要按顺序排序 所以先添加到绘制列表
                manager.AddUpdateDrawChunk(updateChunk, 1);
                //异步生成数据
                updateChunk.StartBuildChunk(null, true);
            }
        }
        //处理同步更新的chunk
        if (manager.listUpdateChunkEditorSync.Count > 0)
        {
            //处理实时更新的chunk
            while (manager.listUpdateChunkEditorSync.TryDequeue(out Chunk updateChunk))
            {
                if (updateChunk == null || !updateChunk.isInit || updateChunk.isDrawMesh)
                {
                    continue;
                }
                //同步生成数据
                updateChunk.StartBuildChunk(null, false);
                //添加到绘制列表
                manager.AddUpdateDrawChunk(updateChunk, 2);
            }
        }
    }

    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate(bool isCheckDis = true)
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            //获取玩家位置
            Vector3 playPosition = GameHandler.Instance.manager.player.transform.position;
            //不计算Y轴坐标
            playPosition.y = 0;
            //计算两点距离
            float dis = Vector3.Distance(playPosition, positionForWorldUpdate);
            //获取刷新距离
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            //如果不需要检测距离 就直接刷新
            if (!isCheckDis)
            {
                dis = float.MaxValue;
            }
            //对比距离 大于刷新距离则刷新
            if (dis > manager.widthChunk)
            {
                positionForWorldUpdate = playPosition;
                CreateChunkRangeForWorldPostion(playPosition, gameConfig.worldRefreshRange, null);
                DestroyChunkRangeForWorldPosition(playPosition, gameConfig.worldRefreshRange + gameConfig.worldDestoryRange, null);
            }
        }
    }

    /// <summary>
    /// 检测是否所有 init 的 chunk都加载完毕  包括生成数据 绘制（不包括预加载的区块）
    /// </summary>
    public bool CheckAllInitChunkLoadComplete()
    {
        foreach (var itemData in manager.dicChunk)
        {
            Chunk itemChunk = itemData.Value;
            //如果
            if (itemChunk.isInit)
            {
                if (itemChunk.isBuildChunk)
                    return false;
                if (itemChunk.isDrawMesh)
                    return false;
            }
        }
        if (manager.listUpdateChunkInit.Count > 0)
            return false;
        if (manager.listUpdateDrawChunkInit.Count > 0)
            return false;
        return true;
    }



    /// <summary>
    /// 设置指定点周围的方块
    /// </summary>
    /// <param name="centerPosition">设置位置</param>
    /// <param name="range">设置半径</param>
    /// <param name="setShape">设置形状  0方形 1圆形 </param>
    /// <param name="blockType"></param>
    /// <param name="isOnlySetAir">是否只设置空气，忽略其他的方块</param>
    public void SetBlockRange(Vector3 centerPosition, BlockTypeEnum blockType = BlockTypeEnum.None, int range = 1, int setShape = 0, bool isOnlySetAir = false, float createDrapRate = 1)
    {
        Vector3Int breakPositionInt = Vector3Int.FloorToInt(centerPosition);
        manager.GetBlockForWorldPosition(breakPositionInt, out Block targetBlock, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                for (int z = -range; z <= range; z++)
                {
                    Vector3Int offsetPosition = new Vector3Int(x, y, z);
                    //判断是否是一个圆
                    if (setShape == 1 && Vector3Int.Distance(offsetPosition, Vector3Int.zero) > range)
                    {
                        continue;
                    }
                    Vector3Int itemWorldPosition = breakPositionInt + offsetPosition;
                    manager.GetBlockForWorldPosition(itemWorldPosition, out Block itemBlock, out Chunk itemChunk);
                    if (itemChunk == null || itemBlock == null)
                    {
                        continue;
                    }
                    //如果是设置空气方块 说明是置空
                    if (blockType == BlockTypeEnum.None)
                    {
                        //如果目标位置是空气 则不用设置了
                        if (itemBlock.blockType == BlockTypeEnum.None)
                        {
                            continue;
                        }
                        Vector3Int localItemBlockPosition = itemWorldPosition - itemChunk.chunkData.positionForWorld;
                        //采用同步更新区块的方式，防止前后更新差产生的镂空
                        itemChunk.SetBlockForLocal(localItemBlockPosition, blockType, updateChunkType: 2);
                        //创建掉落物
                        if (createDrapRate > 0 && UnityEngine.Random.Range(0f, 1f) <= createDrapRate)
                        {
                            ItemsHandler.Instance.CreateItemCptDrop(itemBlock, itemChunk, itemWorldPosition);
                        }
                    }
                    else
                    {
                        Vector3Int localItemBlockPosition = itemWorldPosition - itemChunk.chunkData.positionForWorld;
                        if (isOnlySetAir)
                        {
                            if (itemBlock.blockType == BlockTypeEnum.None)
                            {
                                //采用同步更新区块的方式，防止前后更新差产生的镂空
                                itemChunk.SetBlockForLocal(localItemBlockPosition, blockType, updateChunkType: 2);
                            }
                        }
                        else
                        {
                            //采用同步更新区块的方式，防止前后更新差产生的镂空
                            itemChunk.SetBlockForLocal(localItemBlockPosition, blockType, updateChunkType: 2);
                        }
                    }
                }
            }
        }
    }
}