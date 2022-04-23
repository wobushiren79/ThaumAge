using UnityEditor;
using UnityEngine;

public class PlayerTargetBlock : BaseMonoBehaviour
{
    //互动
    public GameObject objInteractive;
    public GameObject objTargetBlock;
    public GameObject objTargetCenterBlock;

    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected BlockTypeEnum lastBlockType = BlockTypeEnum.None;
    public void Awake()
    {
        meshFilter = objTargetBlock.GetComponent<MeshFilter>();
        meshRenderer = objTargetBlock.GetComponent<MeshRenderer>();
    }
    public void Show(Vector3Int worldPosition, Block block, bool isInteractive)
    {
        transform.position = worldPosition;
        gameObject.SetActive(true);
        //展示文本互动提示
        objInteractive.ShowObj(isInteractive);

        //如果和上一个时同一个
        if (lastBlockType == block.blockType)
        {
            lastBlockType = block.blockType;
            return;
        }
        lastBlockType = block.blockType;

        //设置形状
        if (block.blockShape is BlockShapeCustom blockShapeCustom)
        {
            meshFilter.mesh = blockShapeCustom.blockMeshData.GetMainMesh();
        }
        else if (block.blockShape is BlockShapeCube blockShapeCube)
        {
            meshFilter.mesh = new Mesh();
            Vector2[] newUVS = new Vector2[BlockShape.vertsColliderAdd.Length];
            for (int i = 0; i < BlockShape.vertsColliderAdd.Length; i++)
            {
                newUVS[i] = Vector2.zero;
            }
            meshFilter.mesh.SetVertices(BlockShape.vertsColliderAdd);
            meshFilter.mesh.SetUVs(0, newUVS);
            meshFilter.mesh.SetTriangles(BlockShape.trisColliderAdd, 0);
        }
        else
        {
            meshFilter.mesh = new Mesh();
            Vector2[] newUVS = new Vector2[block.blockShape.vertsAdd.Length];
            for (int i = 0; i < block.blockShape.vertsAdd.Length; i++)
            {
                newUVS[i] = Vector2.zero;
            }
            meshFilter.mesh.SetVertices(block.blockShape.vertsAdd);
            meshFilter.mesh.SetUVs(0, newUVS);
            meshFilter.mesh.SetTriangles(block.blockShape.trisAdd, 0);
        }
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        objTargetCenterBlock.transform.eulerAngles = BlockShape.GetRotateAngles(targetDirection);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}