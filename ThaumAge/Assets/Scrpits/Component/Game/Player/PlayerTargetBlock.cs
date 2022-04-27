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

        Mesh newMeshData = block.blockShape.GetCompleteMeshData();
        //设置形状
        if (block.blockShape is BlockShapeCustom blockShapeCustom)
        {
            meshFilter.mesh = newMeshData;
        }
        else
        {
            Vector2[] newUVS = new Vector2[newMeshData.vertices.Length];
            for (int i = 0; i < newMeshData.vertices.Length; i++)
            {
                newUVS[i] = Vector2.zero;
            }
            newMeshData.SetUVs(0, newUVS);
        }
        meshFilter.mesh = newMeshData;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        objTargetCenterBlock.transform.eulerAngles = BlockShape.GetRotateAngles(targetDirection);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}