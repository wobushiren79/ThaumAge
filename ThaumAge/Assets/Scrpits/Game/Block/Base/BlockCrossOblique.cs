using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0, 1, 0));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 1, 1));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 0, 1));
        chunkMeshData.indexVert++;

        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 0, 0));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(1, 1, 0));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0, 1, 1));
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + new Vector3(0, 0, 1));
        chunkMeshData.indexVert++;

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 1, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 1, 1));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 0, 1));

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 0, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(1, 1, 0));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 1, 1));
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + new Vector3(0, 0, 1));
    }


}