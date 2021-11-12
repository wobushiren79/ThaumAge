using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, ChunkMeshData chunkMeshData)
    {
        ChunkMeshVertsData vertsData = chunkMeshData.vertsData;

        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner);
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0, 1, 0));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 1, 1));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 0, 1));
        vertsData.index++;

        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 0, 0));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(1, 1, 0));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0, 1, 1));
        vertsData.index++;
        AddVert(localPosition, direction, vertsData.verts, vertsData.index, corner + new Vector3(0, 0, 1));
        vertsData.index++;

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