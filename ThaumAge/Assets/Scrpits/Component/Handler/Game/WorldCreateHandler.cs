using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    /// <summary>
    /// 建造区块
    /// </summary>
    /// <param name="worldSeed"></param>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="minHeight"></param>
    public void CreateChunk(int worldSeed, Vector3Int position)
    {
        //检测当前位置是否有区块
        Chunk chunk = manager.GetChunk(position);
        if (chunk != null)
        {
            return;
        }
        //设置种子
        manager.SetWorldSeed(worldSeed);
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        objChunk.transform.position = position;
        chunk = objChunk.GetComponent<Chunk>();
        chunk.name = "Chunk_X:" + position.x + "_Y:" + position.y + "_Z:" + position.z;
        //回调
        Action<Dictionary<Vector3Int, Block>> callBack = (mapBlockData) =>
        {
            //设置数据
            chunk.SetData(position, mapBlockData, manager.widthChunk, manager.heightChunk);
        };
        //生成方块数据
        manager.CreateChunkBlockDataForAsync(chunk, callBack);
        //添加区块
        manager.AddChunk(position, chunk);
    }

    /// <summary>
    /// 根据中心位置创建区域chunk
    /// </summary>
    /// <param name="worldSeed"></param>
    /// <param name="centerPosition"></param>
    /// <param name="range"></param>
    public void CreateChunkForRange(int worldSeed, Vector3Int centerPosition, int range)
    {
        Vector3Int startPosition = -manager.widthChunk * range * new Vector3Int(1, 0, 1) + centerPosition;
        Vector3Int currentPosition = startPosition;
        for (int i = 0; i <= range * 2; i++)
        {
            for (int f = 0; f <= range * 2; f++)
            {
                CreateChunk(worldSeed, currentPosition);
                currentPosition += new Vector3Int(0, 0, manager.widthChunk);
            }
            currentPosition.z = startPosition.z;
            currentPosition += new Vector3Int(manager.widthChunk, 0, 0);
        }
    }

    public void CreateChunkForRange(int worldSeed, Vector3 position, int range)
    {
        int positionX = (int)(position.x / manager.widthChunk) * manager.widthChunk;
        int positionZ = (int)(position.z / manager.widthChunk) * manager.widthChunk;
        Vector3Int centerPosition = new Vector3Int(positionX, 0, positionZ);
        CreateChunkForRange(worldSeed, centerPosition, range);
    }

}