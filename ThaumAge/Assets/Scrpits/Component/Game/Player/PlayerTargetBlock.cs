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

    protected Vector3Int lastWorldPosition = Vector3Int.one * int.MaxValue;
    public void Awake()
    {
        meshFilter = objTargetBlock.GetComponent<MeshFilter>();
        meshRenderer = objTargetBlock.GetComponent<MeshRenderer>();
    }
    public void Show(Vector3Int worldPosition, Block block, bool isInteractive)
    {

        gameObject.SetActive(true);
        //展示文本互动提示
        objInteractive.ShowObj(isInteractive);

        //如果和上一个方块处于同一个位置
        if (lastWorldPosition == worldPosition)
        {
            lastWorldPosition = worldPosition;
            return;
        }
        lastWorldPosition = worldPosition;

        //设置方向
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        objTargetCenterBlock.transform.eulerAngles = targetBlock.GetRotateAngles(targetDirection);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //如果和上一个时同一个
        Mesh newMeshData = block.blockShape.GetCompleteMeshData(targetChunk, localPosition, targetDirection);
        //设置形状
        if (block.blockShape is BlockShapeCustom blockShapeCustom)
        {

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
        transform.position = worldPosition;
        //如果是link类型，
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.CustomLink)
        {
            BlockBean blockData = targetChunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
            BlockMetaBaseLink blockMetaBaseLink = Block.FromMetaData<BlockMetaBaseLink>(blockData.meta);
            transform.position = blockMetaBaseLink.GetBasePosition();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}