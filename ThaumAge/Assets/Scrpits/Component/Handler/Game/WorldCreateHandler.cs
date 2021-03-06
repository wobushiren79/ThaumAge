﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    protected Vector3 positionForWorldUpdate = Vector3.zero;

    protected static object lockForUpdateBlock = new object();

    protected void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForWorldUpdate();
        }
    }

    /// <summary>
    /// 设置世界类型
    /// </summary>
    /// <param name="worldType"></param>
    public void SetWorldType(WorldTypeEnum worldType)
    {
        manager.worldType = worldType;
    }

    /// <summary>
    /// 建造区块
    /// </summary>
    /// <param name="worldSeed"></param>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void CreateChunk(Vector3Int position, Action callBackForCreate)
    {
        //检测当前位置是否有区块
        Chunk chunk = manager.GetChunk(position);
        if (chunk != null)
        {
            return;
        }
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        objChunk.transform.position = position;
        chunk = objChunk.GetComponent<Chunk>();
        chunk.name = "Chunk_X:" + position.x + "_Y:" + position.y + "_Z:" + position.z;
        //设置数据
        chunk.SetData(position, manager.widthChunk, manager.heightChunk);
        //添加区块
        manager.AddChunk(position, chunk);

        //成功更新方块后得回调
        Action callBackForUpdateChunk = () =>
        {
            HandleForUpdateChunk(false, callBackForCreate);
        };

        //成功初始化回调
        Action callBackForComplete = () =>
        {
            //更新待更新方块
            StartCoroutine(CoroutineForDelayUpdateBlock(callBackForUpdateChunk));
        };
        StartCoroutine(CoroutineForDelayBuildChunkBlockData(chunk, callBackForComplete));
    }



    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callback"></param>
    public void CreateChunkRangeForCenterPosition(Vector3Int centerPosition, int range, Action callBackForComplete)
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
                CreateChunk(currentPosition, () =>
                {
                    totalNumber++;
                    if (totalNumber >= rangeNumber)
                    {
                        LogUtil.Log("区块加载完毕");
                        callBackForComplete?.Invoke();
                    }
                });
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
        List<Chunk> listRemoveChunk = new List<Chunk>();
        foreach (var chunk in manager.dicChunk.Values)
        {
            if (chunk.chunkData.positionForWorld.x > maxPosition.x
                || chunk.chunkData.positionForWorld.x < minPosition.x
                || chunk.chunkData.positionForWorld.z > maxPosition.z
                || chunk.chunkData.positionForWorld.z < minPosition.z)
            {
                listRemoveChunk.Add(chunk);
            }
        }

        for (int i = 0; i < listRemoveChunk.Count; i++)
        {
            Chunk chunk = listRemoveChunk[i];
            manager.RemoveChunk(chunk.chunkData.positionForWorld, chunk);
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
        Vector3Int centerPosition = GetChunkPositionByWorldPosition(position);
        CreateChunkRangeForCenterPosition(centerPosition, range, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的坐标 删除一定范围外的区块
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <param name="callBackForComplete"></param>
    public void DestroyChunkRangeForWorldPosition(Vector3 position, int range, Action callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkPositionByWorldPosition(position);
        DestroyChunkRangeForCenterPosition(centerPosition, range, callBackForComplete);
    }

    /// <summary>
    /// 通过角色的世界坐标 获取所在区块的中心坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector3Int GetChunkPositionByWorldPosition(Vector3 position)
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
        if (manager.listUpdateDrawChunk.Count > 0)
        {
            Chunk updateDrawChunk = manager.listUpdateDrawChunk.Peek();
            if (updateDrawChunk != null)
            {
                if (!updateDrawChunk.isBuildChunk)
                {
                    updateDrawChunk = manager.listUpdateDrawChunk.Dequeue();
                    //构建修改过的区块
                    updateDrawChunk.DrawMesh();
                    //继续下一个
                    HandleForDrawChunk();
                }
            }
            else
            {
                manager.listUpdateDrawChunk.Dequeue();
            }
        }
    }


    /// <summary>
    ///  处理 待更新区块
    /// </summary>
    /// <param name="isOrderDraw">是否按顺序绘制</param>
    /// <param name="callBackForUpdateChunk"></param>
    public void HandleForUpdateChunk(bool isOrderDraw, Action callBackForUpdateChunk)
    {
        while (manager.listUpdateChunk.Count > 0)
        {
            manager.listUpdateChunk.TryDequeue(out Chunk updateChunk);
            if (updateChunk == null)
                break;
            if (!updateChunk.isInit || updateChunk.isDrawMesh)
            {
                manager.listUpdateChunk.Enqueue(updateChunk);
                break;
            }
            if (isOrderDraw)
            {
                manager.AddUpdateDrawChunk(updateChunk);
            }
            //构建修改过的区块
            updateChunk.BuildChunkForAsync(() =>
            {
                if (!isOrderDraw)
                {
                    //构建修改过的区块
                    updateChunk.DrawMesh();
                }
                else
                {
                    HandleForDrawChunk();
                }
            });
        }
        callBackForUpdateChunk?.Invoke();
    }


    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate()
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
            SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
            //对比距离 大于刷新距离则刷新
            if (dis > gameInitData.disForWorldUpdate)
            {
                positionForWorldUpdate = playPosition;
                CreateChunkRangeForWorldPostion(playPosition, manager.worldRefreshRange, null);
                DestroyChunkRangeForWorldPosition(playPosition, manager.worldRefreshRange + gameInitData.rangeForWorldUpdateDestory, null);
            }
        }
    }
    /// <summary>
    /// 处理 更新方块
    /// </summary>
    public async void HandleForUpdateBlock(Action callBackForComplete)
    {
        if (manager.listUpdateBlock.Count <= 0)
        {
            callBackForComplete?.Invoke();
            return;
        }
        await Task.Run(() =>
        {
            lock (lockForUpdateBlock)
            {
#if UNITY_EDITOR
                Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
#endif
                List<BlockBean> listNoChunkBlock = new List<BlockBean>();
                //添加修改的方块信息，用于树木或建筑群等用于多个区块的数据     
                while (manager.listUpdateBlock.TryDequeue(out BlockBean itemBlock))
                {
                    Vector3Int positionBlockWorld = itemBlock.worldPosition;
                    Chunk chunk = manager.GetChunkForWorldPosition(positionBlockWorld);
                    if (chunk != null && chunk.isInit)
                    {
                        Vector3Int positionBlockLocal = itemBlock.worldPosition - chunk.chunkData.positionForWorld;
                        //需要重新设置一下本地坐标 之前没有记录本地坐标
                        itemBlock.localPosition = positionBlockLocal;
                        //获取保存的数据
                        WorldDataBean worldData = chunk.GetWorldData();

                        if (worldData == null || worldData.chunkData == null || worldData.chunkData.GetBlockData(positionBlockLocal, out BlockBean blockData))
                        {
                            //如果有存档方块 则不替换
                        }
                        else
                        {
                            //设置方块
                            chunk.SetBlockForLocal(positionBlockLocal, itemBlock.GetBlockType(), itemBlock.GetDirection(), false, false, false);
                            //添加需要更新的chunk
                            manager.AddUpdateChunk(chunk);
                        }
                    }
                    else
                    {
                        listNoChunkBlock.Add(itemBlock);
                    }
                }
                for (int i = 0; i < listNoChunkBlock.Count; i++)
                {
                    manager.AddUpdateBlock(listNoChunkBlock[i]);
                }
#if UNITY_EDITOR
                TimeUtil.GetMethodTimeEnd("Time_HandleForUpdateBlock:", stopwatch);
#endif
            }

        });
        callBackForComplete?.Invoke();
    }


    /// <summary>
    /// 携程延迟生成数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="callBackForComplete"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForDelayBuildChunkBlockData(Chunk chunk, Action callBackForComplete)
    {
        yield return new WaitForEndOfFrame();
        //生成方块数据
        chunk.BuildChunkBlockDataForAsync(callBackForComplete);
    }

    /// <summary>
    /// 延迟刷新数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="callBackForComplete"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForDelayUpdateBlock(Action callBackForComplete)
    {
        yield return new WaitForEndOfFrame();
        HandleForUpdateBlock(callBackForComplete);
    }

    /// <summary>
    /// 携程处理区块更新
    /// </summary>
    /// <param name="updateChunk"></param>
    /// <param name="callBackForComplete"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForDelayUpdateBlock(Chunk updateChunk, Action callBackForComplete)
    {
        yield return new WaitForEndOfFrame();
        updateChunk.BuildChunkForAsync(callBackForComplete);
    }
}