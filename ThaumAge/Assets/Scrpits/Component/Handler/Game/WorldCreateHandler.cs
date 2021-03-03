using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class WorldCreateHandler : BaseHandler<WorldCreateHandler, WorldCreateManager>
{
    public GameObject objModelChunk;


    public void CreateTerrainChunk()
    {
        GameObject objChunk = Instantiate(gameObject, objModelChunk);
        TerrainForChunk terrainForChunk = objChunk.GetComponent<TerrainForChunk>();
        manager.AddChunk(terrainForChunk);
    }


}