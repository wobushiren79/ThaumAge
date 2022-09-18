using UnityEditor;
using UnityEngine;

public class BlockShapeLiquidCross : BlockShapeLiquid
{
    public BlockShapeLiquidCross(Block block) : base(block)
    {
        vertsAdd = BlockShapeCross.VertsAddCross;
        trisAdd = BlockShapeCross.TrisAddCross;
        colorsAdd = BlockShapeCross.ColorsAddCross;

        Vector2 uvStartPosition = BlockShapeCross.GetUVStartPosition(block);

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.BuildBlock(chunk, localPosition);
        BuildFace(chunk, localPosition, vertsAdd, uvsAdd, colorsAdd, trisAdd);
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
        Mesh mesh = new Mesh();
        mesh.vertices = vertsColliderAdd;
        mesh.triangles = trisColliderAdd;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}