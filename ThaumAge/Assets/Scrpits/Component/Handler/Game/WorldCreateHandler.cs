using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    public GameObject objModelChunk;


    public void CreateChunk()
    {
        GameObject objChunk = Instantiate(gameObject, objModelChunk);
        Chunk terrainForChunk = objChunk.GetComponent<Chunk>();
        manager.AddChunk(terrainForChunk);
    }


}