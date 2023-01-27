using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerTargetBlock : BaseMonoBehaviour
{
    //互动
    public GameObject objInteractive;
    public GameObject objTargetBlock;
    public GameObject objTargetCenterBlock;
    //元素显示
    public GameObject objElementalContainer;
    public GameObject objElementalItemModel;
    protected List<GameObject> listElementalItemObj = new List<GameObject>();

    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    protected Vector3Int lastWorldPosition = Vector3Int.one * int.MaxValue;
    protected bool isShow = true;
    protected bool isShowElemental = false;

    protected float timeUpdateShowElemental = 0;
    protected float timeUpdateShowElementalMax = 1;
    public void Awake()
    {
        //EventHandler.Instance.RegisterEvent<Vector3Int>(EventsInfo.BlockTypeCrucible_UpdateElemental, EventForUpdateElemental);
        objElementalItemModel.ShowObj(false);
        meshFilter = objTargetBlock.GetComponent<MeshFilter>();
        meshRenderer = objTargetBlock.GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        if (isShowElemental)
        {
            timeUpdateShowElemental += Time.deltaTime;
            if (timeUpdateShowElemental > timeUpdateShowElementalMax)
            {
                timeUpdateShowElemental = 0;
                ShowElemental(lastWorldPosition);
            }

            Player player = GameHandler.Instance.manager.player;
            objElementalContainer.transform.LookAt(player.transform);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < listElementalItemObj.Count; i++)
        {
            Destroy(listElementalItemObj[i]);
        }
        listElementalItemObj.Clear();
        //EventHandler.Instance.UnRegisterEvent<Vector3Int>(EventsInfo.BlockTypeCrucible_UpdateElemental, EventForUpdateElemental);
    }

    public void Show(Vector3Int worldPosition, Block block, bool isInteractive)
    {
        if (!isShow)
            return;
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
        //先隐藏所有的元素
        foreach (var itemElementalObj in listElementalItemObj)
        {
            itemElementalObj.ShowObj(false);
        }
        isShowElemental = false;

        //设置方向
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        objTargetCenterBlock.transform.eulerAngles = targetBlock.GetRotateAngles(targetDirection);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //如果和上一个时同一个
        Mesh newMeshData = block.blockShape.GetSelectMeshData(targetChunk, localPosition, targetDirection);
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

        //如果是linkchild 则outline位置位base位置
        if (block.blockType == BlockTypeEnum.LinkChild)
        {
            block.GetBlockMetaData(targetChunk, localPosition, out BlockBean oldBlockData, out BlockMetaBaseLink oldeBlockMetaLinkData);
            objTargetCenterBlock.transform.position = oldeBlockMetaLinkData.GetBasePosition() + new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (block.blockType == BlockTypeEnum.Crucible 
            || block.blockType == BlockTypeEnum.ArcaneAlembic
            || block.blockType == BlockTypeEnum.WardedJar)
        {
            ShowElemental(worldPosition);
            objTargetCenterBlock.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            objTargetCenterBlock.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void ShowElemental(Vector3Int worldPosition)
    {
        isShowElemental = true;

        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);

        //获取这个方块的元素信息 
        List<NumberBean> listElemental = new List<NumberBean>();
        Vector3Int targetLocalPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        if (targetBlock.blockType == BlockTypeEnum.Crucible)
        {
            //如果是坩埚 则需要展示所拥有的元素
            BlockTypeCrucible blockTypeCrucible = targetBlock as BlockTypeCrucible;
            blockTypeCrucible.GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
            listElemental = blockMetaData.listElemental;
        }
        else if (targetBlock.blockType == BlockTypeEnum.ArcaneAlembic)
        {
            //如果是奥术蒸馏器
            BlockTypeArcaneAlembic blockTypeArcaneAlembic = targetBlock as BlockTypeArcaneAlembic;
            blockTypeArcaneAlembic.GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaArcaneAlembic blockMetaData);
            if(blockMetaData.elementalData != null)
                listElemental.Add(blockMetaData.elementalData);
        }
        else if (targetBlock.blockType == BlockTypeEnum.WardedJar)
        {
            //如果是奥术蒸馏器
            BlockTypeWardedJar blockTypeWardedJar = targetBlock as BlockTypeWardedJar;
            blockTypeWardedJar.GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaWardedJar blockMetaData);
            listElemental.Add(new NumberBean(blockMetaData.elementalType, blockMetaData.curElemental));
        }
        if (listElemental.IsNull())
        {
            foreach (var itemElementalObj in listElementalItemObj)
            {
                itemElementalObj.ShowObj(false);
            }
            return;
        }

        int index = 0;
        float startX = 0;
        //如果是双数
        if (listElemental.Count % 2 == 0)
        {
            startX = (listElemental.Count / 2f) - 0.5f;
        }
        //如果是单数
        else
        {
            startX = (listElemental.Count / 2f) - 0.5f;
        }

        foreach (var itemElemental in listElemental)
        {
            if (itemElemental.number == 0)
            {
                continue;
            }
            GameObject objElemental;
            if (index < listElementalItemObj.Count)
            {
                objElemental = listElementalItemObj[index];
            }
            else
            {
                objElemental = Instantiate(objElementalContainer, objElementalItemModel);
                listElementalItemObj.Add(objElemental);
            }
            objElemental.transform.localPosition = new Vector3(startX - index, 0, 0);
            //获取元素信息
            ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData(itemElemental.id);

            SpriteRenderer srIcon = objElemental.GetComponent<SpriteRenderer>();
            TextMeshPro tvNumber = objElemental.transform.Find("ItemElementalNum").GetComponent<TextMeshPro>();

            tvNumber.text = $"{itemElemental.number}";
            IconHandler.Instance.manager.GetUISpriteByName(elementalInfo.icon_key, (iconSP) =>
             {
                 objElemental.ShowObj(true);
                 srIcon.sprite = iconSP;

                 ColorUtility.TryParseHtmlString($"{elementalInfo.color}", out Color ivColor);
                 srIcon.material.SetColor("_Color", ivColor);
             });
            index++;
        }
        //如果没有元素了
        if (index == 0)
        {
            foreach (var itemElementalObj in listElementalItemObj)
            {
                itemElementalObj.ShowObj(false);
            }
            return;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置是否显示
    /// </summary>
    /// <param name="isShow"></param>
    public void SetIsShow(bool isShow)
    {
        this.isShow = isShow;
        if (isShow)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}