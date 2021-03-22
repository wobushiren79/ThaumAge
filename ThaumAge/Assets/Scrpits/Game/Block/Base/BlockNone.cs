using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockNone : Block
{

    public override void AddTris(int index, List<int> tris, int indexCollider, List<int> trisCollider)
    {
    }

    public override void AddUVs(BlockBean blockData, List<Vector2> uvs)
    {
    }

    public override void AddVerts(Vector3 corner, List<Vector3> verts, List<Vector3> vertsCollider)
    {
    }

    public override void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris, List<Vector3> vertsCollider, List<int> trisCollider)
    {
    }

    public override void BuildFace(BlockBean blockData, Vector3 corner, List<Vector3> verts, List<Vector2> uvs, List<int> tris, List<Vector3> vertsCollider, List<int> trisCollider)
    {
    }
}