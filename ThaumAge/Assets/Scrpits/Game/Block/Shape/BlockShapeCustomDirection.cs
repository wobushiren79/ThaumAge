using UnityEditor;
using UnityEngine;

public class BlockShapeCustomDirection : BlockShapeCustom
{
    protected Vector3[] vertsAddDirection;
    protected int[] trisAddDirection;
    protected Vector2[] uvsAddDirection;
    protected Color[] colorsAddDirection;

    public BlockShapeCustomDirection(Block block) : base(block)
    {
        MeshDataDetailsCustom otherMesh = blockMeshData.otherMeshData[0];
        vertsAddDirection = otherMesh.vertices;
        trisAddDirection = otherMesh.triangles;
        uvsAddDirection = otherMesh.uv;
        colorsAddDirection = new Color[vertsAddDirection.Length];
        for (int i = 0; i < colorsAddDirection.Length; i++)
        {
            colorsAddDirection[i] = Color.white;
        }
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        BlockDirectionEnum blockDirection = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
        int unitTen = MathUtil.GetUnitTen((int)blockDirection);
        switch (unitTen) 
        {
            case 1:
            case 2:
                base.BuildBlock(chunk, localPosition);
                break;
            case 3:
                AddOtherMeshData( chunk,  localPosition, -90);
                break;
            case 4:
                AddOtherMeshData(chunk, localPosition, 90);
                break;
            case 5:
                AddOtherMeshData(chunk, localPosition, 0);
                break;
            case 6:
                AddOtherMeshData(chunk, localPosition, 180);
                break;
        }
    }

    /// <summary>
    /// 增加链接的mesh数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="angle"></param>
    protected void AddOtherMeshData(Chunk chunk, Vector3Int localPosition, float angle)
    {
        Vector3[] rotatePositionArray = RotateOtherMeshVerts(angle);
        BaseAddTrisForCustom(chunk, localPosition, BlockDirectionEnum.UpForward, trisAddDirection);
        BaseAddVertsUVsColorsForCustom(chunk, localPosition, BlockDirectionEnum.UpForward,
            rotatePositionArray, uvsAddDirection, colorsAddDirection, vertsColliderAddCustom);
    }

    protected Vector3[] RotateOtherMeshVerts(float angle)
    {
        return VectorUtil.GetRotatedPosition(new Vector3(0.5f, 0.5f, 0.5f), vertsAddDirection, new Vector3(0, angle, 0));
    }


    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection)
    {
        int unitTen = MathUtil.GetUnitTen((int)blockDirection);
        Vector3[] otherVerts = null;
        switch (unitTen)
        {
            case 1:
            case 2:
                return base.GetCompleteMeshData(chunk, localPosition, blockDirection);
            case 3:
                otherVerts = RotateOtherMeshVerts(-90);
                break;
            case 4:
                otherVerts = RotateOtherMeshVerts(90);
                break;
            case 5:
                otherVerts = RotateOtherMeshVerts(0);
                break;
            case 6:
                otherVerts = RotateOtherMeshVerts(180);
                break;
        }
        Mesh mesh = blockMeshData.GetOtherMesh(0);
        mesh.SetVertices(otherVerts);
        return mesh;
    }

}