using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Chunk.ChunkRenderData chunkData)
    {
        AddVert(localPosition, direction, chunkData.verts, corner);
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0, 1, 0));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 1, 1));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 0, 1));

        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 0, 0));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 1, 0));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0, 1, 1));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0, 0, 1));


        AddVert(localPosition, direction, chunkData.vertsTrigger, corner);
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0, 1, 0));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 1, 1));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 0, 1));

        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 0, 0));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 1, 0));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0, 1, 1));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0, 0, 1));
    }


}