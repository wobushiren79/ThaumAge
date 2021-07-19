using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCross : Block
{
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(localPosition, direction, localPosition, chunkData);
        }
    }

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.RefreshBlock(chunk,  localPosition,  direction);
        //获取下方方块
        chunk.GetBlockForLocal(localPosition + Vector3Int.down, out BlockTypeEnum downBlockType, out DirectionEnum downBlockdirection, out bool isInside);
        //获取下方方块数据
        BlockInfoBean downBlockInfo = BlockHandler.Instance.manager.GetBlockInfo(downBlockType);
        //如果下方方块为NONE或者为液体
        if (isInside && (downBlockType == BlockTypeEnum.None || downBlockInfo.GetBlockShape() == BlockShapeEnum.Liquid))
        {
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
            WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk);
        }
    }

    public override void AddTris(Chunk.ChunkRenderData chunkData)
    {
        base.AddTris(chunkData);

        int index = chunkData.verts.Count;
        int triggerIndex = chunkData.vertsTrigger.Count;

        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 1);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 2);

        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 2);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 3);

        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 4);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 5);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 6);

        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 4);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 6);
        chunkData.dicTris[BlockMaterialEnum.BothFaceSwing].Add(index + 7);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 1);
        chunkData.trisTrigger.Add(triggerIndex + 2);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 2);
        chunkData.trisTrigger.Add(triggerIndex + 3);

        chunkData.trisTrigger.Add(triggerIndex + 4);
        chunkData.trisTrigger.Add(triggerIndex + 5);
        chunkData.trisTrigger.Add(triggerIndex + 6);

        chunkData.trisTrigger.Add(triggerIndex + 4);
        chunkData.trisTrigger.Add(triggerIndex + 6);
        chunkData.trisTrigger.Add(triggerIndex + 7);
    }

    public override void AddUVs(Chunk.ChunkRenderData chunkData)
    {
        base.AddUVs(chunkData);

        List<Vector2Int> listData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (CheckUtil.ListIsNull(listData))
        {
            uvStartPosition = Vector2.zero;
        }
        else if (listData.Count == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        }
        else
        {
            //随机选一个
            uvStartPosition = Vector2.zero;
        }
        chunkData.uvs.Add(uvStartPosition);
        chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));

        chunkData.uvs.Add(uvStartPosition);
        chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));
    }

    public override void AddVerts(Vector3Int localPosition, DirectionEnum direction,Vector3 corner, Chunk.ChunkRenderData chunkData)
    {
        base.AddVerts(localPosition, direction,corner, chunkData);
        AddVert(localPosition, direction,chunkData.verts, corner + new Vector3(0.5f, 0, 0));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0.5f, 1, 0));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0.5f, 1, 1));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0.5f, 0, 1));

        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0, 0, 0.5f));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(0, 1, 0.5f));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 1, 0.5f));
        AddVert(localPosition, direction, chunkData.verts, corner + new Vector3(1, 0, 0.5f));


        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0.5f, 0, 0));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0.5f, 1, 0));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0.5f, 1, 1));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0.5f, 0, 1));

        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0, 0, 0.5f));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(0, 1, 0.5f));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 1, 0.5f));
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + new Vector3(1, 0, 0.5f));
    }


}