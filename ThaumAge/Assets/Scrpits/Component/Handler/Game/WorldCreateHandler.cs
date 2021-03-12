using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    public void CreateChunk(int worldSeed, int width, int height, int minHeight)
    {
        //设置种子
        manager.SetWorldSeed(worldSeed);
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        Chunk chunk = objChunk.GetComponent<Chunk>();
        //生成方块数据
        Dictionary<Vector3Int, BlockBean> mapBlockData = manager.CreateChunkBlockData(width, height, minHeight);
        //设置数据
        chunk.SetData(mapBlockData, width, height, minHeight);
        //添加区块
        manager.AddChunk(objChunk.transform.position, chunk);
    }



}