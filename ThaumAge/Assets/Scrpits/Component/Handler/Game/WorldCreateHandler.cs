using System;
using UnityEngine;

public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    protected float timeForWorldUpdate = 0;

    protected void Update()
    {
        timeForWorldUpdate -= Time.deltaTime;
        if (timeForWorldUpdate <= 0)
        {
            timeForWorldUpdate = 0.2f;
            HandleForWorldUpdate();
        }
    }

    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate()
    {
        Vector3 playPosition = GameHandler.Instance.manager.player.transform.position;
        CreateChunkForRangeForWorldPostion(playPosition, manager.worldRefreshRange, () =>
        {

        });
    }

    /// <summary>
    /// 建造区块
    /// </summary>
    /// <param name="worldSeed"></param>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void CreateChunk(Vector3Int position, Action callback)
    {
        //检测当前位置是否有区块
        Chunk chunk = manager.GetChunk(position);
        if (chunk != null)
        {
            callback?.Invoke();
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

        //回调
        Action callBack = () =>
        {
            //设置数据
            chunk.BuildChunkRangeForAsync();
            //设置回调
            callback?.Invoke();
            //初始化完毕
            chunk.SetInitState(true);
        };
        //生成方块数据
        manager.CreateChunkBlockDataForAsync(chunk, callBack);
    }


    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    /// <param name="callback"></param>
    public void CreateChunkForRangeForCenterPosition(Vector3Int centerPosition, int range, Action callback)
    {
        manager.worldRefreshRange = range;

        Vector3Int startPosition = -manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int currentPosition = startPosition;
        int totalNumber = 0;
        for (int i = 0; i <= range * 2; i++)
        {
            for (int f = 0; f <= range * 2; f++)
            {
                CreateChunk(currentPosition, () =>
                {
                    totalNumber++;
                    if (totalNumber >= (range * 2 + 1) * (range * 2 + 1))
                    {
                        manager.HandleForUpdateBlock(callback);
                    }
                });
                currentPosition += new Vector3Int(0, 0, manager.widthChunk);
            }
            currentPosition.z = startPosition.z;
            currentPosition += new Vector3Int(manager.widthChunk, 0, 0);
        }
    }

    public void CreateChunkForRangeForWorldPostion(Vector3 position, int range, Action callback)
    {
        int positionX = (int)(position.x / manager.widthChunk) * manager.widthChunk;
        int positionZ = (int)(position.z / manager.widthChunk) * manager.widthChunk;
        Vector3Int centerPosition = new Vector3Int(positionX, 0, positionZ);
        CreateChunkForRangeForCenterPosition(centerPosition, range, callback);
    }

}