using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkComponent : BaseMonoBehaviour
{
    [Header("碰撞-需要手动赋值")]
    public MeshCollider meshCollider;
    [Header("触发-需要手动赋值")]
    public MeshCollider meshTrigger;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public Mesh chunkMesh;
    public Mesh chunkMeshCollider;
    public Mesh chunkMeshTrigger;

    protected Chunk chunk;

    public void Awake()
    {
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        chunkMesh = new Mesh();
        chunkMeshCollider = new Mesh();
        chunkMeshTrigger = new Mesh();

        //设置mesh的三角形上限
        chunkMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshCollider.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        chunkMeshTrigger.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        meshFilter.sharedMesh = chunkMesh;
        meshCollider.sharedMesh = chunkMeshCollider;
        meshTrigger.sharedMesh = chunkMeshTrigger;

        //设置mesh的三角形上限
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshTrigger.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //设置为动态变更，理论上可以提高效率
        chunkMesh.MarkDynamic();
        chunkMeshCollider.MarkDynamic();
        chunkMeshTrigger.MarkDynamic();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="chunk"></param>
    public void SetData(Chunk chunk)
    {
        this.chunk = chunk;
    }

    /// <summary>
    /// 初始化mats
    /// </summary>
    protected void InitBlockMats()
    {
        Material[] allBlockMats = BlockHandler.Instance.manager.GetAllBlockMaterial();
        List<Material> newBlockMtas = new List<Material>();
        for (int i = 0; i < allBlockMats.Length; i++)
        {
            List<int> listTrisMat = chunk.chunkMeshData.dicTris[i];
            if (!listTrisMat.IsNull())
            {
                newBlockMtas.Add(allBlockMats[i]);
            }
        }
        meshRenderer.materials = newBlockMtas.ToArray();
    }

    /// <summary>
    /// 刷新网格
    /// </summary>
    public void DrawMesh()
    {
        if (chunk.isBuildChunk)
            return;
        try
        {
            InitBlockMats();
            chunk.isDrawMesh = true;

            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //定点数判断
            if (chunk.chunkMeshData == null && chunk.chunkMeshData.verts.Count < 3)
            {
                chunk.isDrawMesh = false;
                return;
            }
            chunkMesh.Clear();
            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //设置顶点
            chunkMesh.SetVertices(chunk.chunkMeshData.verts);
            //设置UV
            chunkMesh.SetUVs(0, chunk.chunkMeshData.uvs);

            //设置三角（单面渲染，双面渲染,液体）
            int indexMat = 0;
            for (int i = 0; i < chunk.chunkMeshData.dicTris.Length; i++)
            {
                List<int> trisData = chunk.chunkMeshData.dicTris[i];
                if (trisData.IsNull())
                    continue;
                chunkMesh.SetTriangles(trisData, indexMat);
                indexMat++;
            }

            //碰撞数据设置
            if (chunk.chunkMeshData.vertsCollider.Count >= 3)
            {
                chunkMeshCollider.Clear();
                chunkMeshCollider.SetVertices(chunk.chunkMeshData.vertsCollider);
                chunkMeshCollider.SetTriangles(chunk.chunkMeshData.trisCollider, 0);
            }
            //触发数据设置
            if (chunk.chunkMeshData.vertsTrigger.Count >= 3)
            {
                chunkMeshTrigger.Clear();
                chunkMeshTrigger.SetVertices(chunk.chunkMeshData.vertsTrigger);
                chunkMeshTrigger.SetTriangles(chunk.chunkMeshData.trisTrigger, 0);
            }


            //刷新
            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();
            //刷新
            //chunkMeshCollider.RecalculateBounds();
            //chunkMeshCollider.RecalculateNormals();
            //刷新
            //chunkMeshTrigger.RecalculateBounds();
            //chunkMeshTrigger.RecalculateNormals();

            //meshFilter.mesh.Optimize();

            if (chunkMesh.vertexCount >= 3) meshFilter.sharedMesh = chunkMesh;
            meshCollider.sharedMesh = chunkMeshCollider;
            meshTrigger.sharedMesh = chunkMeshTrigger;

            //Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
            //Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);

            //初始化动画
            //AnimForInit(() =>
            //{

            //});
            //刷新寻路
            PathFindingHandler.Instance.manager.RefreshPathFinding(chunk);
        }
        catch (Exception e)
        {
            LogUtil.Log("绘制出错_" + e.ToString());
            chunk.isDrawMesh = false;
        }
        finally
        {
            chunk.isDrawMesh = false;
        }
    }
}