using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Unity.Collections;
using Unity.Jobs;
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

    protected bool isDrawMeshCollider = false;
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

    public void Update()
    {
        if (chunk != null)
            chunk.Update();
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
    /// 是否显示实例放方块
    /// </summary>
    /// <param name="isOptimizeShow"></param>
    public void IsShowEntityBlock(bool isShow)
    {
        objBlockContainer.SetActive(isShow);
    }

    /// <summary>
    /// 是否接受阴影
    /// </summary>
    public void IsCastShadow(bool isShow)
    {
        if (isShow)
        {
            meshRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
        else
        {
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
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
            //定点数判断
            if (chunk.chunkMeshData == null && chunk.chunkMeshData.verts.Count < 3)
            {
                chunk.isDrawMesh = false;
                return;
            }
            InitBlockMats();
            chunk.isDrawMesh = true;
            chunkMesh.Clear();
            chunkMesh.subMeshCount = meshRenderer.materials.Length;
            //设置顶点
            chunkMesh.SetVertices(chunk.chunkMeshData.verts);
            //设置UV
            chunkMesh.SetUVs(0, chunk.chunkMeshData.uvs);
            //设置颜色
            chunkMesh.SetColors(chunk.chunkMeshData.colors);
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

            meshFilter.mesh.Optimize();
            chunkMeshTrigger.Optimize();
            chunkMeshCollider.Optimize();

            //CombineMesh(chunk.chunkMeshData);

            Physics.BakeMesh(chunkMeshCollider.GetInstanceID(), false);
            Physics.BakeMesh(chunkMeshTrigger.GetInstanceID(), false);

            meshCollider.sharedMesh = chunkMeshCollider;
            meshTrigger.sharedMesh = chunkMeshTrigger;

            meshCollider.enabled = true;
            meshTrigger.enabled = true;

            if (chunkMesh.vertexCount >= 3) meshFilter.sharedMesh = chunkMesh;

            if (meshRenderer.renderingLayerMask == 0)
                meshRenderer.renderingLayerMask = 1;

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
        chunk.isDrawMesh = false;
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {
        chunk = null;
        objBlockContainer.transform.DestroyAllChild();
        meshCollider.enabled = false;
        meshTrigger.enabled = false;

        meshRenderer.renderingLayerMask = 0;
        meshFilter.sharedMesh.Clear();
    }

    public struct VertexStruct
    {
        public Vector3 vertice;
        public Vector3 normal;
        public Vector2 uv;
        public Color color;
    }

    public void CombineMesh(ChunkMeshData chunkMeshData)
    {
        //获取输出meshData
        Mesh.MeshDataArray outMeshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData outMesh = outMeshDataArray[0];

        Mesh.MeshDataArray outMeshDataArrayCollider = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData outMeshCollider = outMeshDataArrayCollider[0];

        Mesh.MeshDataArray outMeshDataArrayTrigger = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData outMeshTrigger = outMeshDataArrayTrigger[0];

        int subMeshCount = 0;
        int subMeshIndexCount = 0;

        List<int> trisDataAll = new List<int>();
        List<SubMeshDescriptor> listSubMeshDescriptor = new List<SubMeshDescriptor>();
        for (int i = 0; i < chunkMeshData.dicTris.Length; i++)
        {
            List<int> trisData = chunkMeshData.dicTris[i];
            if (trisData.IsNull())
                continue;
            trisDataAll.AddRange(trisData);
            SubMeshDescriptor subMeshDesc = new SubMeshDescriptor
            {
                indexStart = subMeshIndexCount,
                indexCount = trisData.Count
            };
            listSubMeshDescriptor.Add(subMeshDesc);
            subMeshCount++;
            subMeshIndexCount += trisData.Count;
        }

        VertexStruct[] listVertex = chunkMeshData.GetVertexStruct();
        outMesh.SetVertexBufferParams(listVertex.Length, vertexAttributeDescriptors);

        VertexStruct[] listVertexCollider = chunkMeshData.GetVertexStructCollider();
        outMeshCollider.SetVertexBufferParams(listVertexCollider.Length, vertexAttributeDescriptors);

        VertexStruct[] listVertexTrigger = chunkMeshData.GetVertexStructTrigger();
        outMeshTrigger.SetVertexBufferParams(listVertexTrigger.Length, vertexAttributeDescriptors);

        //获取点信息
        NativeArray<VertexStruct> vertexData = outMesh.GetVertexData<VertexStruct>();
        NativeArray<VertexStruct> vertexDataCollider = outMeshCollider.GetVertexData<VertexStruct>();
        NativeArray<VertexStruct> vertexDataTrigger = outMeshTrigger.GetVertexData<VertexStruct>();
        //设置点信息
        NativeArray<VertexStruct>.Copy(listVertex, vertexData);
        NativeArray<VertexStruct>.Copy(listVertexCollider, vertexDataCollider);
        NativeArray<VertexStruct>.Copy(listVertexTrigger, vertexDataTrigger);

        //设置三角数量
        outMesh.SetIndexBufferParams(trisDataAll.Count, IndexFormat.UInt32);
        outMeshCollider.SetIndexBufferParams(chunkMeshData.trisCollider.Count, IndexFormat.UInt32);
        outMeshTrigger.SetIndexBufferParams(chunkMeshData.trisTrigger.Count, IndexFormat.UInt32);
        //获取三角下标
        NativeArray<int> triangelData = outMesh.GetIndexData<int>();
        NativeArray<int> triangelDataCollider = outMeshCollider.GetIndexData<int>();
        NativeArray<int> triangelDataTrigger = outMeshTrigger.GetIndexData<int>();

        NativeArray<int>.Copy(trisDataAll.ToArray(), triangelData);
        NativeArray<int>.Copy(chunkMeshData.trisCollider.ToArray(), triangelDataCollider);
        NativeArray<int>.Copy(chunkMeshData.trisTrigger.ToArray(), triangelDataTrigger);

        outMesh.subMeshCount = subMeshCount;
        outMeshCollider.subMeshCount = 1;
        outMeshTrigger.subMeshCount = 1;

        for (int i = 0; i < listSubMeshDescriptor.Count; i++)
        {
            outMesh.SetSubMesh(i, listSubMeshDescriptor[i]);
        }
        outMeshCollider.SetSubMesh(0, new SubMeshDescriptor
        {
            indexStart = 0,
            indexCount = chunkMeshData.trisCollider.Count
        });
        outMeshTrigger.SetSubMesh(0, new SubMeshDescriptor
        {
            indexStart = 0,
            indexCount = chunkMeshData.trisTrigger.Count
        });

        Mesh.ApplyAndDisposeWritableMeshData(outMeshDataArray, chunkMesh);
        Mesh.ApplyAndDisposeWritableMeshData(outMeshDataArrayCollider, chunkMeshCollider);
        Mesh.ApplyAndDisposeWritableMeshData(outMeshDataArrayTrigger, chunkMeshTrigger);

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        //chunkMeshCollider.RecalculateNormals();
        //chunkMeshCollider.RecalculateBounds();

        //chunkMeshTrigger.RecalculateNormals();
        //chunkMeshTrigger.RecalculateBounds();

        vertexData.Dispose();
        triangelData.Dispose();

        vertexDataCollider.Dispose();
        triangelDataCollider.Dispose();

        vertexDataTrigger.Dispose();
        triangelDataTrigger.Dispose();
    }

    public static VertexAttributeDescriptor[] vertexAttributeDescriptors = new VertexAttributeDescriptor[]
    {
         new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3,0),
         new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3,0),
         new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2,0),
         new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4,0)
    };

    public struct BakeJob : IJobParallelFor
    {
        private NativeArray<int> meshIds;

        public BakeJob(NativeArray<int> meshIds)
        {
            this.meshIds = meshIds;
        }

        public void Execute(int index)
        {
            Physics.BakeMesh(meshIds[index], false);
        }
    }
}