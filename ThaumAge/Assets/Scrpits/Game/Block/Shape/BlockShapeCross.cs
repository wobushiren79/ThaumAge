using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCross : BlockShape
{

    public BlockShapeCross() : base()
    {
        vertsAdd = new Vector3[]
        {
            new Vector3(0.5f,0f,0f),
            new Vector3(0.5f,1f,0f),
            new Vector3(0.5f,1f,1f),
            new Vector3(0.5f,0f,1f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(1f,1f,0.5f),
            new Vector3(1f,0f,0.5f)
        };
        trisAdd = new int[]
        {
            0,1,2, 0,2,3, 4,5,6, 4,6,7
        };

        colorsAdd = new Color[]
        {
            Color.white,Color.white,Color.white,Color.white,
            Color.white,Color.white,Color.white,Color.white
        };
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
        if (block.blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition,vertsAdd,uvsAdd,colorsAdd);
            //AddMeshIndexData( chunk,  localPosition);
        }
    }

    /// <summary>
    /// 增加保存的mesh数据
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    //protected virtual void AddMeshIndexData(Chunk chunk, Vector3Int localPosition)
    //{
    //    int startVertsIndex = chunk.chunkMeshData.verts.Count;
    //    int startTrisIndex = chunk.chunkMeshData.dicTris[block.blockInfo.material_type].Count;

    //    int startVertsColliderIndex = 0;
    //    int startTrisColliderIndex = 0;

    //    if (block.blockInfo.collider_state == 1)
    //    {
    //        startVertsColliderIndex = chunk.chunkMeshData.vertsCollider.Count;
    //        startTrisColliderIndex = chunk.chunkMeshData.trisCollider.Count;
    //    }
    //    else if (block.blockInfo.trigger_state == 1)
    //    {
    //        startVertsColliderIndex = chunk.chunkMeshData.vertsTrigger.Count;
    //        startTrisColliderIndex = chunk.chunkMeshData.trisTrigger.Count;
    //    }
    //    chunk.chunkMeshData.AddMeshIndexData(localPosition,
    //             startVertsIndex, vertsAdd.Length, startTrisIndex, trisAdd.Length,
    //             startVertsColliderIndex, vertsColliderAdd.Length, startTrisColliderIndex, trisColliderAdd.Length);
    //}


    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection)
    {
        base.BaseAddTris(chunk, localPosition, blockDirection);

        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisData, trisAdd);
        if (block.blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisColliderAdd);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAdd);
        }
    }

    public override void BaseAddVertsUVsColors(
        Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd,Vector2[] uvsAdd,Color[] colorsAdd)
    {
        AddVertsUVsColors(localPosition, blockDirection,
            chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
            vertsAdd, uvsAdd, colorsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }

    public virtual Vector2 GetUVStartPosition(Block block)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (arrayUVData.Length == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        else
        {
            //随机选一个
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        return uvStartPosition;
    }
}