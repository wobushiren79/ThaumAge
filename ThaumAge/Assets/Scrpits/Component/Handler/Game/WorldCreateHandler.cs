using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    public void CreateChunk(int width,int height)
    {
        //生成区块
        GameObject objModel = manager.GetChunkModel();
        GameObject objChunk = Instantiate(gameObject, objModel);
        Chunk chunk = objChunk.GetComponent<Chunk>();
        //生成方块数据
        BlockBean[,,] mapBlockData = manager.CreateChunkBlockData(width, height);
        //设置数据
        chunk.SetData(mapBlockData, width, height);
        //添加区块
        manager.AddChunk(objChunk.transform.position, chunk);
    }



}