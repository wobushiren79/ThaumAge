using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropCross : BlockShapeCross
{
    public BlockShapeCropCross(Block block) : base(block)
    {
        vertsAdd = vertsAdd.AddY(-(1f / 16f));
    }

    public float[] arrayVertsColliderHeight;
    /// <summary>
    /// 初始化碰撞
    /// </summary>
    public override void InitVertsColliderAdd()
    {
        vertsColliderAddBuffer = VertsColliderAddCube;
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public void GetUVsAddForCrop(Chunk chunk, Block block, Vector3Int localPosition, BlockInfoBean blockInfo,
        out Vector2[] uvsAdd, out Vector3[] vertsColliderAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockCropData = BlockBaseCrop.FromMetaData<BlockMetaCrop>(blockData.meta);

        //设置UV
        Vector2 uvStart = BlockBaseCrop.GetUVStartPosition(block, blockInfo, blockCropData);
        uvsAdd = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y)
        };

        vertsColliderAdd = GetVertsColliderAdd(blockCropData);
    }

    /// <summary>
    /// 获取碰撞点集合
    /// </summary>
    public virtual Vector3[] GetVertsColliderAdd(BlockMetaCrop blockCropData)
    {
        Vector3[] vertsColliderAdd;
        //设置碰撞
        if (block.blockInfo.remark_string.IsNull())
        {
            vertsColliderAdd = VertsColliderAddCube;
        }
        else
        {
            //用备注信息来设置高
            float[] arrayHeightRate = block.blockInfo.remark_string.SplitForArrayFloat('|');
            float heightRate = arrayHeightRate[blockCropData.growPro];
            vertsColliderAdd = VertsColliderAddCube.MultiplyY(heightRate);
        }
        return vertsColliderAdd;
    }

    /// <summary>
    /// 重新选中的方块预览 方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public override Mesh GetSelectMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockCropData = BlockBaseCrop.FromMetaData<BlockMetaCrop>(blockData.meta);
        Vector3[] vertsColliderAdd = GetVertsColliderAdd(blockCropData);

        Mesh mesh = new Mesh();
        mesh.vertices = vertsColliderAdd;
        mesh.triangles = trisColliderAddBuffer;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public override void BaseAddVertsUVsColors(
        Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd,
        Vector3[] vertsColliderAdd, Vector3[] vertsTriggerAdd)
    {
        GetUVsAddForCrop(chunk, block, localPosition, block.blockInfo, out Vector2[] uvsAddNew, out Vector3[] vertsColliderAddNew);
        base.BaseAddVertsUVsColors(chunk, localPosition, blockDirection, vertsAdd, uvsAddNew, colorsAdd, vertsColliderAddNew, vertsColliderAddNew);
    }
}