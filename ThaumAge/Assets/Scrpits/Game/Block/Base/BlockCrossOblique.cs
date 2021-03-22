using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCrossOblique : BlockCross
{

    public override void AddVerts(Vector3 corner, List<Vector3> verts, List<Vector3> vertsCollider)
    {
        verts.Add(corner);
        verts.Add(corner + new Vector3(0, 1, 0));
        verts.Add(corner + new Vector3(1, 1, 1));
        verts.Add(corner + new Vector3(1, 0, 1));

        verts.Add(corner + new Vector3(1, 0, 0));
        verts.Add(corner + new Vector3(1, 1, 0));
        verts.Add(corner + new Vector3(0, 1, 1));
        verts.Add(corner + new Vector3(0, 0, 1));
    }


}