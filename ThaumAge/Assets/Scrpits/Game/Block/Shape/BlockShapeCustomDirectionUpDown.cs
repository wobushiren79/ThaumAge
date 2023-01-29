using UnityEditor;
using UnityEngine;

public class BlockShapeCustomDirectionUpDown : BlockShapeCustom
{
    protected Vector3[] vertsAddDirection;
    protected int[] trisAddDirection;
    protected Vector2[] uvsAddDirection;
    protected Color[] colorsAddDirection;

    public BlockShapeCustomDirectionUpDown(Block block) :base(block)
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
        switch (blockDirection)
        {
            case BlockDirectionEnum.UpForward:
                base.BuildBlock(chunk, localPosition);
                break;
            case BlockDirectionEnum.DownForward:
                AddOtherMeshData(chunk, localPosition);
                break;
        }
    }

    /// <summary>
    /// 增加链接的mesh数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="angle"></param>
    protected void AddOtherMeshData(Chunk chunk, Vector3Int localPosition)
    {
        BaseAddTrisForCustom(chunk, localPosition, BlockDirectionEnum.UpForward, trisAddDirection);
        BaseAddVertsUVsColorsForCustom(chunk, localPosition, BlockDirectionEnum.UpForward,
            vertsAddDirection, uvsAddDirection, colorsAddDirection, vertsColliderAddCustom);
    }

    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection)
    {
        switch (blockDirection)
        {
            case BlockDirectionEnum.UpForward:
                return base.GetCompleteMeshData(chunk, localPosition, blockDirection);
            case BlockDirectionEnum.DownForward:
                Mesh mesh = blockMeshData.GetOtherMesh(0);
                return mesh;
        }
        return null;
    }

    /// <summary>
    /// 设置颜色的亮度
    /// </summary>
    /// <param name="emission"></param>
    public override void SetColorsEmission(float emission)
    {
        base.SetColorsEmission(emission);
        for (int i = 0; i < colorsAddDirection.Length; i++)
        {
            colorsAddDirection[i].a = emission;
        }
    }
}