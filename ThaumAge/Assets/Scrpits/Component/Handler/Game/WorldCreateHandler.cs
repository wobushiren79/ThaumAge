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

    protected static object lockForUpdateBlock = new object();

    protected void Update()
    {
        HandleForWorldUpdate();
        HandleForDrawChunk();
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
    /// <param name="isInit"></param>
    /// <param name="callBackForCreate"></param>
    public void CreateChunkData(Vector3Int position, Action<Chunk> callBackForCreateData)
    {
        //检测当前位置是否有区块
        Chunk chunk = manager.GetChunk(position);
        if (chunk != null)
        {
            callBackForCreateData?.Invoke(chunk);
            return;
        }
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        objChunk.transform.position = position;
        chunk = objChunk.GetComponent<Chunk>();
        chunk.name = $"Chunk_X:{position.x}_Y:{ position.y}_Z:{position.z}";
        //设置数据
        chunk.SetData(position, manager.widthChunk, manager.heightChunk);
        //添加区块
        manager.AddChunk(position, chunk);

        //成功更新方块后得回调
        Action callBackForUpdateChunk = () =>
        {
            callBackForCreateData?.Invoke(chunk);
        };

        //成功初始化回调
        Action callBackForComplete = () =>
        {
            //更新待更新方块
            HandleForUpdateBlock(callBackForUpdateChunk);
        };

        chunk.BuildChunkBlockDataForAsync(callBackForComplete);
    }

    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callback"></param>
    public void CreateChunkRangeForCenterPosition(Vector3Int centerPosition, int range, bool isInit, Action<Chunk> callBackForComplete)
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
                    Action<Chunk> callBackForCreateData = (successCreateChunkData) =>
                    {
                        //创建所有初始化区块数据完成之后 再一次性更新所有区块
                        totalNumber++;
                        if (totalNumber >= rangeNumber)
                        {
                            totalNumber = 0;
                            Action<Chunk> callBackForUpdateChunk = (successUpdateChunk) =>
                            {
                                totalNumber++;
                                if (totalNumber >= rangeNumber)
                                {
                                    callBackForComplete?.Invoke(successUpdateChunk);
                                }
                            };
                            HandleForUpdateChunk(false, callBackForUpdateChunk);
                        }
                    };
                    CreateChunkData(currentPosition, callBackForCreateData);

                }
                //如果不是初始化创建
                else
                {
                    Action<Chunk> callBackForCreateData = (successCreateChunkData) =>
                    {
                        HandleForUpdateChunk(false, callBackForComplete);
                    };
                    CreateChunkData(currentPosition, callBackForCreateData);
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

    public void CreateChunkRangeForWorldPostion(Vector3 position, int range, Action<Chunk> callBackForComplete)
    {
        Vector3Int centerPosition = GetChunkPositionByWorldPosition(position);
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
        if (manager.listUpdateDrawChunkInit.Count > 0)
        {
            Chunk updateDrawChunk = manager.listUpdateDrawChunkInit.Dequeue();
            if (updateDrawChunk != null && updateDrawChunk.isInit)
            {
                if (!updateDrawChunk.isBuildChunk)
                {
                    updateDrawChunk.DrawMesh();
                }
            }
        }
        if (manager.listUpdateDrawChunkEditor.Count > 0)
        {
            //按照顺序依次渲染 编辑的区块 
            Chunk updateDrawChunk = manager.listUpdateDrawChunkEditor.Dequeue();
            if (updateDrawChunk != null && updateDrawChunk.isInit)
            {
                if (!updateDrawChunk.isBuildChunk)
                {
                    //构建修改过的区块
                    updateDrawChunk.DrawMesh();
                }
            }
        }
        if (manager.listUpdateDrawChunkEditor.Count > 0 || manager.listUpdateDrawChunkInit.Count > 0)
        {
            HandleForDrawChunk();
        }
    }

    /// <summary>
    ///  处理 待更新区块
    /// </summary>
    /// <param name="isOrderDraw">是否按顺序绘制</param>
    /// <param name="callBackForUpdateChunk"></param>
    public void HandleForUpdateChunk(bool isOrderDraw, Action<Chunk> callBackForUpdateChunk)
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
                manager.AddUpdateDrawChunk(updateChunk, 1);
            }
            //构建修改过的区块
            Action callBackForComplete = () =>
            {
                if (!isOrderDraw)
                {
                    //构建修改过的区块
                    manager.AddUpdateDrawChunk(updateChunk, 0);
                }
                callBackForUpdateChunk?.Invoke(updateChunk);
            };
            updateChunk.BuildChunkForAsync(callBackForComplete);
        }
    }


    /// <summary>
    /// 处理 待更新区块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="block"></param>
    /// <param name="direction"></param>
    /// <param name="isRefreshRange">是否刷新周围方块</param>
    public void HandleForUpdateChunk(Chunk chunk, Vector3Int localPosition, Block oldBlock, Block newBlock, BlockDirectionEnum direction = BlockDirectionEnum.UpForward, bool isRefreshRange = true)
    {
        //如果正在构建方块 则先不更新mesh
        if (chunk.isBuildChunk)
            return;
        //如果超过刷新上限 则重新刷新
        if (chunk.chunkMeshData.refreshNumber >= 1024)
        {
            manager.AddUpdateChunk(chunk);
            HandleForUpdateChunk(true, null);
            return;
        }

        //如果是需要刷新周围方块
        if (isRefreshRange)
        {
            //暂时不需要 放在每个方块的refres方法中刷新
            //上
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.up);
            //下
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.down);
            //左
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.left);
            //右
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.right);
            //前
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.forward);
            //后
            //HandleForUpdateChunkClose(chunk, localPosition + Vector3Int.back);
        }

        if (chunk.chunkMeshData.dicIndexData.TryGetValue(localPosition, out ChunkMeshIndexData meshIndexData))
        {
            //先删除指定方块
            if (oldBlock != null)
                oldBlock.RemoveBlockMesh(chunk, localPosition, direction, meshIndexData);
        }
        else
        {

        }
        newBlock.BuildBlock(chunk, localPosition);
        manager.AddUpdateDrawChunk(chunk, 1);
    }

    protected void HandleForUpdateChunkClose(Chunk chunk, Vector3Int localPosition)
    {
        chunk.GetBlockForLocal(localPosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk != null && closeBlock != null && closeBlock.blockType != BlockTypeEnum.None)
        {
            HandleForUpdateChunk(closeChunk, localPosition + chunk.chunkData.positionForWorld - closeChunk.chunkData.positionForWorld, closeBlock, closeBlock, closeBlockDirection, false);
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
                List<BlockTempBean> listNoChunkBlock = new List<BlockTempBean>();
                //添加修改的方块信息，用于树木或建筑群等用于多个区块的数据     
                while (manager.listUpdateBlock.TryDequeue(out BlockTempBean itemBlock))
                {
                    Chunk chunk = manager.GetChunkForWorldPosition(itemBlock.worldX, itemBlock.worldZ);
                    if (chunk != null && chunk.isInit)
                    {
                        //获取保存的数据
                        ChunkSaveBean chunkSaveData = chunk.GetChunkSaveData();
                        int localX = itemBlock.worldX - chunk.chunkData.positionForWorld.x;
                        int localY = itemBlock.worldY;
                        int localZ = itemBlock.worldZ - chunk.chunkData.positionForWorld.z;
                        BlockBean blockData = chunk.GetBlockData(localX, localY, localZ);
                        if (blockData != null)
                        {
                            //如果有存档方块 则不替换
                        }
                        else
                        {
                            //设置方块
                            chunk.SetBlockForLocal(new Vector3Int(localX, localY, localZ), itemBlock.GetBlockType(), itemBlock.GetDirection(), null, false, false, false);
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
}