using UnityEditor;
using UnityEngine;

public class SceneElementBlockFire : SceneElementBlockBase
{
    public GameObject objFire;
    protected float timeFire = 0;
    public override void SetData(SceneElementBlockBean sceneElementBlockData)
    {
        base.SetData(sceneElementBlockData);
        //生成粒子特效
        SceneElementHandler sceneElementHandler = SceneElementHandler.Instance;
        objFire = SceneElementHandler.Instance.Instantiate(sceneElementHandler.gameObject, sceneElementHandler.manager.objBlockFire);
        objFire.transform.position = sceneElementBlockData.position + new Vector3(0.5f, 0f, 0.5f);
    }

    public override void Update()
    {
        timeFire += 1;
        if (timeFire >= sceneElementBlockData.timeForWork)
        {
            HandleForFire();
        }
    }

    public override void Destory()
    {
        GameObject.Destroy(objFire);
        base.Destory();
    }

    /// <summary>
    /// 处理火势
    /// </summary>
    public virtual void HandleForFire()
    {
        Vector3Int firePosition = sceneElementBlockData.position;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(firePosition, out Block targetBlock, out Chunk targetChunk);
        if (targetChunk != null && targetBlock != null)
        {
            var blockItemInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(targetBlock.blockType);
            //如果当前方块的木元素大于0 则可以燃烧摧毁
            if (blockItemInfo.GetElemental(ElementalTypeEnum.Wood) > 0)
            {
                //摧毁当前方块
                targetChunk.RemoveBlockForLocal(firePosition - targetChunk.chunkData.positionForWorld);
            }
            HandleForItemBlock(firePosition + Vector3Int.left);
            HandleForItemBlock(firePosition + Vector3Int.right);
            HandleForItemBlock(firePosition + Vector3Int.up);
            HandleForItemBlock(firePosition + Vector3Int.down);
            HandleForItemBlock(firePosition + Vector3Int.forward);
            HandleForItemBlock(firePosition + Vector3Int.back);
        }
        //删除自身
        Destory();
    }

    protected void HandleForItemBlock(Vector3Int worldBlockPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldBlockPosition, out Block targetBlock, out Chunk targetChunk);
        if (targetChunk == null || targetBlock == null|| targetBlock.blockType == BlockTypeEnum.None)
            return;
        var blockItemInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(targetBlock.blockType);
        //如果当前方块的木元素大于0 则可以燃烧
        if (blockItemInfo.GetElemental(ElementalTypeEnum.Wood) > 0)
        {                
            //生成火
            SceneElementBlockBean sceneElementBlockData = new SceneElementBlockBean();
            sceneElementBlockData.position = worldBlockPosition;
            sceneElementBlockData.elementalType = (int)ElementalTypeEnum.Fire;
            SceneElementHandler.Instance.CreateSceneElementBlock(sceneElementBlockData);
        }
    }
}