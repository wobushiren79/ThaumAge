using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BlockShapeFace : BlockShape
{
    public static Vector3[] VertsAddFace = new Vector3[]
    {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
    };

    public static int[] TrisAddFace = new int[]
    {
            0,1,2, 0,2,3
    };

    public static Color[] ColorAddFace = new Color[]
    {
            Color.white,Color.white,Color.white,Color.white
    };

    public static Vector3[] VertsColliderAddFace = new Vector3[]
    {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f),

            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
    };

    public static int[] TrisColliderAddFace = new int[]
    {
            0,1,2, 0,2,3, 4,6,5, 4,7,6
    };
    public BlockShapeFace() : base()
    {
        vertsAdd = VertsAddFace;

        trisAdd = TrisAddFace;

        colorsAdd = ColorAddFace;
    }

    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public override void BaseAddVertsUVsColors(
        Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {

        AddVertsUVsColors(localPosition,
            blockDirection, chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
            vertsAdd, uvsAdd, colorsAdd);

        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsCollider, VertsColliderAddFace);

        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int[] trisAdd)
    {
        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisData, trisAdd);
        if (block.blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, TrisColliderAddFace);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAdd);
        }
    }

    public override void InitData(Block block)
    {
        base.InitData(block);
        Vector2 uvStartPosition = GetUVStartPosition(block);

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }

    public virtual Vector2 GetUVStartPosition(Block block)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        return uvStartPosition;
    }
}