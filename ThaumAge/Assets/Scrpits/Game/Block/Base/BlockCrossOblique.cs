using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3 corner, Chunk.ChunkRenderData chunkData)
    {
        AddVert(chunkData.verts, corner);
        AddVert(chunkData.verts, corner + new Vector3(0, 1, 0));
        AddVert(chunkData.verts, corner + new Vector3(1, 1, 1));
        AddVert(chunkData.verts, corner + new Vector3(1, 0, 1));

        AddVert(chunkData.verts, corner + new Vector3(1, 0, 0));
        AddVert(chunkData.verts, corner + new Vector3(1, 1, 0));
        AddVert(chunkData.verts, corner + new Vector3(0, 1, 1));
        AddVert(chunkData.verts, corner + new Vector3(0, 0, 1));


        AddVert(chunkData.vertsTrigger, corner);
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0, 1, 0));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 1, 1));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 0, 1));

        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 0, 0));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 1, 0));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0, 1, 1));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0, 0, 1));
    }


}