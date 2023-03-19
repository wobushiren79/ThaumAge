using UnityEditor;
using UnityEngine;

public class CreatureCptBaseGolem : CreatureCptBase
{
    [HideInInspector]
    public AIGolemEntity aiEntity;

    //傀儡的meta数据
    public ItemMetaGolem golemMetaData;

    protected Transform headTF;
    protected SkinnedMeshRenderer headSMR;

    protected Transform bodyTF;
    protected SkinnedMeshRenderer bodySMR;

    protected Transform handTF;
    protected SkinnedMeshRenderer handSMR;

    protected Transform legTF;
    protected SkinnedMeshRenderer legSMR;

    public override void Awake()
    {
        base.Awake();
        aiEntity = gameObject.AddComponentEX<AIGolemEntity>();
        aiEntity.SetData(this);

        headTF = transform.Find("Golem_Body/Golem_Head_1");
        headSMR = headTF.GetComponent<SkinnedMeshRenderer>();

        bodyTF = transform.Find("Golem_Body/Golem_Body_1");
        bodySMR = bodyTF.GetComponent<SkinnedMeshRenderer>();

        handTF = transform.Find("Golem_Body/Golem_Hand_1");
        handSMR = handTF.GetComponent<SkinnedMeshRenderer>();

        legTF = transform.Find("Golem_Body/Golem_Leg_1");
        legSMR = legTF.GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="golemMetaData"></param>
    public void SetData(ItemMetaGolem golemMetaData)
    {
        this.golemMetaData = golemMetaData;
        SetGolemMaterial(golemMetaData.material);
    }

    /// <summary>
    /// 设置傀儡的材质
    /// </summary>
    public void SetGolemMaterial(int material)
    {
        if (material == 0)
            material = 10002;
        var materialData = GolemPressInfoCfg.GetItemData(material);
        Texture2D matTex = LoadAddressablesUtil.LoadAssetSync<Texture2D>(materialData.tex);
        if (matTex != null)
        {
            headSMR.material.mainTexture = matTex;
            bodySMR.material.mainTexture = matTex;
            handSMR.material.mainTexture = matTex;
            legSMR.material.mainTexture = matTex;
        }
    }

    public override void CreateDropItems()
    {
        //回收傀儡
        ItemsBean itemNewGolemData = new ItemsBean(9900001, 1);
        itemNewGolemData.SetMetaData(golemMetaData);
        //创建道具
        ItemDropBean itemDrop = new ItemDropBean(itemNewGolemData, ItemDropStateEnum.DropPick, transform.position + new Vector3(0, 0.5f, 0), Vector3.up);
        ItemsHandler.Instance.CreateItemCptDrop(itemDrop);
    }
}