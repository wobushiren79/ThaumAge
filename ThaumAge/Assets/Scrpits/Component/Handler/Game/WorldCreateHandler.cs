using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    protected float timeForWorldUpdate = 0;

    protected void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            timeForWorldUpdate -= Time.deltaTime;
            if (timeForWorldUpdate <= 0)
            {
                timeForWorldUpdate = 0.2f;
                HandleForWorldUpdate();
            }
            HandleForUpdateChunk(null);
        }

    }

    /// <summary>
    /// 建造区块
    /// </summary>
    /// <param name="worldSeed"></param>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void CreateChunk(Vector3Int position)
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
        //生成方块数据
        chunk.BuildChunkBlockDataForAsync();
    }


    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callback"></param>
    public void CreateChunkForRangeForCenterPosition(Vector3Int centerPosition, int range)
    {
        manager.worldRefreshRange = range;

        Vector3Int startPosition = -manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int currentPosition = startPosition;
        for (int i = 0; i <= range * 2; i++)
        {
            for (int f = 0; f <= range * 2; f++)
            {
                CreateChunk(currentPosition);
                currentPosition += new Vector3Int(0, 0, manager.widthChunk);
            }
            currentPosition.z = startPosition.z;
            currentPosition += new Vector3Int(manager.widthChunk, 0, 0);
        }
    }

    public void CreateChunkForRangeForWorldPostion(Vector3 position, int range)
    {
        int positionX = (int)(position.x / manager.widthChunk) * manager.widthChunk;
        int positionZ = (int)(position.z / manager.widthChunk) * manager.widthChunk;
        Vector3Int centerPosition = new Vector3Int(positionX, 0, positionZ);
        CreateChunkForRangeForCenterPosition(centerPosition, range);
    }

    /// <summary>
    /// 处理 待更新区块
    /// </summary>
    /// <param name="callBack"></param>
    public void HandleForUpdateChunk(Action callBack)
    {
        if (manager.listUpdateChunk.Count > 0)
        {
            manager.listUpdateChunk.TryDequeue(out Chunk updateChunk);
            if (updateChunk == null)
                return;
            if (!updateChunk.isInit || updateChunk.isDrawMesh)
            {
                manager.listUpdateChunk.Enqueue(updateChunk);
                return;
            }
            manager.AddUpdateDrawChunk(updateChunk);
            //构建修改过的区块
            updateChunk.BuildChunkForAsync(null);
        }


        if (manager.listUpdateDrawChunk.Count > 0)
        {
            Chunk updateDrawChunk = manager.listUpdateDrawChunk.Peek();
            if (updateDrawChunk != null)
            {
                if (!updateDrawChunk.isBuildChunk)
                {
                    //构建修改过的区块
                    updateDrawChunk.DrawMesh();
                    manager.listUpdateDrawChunk.Dequeue();
                    //刷新寻路
                    PathFindingHandler.Instance.manager.RefreshPathFinding(updateDrawChunk);
                }
            }
            else
            {
                manager.listUpdateDrawChunk.Dequeue();
            }
        }
        else
        {
            callBack?.Invoke();
        }
    }
    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            Vector3 playPosition = GameHandler.Instance.manager.player.transform.position;
            CreateChunkForRangeForWorldPostion(playPosition, manager.worldRefreshRange);
        }
    }
    /// <summary>
    /// 处理 更新方块
    /// </summary>
    public void HandleForUpdateBlock()
    {
        if (manager.listUpdateBlock.Count <= 0)
        {
            return;
        }
        Thread threadForUpdateBlock = new Thread(() =>
        {
            List<BlockBean> listNoChunkBlock = new List<BlockBean>();
            //添加修改的方块信息，用于树木或建筑群等用于多个区块的数据     
            while (manager.listUpdateBlock.TryDequeue(out BlockBean itemBlock))
            {
                if (itemBlock == null || itemBlock.worldPosition == null)
                {
                    continue;
                }
                Vector3Int positionBlockWorld = itemBlock.worldPosition.GetVector3Int();
                Chunk chunk = manager.GetChunkForWorldPosition(positionBlockWorld);
                if (chunk != null && chunk.isInit)
                {
                    Vector3Int positionBlockLocal = itemBlock.worldPosition.GetVector3Int() - chunk.worldPosition;
                    //需要重新设置一下本地坐标 之前没有记录本地坐标
                    itemBlock.localPosition = new Vector3IntBean(positionBlockLocal);
                    //获取保存的数据
                    WorldDataBean worldData = chunk.GetWorldData();
                    if (worldData == null || worldData.chunkData == null || worldData.chunkData.GetBlockData(positionBlockLocal) == null)
                    {
                        //设置方块
                        chunk.SetBlock(itemBlock, false, false, false);
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
        });
        threadForUpdateBlock.Start();
    }
}