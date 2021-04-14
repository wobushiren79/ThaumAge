using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCross : Block
{
    public override void BuildBlock(Chunk.ChunkData chunkData)
    {
        base.BuildBlock(chunkData);

        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(blockData, localPosition, chunkData);
        }
    }

    public override void RefreshBlock()
    {
        base.RefreshBlock();
        Block blockDown = chunk.GetBlockForLocal(localPosition + Vector3Int.down);
        //如果下方方块为NONE或者为液体
        if (blockDown.blockData.GetBlockType() == BlockTypeEnum.None || blockDown.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
        {
            BlockBean newBlockData = new BlockBean(BlockTypeEnum.None, localPosition, worldPosition);
            chunk.listUpdateBlock.Add(newBlockData);
        }
    }

    public override void AddTris(Chunk.ChunkData chunkData)
    {
        base.AddTris(chunkData);

        int index = chunkData.verts.Count;

        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 1);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 2);

        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 2);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 3);

        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 4);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 5);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 6);

        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 4);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 6);
        chunkData.dicTris[BlockMaterialEnum.BothFace].Add(index + 7);
    }

    public override void AddUVs(BlockBean blockData, Chunk.ChunkData chunkData)
    {
        base.AddUVs(blockData, chunkData);

        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.GetBlockType());
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

    public override void AddVerts(Vector3 corner, Chunk.ChunkData chunkData)
    {
        base.AddVerts(corner, chunkData);
        AddVert(chunkData.verts, corner + new Vector3(0.5f, 0, 0));
        AddVert(chunkData.verts, corner + new Vector3(0.5f, 1, 0));
        AddVert(chunkData.verts, corner + new Vector3(0.5f, 1, 1));
        AddVert(chunkData.verts, corner + new Vector3(0.5f, 0, 1));

        AddVert(chunkData.verts, corner + new Vector3(0, 0, 0.5f));
        AddVert(chunkData.verts, corner + new Vector3(0, 1, 0.5f));
        AddVert(chunkData.verts, corner + new Vector3(1, 1, 0.5f));
        AddVert(chunkData.verts, corner + new Vector3(1, 0, 0.5f));


        AddVert(chunkData.vertsTrigger, corner + new Vector3(0.5f, 0, 0));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0.5f, 1, 0));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0.5f, 1, 1));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0.5f, 0, 1));

        AddVert(chunkData.vertsTrigger, corner + new Vector3(0, 0, 0.5f));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(0, 1, 0.5f));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 1, 0.5f));
        AddVert(chunkData.vertsTrigger, corner + new Vector3(1, 0, 0.5f));
    }


}