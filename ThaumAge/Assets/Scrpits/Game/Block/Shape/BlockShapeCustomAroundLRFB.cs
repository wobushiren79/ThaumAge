using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustomAroundLRFB : BlockShapeCustom
{
    protected Vector3[] vertsAddLink;
    protected int[] trisAddLink;
    protected Vector2[] uvsAddLink;
    protected Color[] colorAddLink;

    public BlockShapeCustomAroundLRFB(Block block) : base(block)
    {
        MeshDataDetailsCustom otherMesh = blockMeshData.otherMeshData[0];
        vertsAddLink = otherMesh.vertices;
        trisAddLink = otherMesh.triangles;
        uvsAddLink = otherMesh.uv;

        colorAddLink = new Color[vertsAddLink.Length];
        for (int i = 0; i < colorAddLink.Length; i++)
        {
            colorAddLink[i] = Color.white;
        }
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.BuildBlock(chunk, localPosition);
        if (CheckCanLink(chunk, localPosition, DirectionEnum.Left))
        {
            AddOtherMeshData(chunk, localPosition, 90);
        }
        if (CheckCanLink(chunk, localPosition, DirectionEnum.Right))
        {
            AddOtherMeshData(chunk, localPosition, -90);
        }
        if (CheckCanLink(chunk, localPosition, DirectionEnum.Forward))
        {
            AddOtherMeshData(chunk, localPosition, 0);
        }
        if (CheckCanLink(chunk, localPosition, DirectionEnum.Back))
        {
            AddOtherMeshData(chunk, localPosition, 180);
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
        Vector3[] rotatePositionArray = VectorUtil.GetRotatedPosition(new Vector3(0.5f, 0.5f, 0.5f), vertsAddLink, new Vector3(0, angle, 0));
        BaseAddTrisForCustomOther(chunk, localPosition, BlockDirectionEnum.UpForward, trisAddLink);
        BaseAddVertsUVsColorsForCustom(chunk, localPosition, BlockDirectionEnum.UpForward,
            rotatePositionArray, uvsAddLink, colorAddLink, new Vector3[0]);
    }

    protected virtual void BaseAddTrisForCustomOther(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int[] trisAdd)
    {
        int index = chunk.chunkMeshData.verts.Count;
        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        AddTris(index, trisData, trisAdd);
    }

    /// <summary>
    /// 检测该方向是否能链接
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="faceDiection"></param>
    /// <returns></returns>
    protected virtual bool CheckCanLink(Chunk chunk, Vector3Int localPosition, DirectionEnum faceDiection)
    {
        if (block is BlockBaseAroundLRFB blockAroundLRFB)
        {
            return blockAroundLRFB.CheckCanLink(chunk, localPosition, faceDiection);
        }
        return false;
    }
}