using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingEditorModel : BaseMonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public BlockInfoBean blockInfo;
    public BlockDirectionEnum blockDirection;
    //创建概率
    public float randomRate = 1;

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetData(BlockInfoBean blockInfo, BlockDirectionEnum blockDirection)
    {
        this.blockDirection = blockDirection;
        this.blockInfo = blockInfo;

        Block targetBlock = BlockHandler.Instance.manager.GetRegisterBlock((int)blockInfo.id);
        Mesh targetMesh = targetBlock.blockShape.GetCompleteMeshData(null, Vector3Int.zero, BuildingEditorHandler.Instance.manager.curBlockDirection);

        //显示问题    //向下移动0.5个单位
        Vector3[] verts = targetMesh.vertices;
        Vector3 angleRotate = BlockShape.GetRotateAngles(BuildingEditorHandler.Instance.manager.curBlockDirection);
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 itemPosition = verts[i];
            Vector3 rotatePosition= VectorUtil.GetRotatedPosition(Vector3.zero, itemPosition, angleRotate);
            rotatePosition -= new Vector3(0.5f, 0.5f, 0.5f);
            verts[i] = rotatePosition;
        }
        targetMesh.SetVertices(verts);


        meshFilter.mesh = targetMesh;
        meshRenderer.material = BlockHandler.Instance.manager.GetBlockMaterial(blockInfo.GetBlockMaterialType());
    }
}