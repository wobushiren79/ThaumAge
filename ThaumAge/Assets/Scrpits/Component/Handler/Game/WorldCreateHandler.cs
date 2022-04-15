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
    }

    /// <summary>
    /// 建造区块
    /// </summary>
    /// <param name="position"></param>
    /// <param name="callBackForCreateData"></param>
    /// <param name="isCreateChunkComponent">是否创建区块组件</param>
    /// <param name="isCreateBlockData">是否创建方块数据</param>
    public Chunk CreateChunk(Vector3Int position, Action<Chunk> callBackForCreateData,
         bool isCreateChunkComponent = true, bool isCreateBlockData = true)
    {
        Chunk chunk;

        lock (lockWorldCreate)
        {
            //检测当前位置是否有区块
            chunk = manager.GetChunk(position);
            //创建区块
            if (chunk == null)
            {   //生成区块
                chunk = new Chunk();
                //设置数据
                chunk.SetData(position, manager.widthChunk, manager.heightChunk);
                //添加区块
                manager.AddChunk(position, chunk);
            }
            else
            {
                chunk.isInit = false;
            }
        }
        //创建区块组件(没有创建过组件)
        if (isCreateChunkComponent && chunk.chunkComponent == null)
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
        //创建基础方块数据（没有创建过基础数据）
        if (isCreateBlockData && !chunk.isInit)
        {
            //开始异步创建方块数据
            chunk.BuildChunkBlockDataForAsync(callBackForCreateData);
        }
        else
        {
            callBackForCreateData?.Invoke(chunk);
        }
        return chunk;
    }

    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callback"></param>
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
                    CreateChunk(currentPosition, callBackForCreateChunk);
                }
                //如果不是初始化创建
                else
                {
                    Action<Chunk> callBackForCreateChunk = (successCreateChunk) =>
                    {
                        manager.AddUpdateChunk(successCreateChunk, 0);
                    };
                    CreateChunk(currentPosition, callBackForCreateChunk);
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
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callBackForComplete"></param>
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
            //如果区块已经初始化了
            if (chunk.isInit)
            {
                if (chunk.chunkData.positionForWorld.x > maxPosition.x
                    || chunk.chunkData.positionForWorld.x < minPosition.x
                    || chunk.chunkData.positionForWorld.z > maxPosition.z
                    || chunk.chunkData.positionForWorld.z < minPosition.z)
                {
                    listRemoveChunk.Add(chunk);
                }
            }
            //如果区块还没有初始化
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
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <param name="callBackForComplete"></param>

    public void CreateChunkRangeForWorldPostion(Vector3 position, int range, Action callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkCenterPositionByWorldPosition(position);
        CreateChunkRangeForCenterPosition(centerPosition, range, false, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的坐标 删除一定范围外的区块
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <param name="callBackForComplete"></param>
    public void DestroyChunkRangeForWorldPosition(Vector3 position, int range, Action callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkCenterPositionByWorldPosition(position);
        DestroyChunkRangeForCenterPosition(centerPosition, range, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的世界坐标 获取所在区块的中心坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
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
                if (updateDrawChunk != null && updateDrawChunk.isInit)
                {
                    if (!updateDrawChunk.isBuildChunk)
                    {
                        updateDrawChunk.chunkComponent.DrawMesh();
                    }
                }
            }
        }
        if (manager.listUpdateDrawChunkEditor.Count > 0)
        {
            //按照顺序依次渲染 编辑的区块 
            manager.listUpdateDrawChunkEditor.TryPeek(out Chunk updateDrawChunk);
            if (updateDrawChunk != null && updateDrawChunk.isInit)
            {
                if (!updateDrawChunk.isBuildChunk)
                {
                    if (manager.listUpdateDrawChunkEditor.TryDequeue(out updateDrawChunk))
                    {
                        //构建修改过的区块
                        updateDrawChunk.chunkComponent.DrawMesh();
                    }
                }
            }
        }
        //if (manager.listUpdateDrawChunkInit.Count > 0)
        //{
        //    HandleForDrawChunk();
        //}
    }

    /// <summary>
    ///  处理 待更新区块
    /// </summary>
    public void HandleForUpdateChunk()
    {
        if (manager.listUpdateChunkInit.Count > 0)
        {
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
                updateChunk.BuildChunkForAsync(callBackForComplete);
            }
        }
        if (manager.listUpdateChunkEditor.Count > 0)
        {
            //处理实时更新的chunk
            while (manager.listUpdateChunkEditor.TryDequeue(out Chunk updateChunk))
            {
                if (updateChunk == null || !updateChunk.isInit || updateChunk.isDrawMesh)
                {
                    continue;
                }
                //因为需要按顺序排序 所以先添加到绘制列表
                manager.AddUpdateDrawChunk(updateChunk, 1);
                updateChunk.BuildChunkForAsync(null);
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
}