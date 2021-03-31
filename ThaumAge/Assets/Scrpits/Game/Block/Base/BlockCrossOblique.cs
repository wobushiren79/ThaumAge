using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3 corner, Chunk.ChunkData chunkData)
    {
        chunkData.verts.Add(corner);
        chunkData.verts.Add(corner + new Vector3(0, 1, 0));
        chunkData.verts.Add(corner + new Vector3(1, 1, 1));
        chunkData.verts.Add(corner + new Vector3(1, 0, 1));

        chunkData.verts.Add(corner + new Vector3(1, 0, 0));
        chunkData.verts.Add(corner + new Vector3(1, 1, 0));
        chunkData.verts.Add(corner + new Vector3(0, 1, 1));
        chunkData.verts.Add(corner + new Vector3(0, 0, 1));
    }


}