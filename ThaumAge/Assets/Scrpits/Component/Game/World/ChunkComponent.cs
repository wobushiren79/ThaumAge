using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

using static UnityEngine.Mesh;

public class ChunkComponent : BaseMonoBehaviour
{
    [Header("碰撞-需要手动赋值")]
    public MeshCollider meshCollider;
    [Header("触发-需要手动赋值")]
    public MeshCollider meshTrigger;
    [Header("方块实体容器-需要手动赋值")]
    public GameObject objBlockContainer;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public Mesh chunkMesh;
    public Mesh chunkMeshCollider;
    public Mesh chunkMeshTrigger;

    public Chunk chunk;


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
            //chunkMeshTrigger.Optimize();
            //chunkMeshCollider.Optimize();


            if (chunkMesh.vertexCount >= 3) meshFilter.sharedMesh = chunkMesh;
            meshCollider.sharedMesh = chunkMeshCollider;
            meshTrigger.sharedMesh = chunkMeshTrigger;

            meshCollider.enabled = true;
            meshTrigger.enabled = true;

            if (meshRenderer.renderingLayerMask == 0)
                meshRenderer.renderingLayerMask = 1;
            //Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
            //Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);

            //初始化动画
            //AnimForInit(() =>
            //{

            //});
            //刷新寻路
            PathFindingHandler.Instance.manager.RefreshPathFinding(chunk);
            //显示
            gameObject.SetActive(true);
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

    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {
        objBlockContainer.transform.DestroyAllChild();
        meshCollider.enabled = false;
        meshTrigger.enabled = false;

        meshRenderer.renderingLayerMask = 0;
        meshFilter.sharedMesh.Clear();
    }

    struct VertexStruct
    {
        public float3 pos;
        public float3 normal;
        public float4 tangent;
        public float2 uv0;
        public float2 uv1;
    }
    private NativeArray<VertexStruct> mCacheInVertices;

    public void CombineMesh(Mesh targetMesh, Mesh srcMesh)
    {
        Mesh.MeshDataArray inMeshDataArray = Mesh.AcquireReadOnlyMeshData(srcMesh);
        Mesh.MeshData inMesh = inMeshDataArray[0];
        mCacheInVertices = inMesh.GetVertexData<VertexStruct>();

        int vertexCount = srcMesh.vertexCount;
        int indexCount = srcMesh.triangles.Length;

        Mesh.MeshDataArray outMeshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData outMesh = outMeshDataArray[0];
        outMesh.SetVertexBufferParams(vertexCount,
            new VertexAttributeDescriptor(VertexAttribute.Position),
            new VertexAttributeDescriptor(VertexAttribute.Normal),
            new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2));

        outMesh.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

        NativeArray<ushort> indices = outMesh.GetIndexData<ushort>();
        for (int i = 0; i < srcMesh.triangles.Length; ++i)
            indices[i] = (ushort)srcMesh.triangles[i];

        NativeArray<VertexStruct> outVertices = outMesh.GetVertexData<VertexStruct>();
        for (int i = 0; i < mCacheInVertices.Length; i++)
        {
            VertexStruct vert = mCacheInVertices[i];
            vert.pos.x += math.sin(i + Time.time) * 0.03f;
            outVertices[i] = vert;
        }

        outMesh.subMeshCount = 1;
        SubMeshDescriptor subMeshDesc = new SubMeshDescriptor
        {
            indexStart = 0,
            indexCount = indexCount,
            topology = MeshTopology.Triangles
            //firstVertex = 0,
            //vertexCount = vertexCount,
            //bounds = new Bounds(Vector3.zero, Vector3.one * 100f)
        };
        outMesh.SetSubMesh(0, subMeshDesc);

        Mesh.ApplyAndDisposeWritableMeshData(outMeshDataArray, targetMesh);
        targetMesh.RecalculateNormals();
        targetMesh.RecalculateBounds();

        mCacheInVertices.Dispose();
        inMeshDataArray.Dispose();
    }

    //private void Update()
    //{
    //    Mesh.MeshDataArray inMeshDataArray = Mesh.AcquireReadOnlyMeshData(srcMesh);
    //    Mesh.MeshData inMesh = inMeshDataArray[0];
    //    mCacheInVertices = inMesh.GetVertexData<VertexStruct>();

    //    int vertexCount = srcMesh.vertexCount;
    //    int indexCount = srcMesh.triangles.Length;

    //    Mesh.MeshDataArray outMeshDataArray = Mesh.AllocateWritableMeshData(1);
    //    Mesh.MeshData outMesh = outMeshDataArray[0];
    //    outMesh.SetVertexBufferParams(vertexCount,
    //        new VertexAttributeDescriptor(VertexAttribute.Position),
    //        new VertexAttributeDescriptor(VertexAttribute.Normal),
    //        new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
    //        new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
    //        new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2));

    //    outMesh.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

    //    NativeArray<ushort> indices = outMesh.GetIndexData<ushort>();
    //    for (int i = 0; i < srcMesh.triangles.Length; ++i)
    //        indices[i] = (ushort)srcMesh.triangles[i];

    //    NativeArray<VertexStruct> outVertices = outMesh.GetVertexData<VertexStruct>();
    //    for (int i = 0; i < mCacheInVertices.Length; i++)
    //    {
    //        VertexStruct vert = mCacheInVertices[i];
    //        vert.pos.x += math.sin(i + Time.time) * 0.03f;
    //        outVertices[i] = vert;
    //    }

    //    outMesh.subMeshCount = 1;
    //    SubMeshDescriptor subMeshDesc = new SubMeshDescriptor
    //    {
    //        indexStart = 0,
    //        indexCount = indexCount,
    //        topology = MeshTopology.Triangles,
    //        firstVertex = 0,
    //        vertexCount = vertexCount,
    //        bounds = new Bounds(Vector3.zero, Vector3.one * 100f)
    //    };
    //    outMesh.SetSubMesh(0, subMeshDesc);

    //    Mesh.ApplyAndDisposeWritableMeshData(outMeshDataArray, mCacheMesh);
    //    mCacheMesh.RecalculateNormals();
    //    mCacheMesh.RecalculateBounds();

    //    mCacheInVertices.Dispose();
    //    inMeshDataArray.Dispose();
    //}

    public static VertexAttributeDescriptor[] tempvd = new VertexAttributeDescriptor[] {
            new VertexAttributeDescriptor
                (VertexAttribute.Position, VertexAttributeFormat.Float32, 3,0),
                 new VertexAttributeDescriptor
                (VertexAttribute.Normal, VertexAttributeFormat.Float32, 3,0),
                    new VertexAttributeDescriptor
                (VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2,0)
        };


}