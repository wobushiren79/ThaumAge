using System.Collections.Generic;
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
    public void CreateChunk(int worldSeed,Vector3Int position, int width, int height, int minHeight)
    {
        //设置种子
        manager.SetWorldSeed(worldSeed);
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        objChunk.transform.position = position;
        Chunk chunk = objChunk.GetComponent<Chunk>();
        chunk.name = "Chunk_X_" + position.x + "_Y_" + position.y;
        //生成方块数据
        Dictionary<Vector3Int, BlockBean> mapBlockData = manager.CreateChunkBlockData(chunk,width, height, minHeight);
        //设置数据
        chunk.SetData(mapBlockData, width, height, minHeight);
        //添加区块
        manager.AddChunk(position, chunk);
    }

}