using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3 corner)
    {
        List<Vector3> verts = chunkMeshData.verts;
        List<Vector3> vertsTrigger = chunkMeshData.vertsTrigger;

        AddVert(localPosition, direction, verts, corner);
        AddVert(localPosition, direction, verts, new Vector3(corner.x, corner.y + 1f, corner.z));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y + 1f, corner.z + 1f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y, corner.z + 1f));

        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y, corner.z));
        AddVert(localPosition, direction, verts, new Vector3(corner.x + 1f, corner.y + 1f, corner.z));
        AddVert(localPosition, direction, verts, new Vector3(corner.x, corner.y + 1f, corner.z + 1f));
        AddVert(localPosition, direction, verts, new Vector3(corner.x, corner.y, corner.z + 1f));

        AddVert(localPosition, direction, vertsTrigger, corner);
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x, corner.y + 1f, corner.z));
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x + 1f, corner.y + 1f, corner.z + 1f));
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x + 1f, corner.y, corner.z + 1f));

        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x + 1f, corner.y, corner.z));
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x + 1f, corner.y + 1f, corner.z));
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x, corner.y + 1f, corner.z + 1f));
        AddVert(localPosition, direction, vertsTrigger, new Vector3(corner.x, corner.y, corner.z + 1f));
    }


}