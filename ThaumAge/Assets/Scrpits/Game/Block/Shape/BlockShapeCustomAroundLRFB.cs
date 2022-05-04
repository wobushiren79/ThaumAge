using UnityEditor;
using UnityEngine;

public class BlockShapeCustomAroundLRFB : BlockShapeCustom
{
    protected Vector3[] vertsAddLink;
    protected int[] trisAddLink;
    protected Vector2[] uvsAddLink;
    protected Color[] colorAddLink;

    public override void InitData(Block block)
    {
        base.InitData(block);
        MeshDataDetailsCustom otherMesh = blockMeshData.otherMeshData[0];
        vertsAddLink = otherMesh.vertices;
        trisAddLink = otherMesh.triangles;
        uvsAddLink = otherMesh.uv;

        colorAddLink = new Color[vertsAddLink.Length];
        for (int i = 0; i < colorAddLink.Length;i++)
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
    protected void AddOtherMeshData(Chunk chunk, Vector3Int localPosition,float angle)
    {
        Vector3[] rotatePositionArray = VectorUtil.GetRotatedPosition(new Vector3(0.5f,0.5f,0.5f), vertsAddLink, new Vector3(0, angle, 0));
        BaseAddTrisForCustom(chunk, localPosition, BlockDirectionEnum.UpForward, trisAddLink);
        BaseAddVertsUVsColorsForCustom(chunk, localPosition, BlockDirectionEnum.UpForward,
            rotatePositionArray, uvsAddLink, colorAddLink, new Vector3[0]);
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
        //获取四周的方块 判断是否需要添加
        block.GetCloseBlockByDirection(chunk, localPosition, faceDiection,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int localPositionClose);
        //如果是方块
        if (blockClose.blockInfo.GetBlockShape() == BlockShapeEnum.Cube)
        {
            return true;
        }
        //如果是同一种类型
        if (blockClose.blockType == block.blockType)
        {
            return true;
        }
        return false;
    }
}