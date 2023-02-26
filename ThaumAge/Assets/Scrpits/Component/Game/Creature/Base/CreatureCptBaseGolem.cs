using UnityEditor;
using UnityEngine;

public class CreatureCptBaseGolem : CreatureCptBase
{
    public AIGolemEntity aiEntity;

    //傀儡的meta数据
    public ItemMetaGolem golemMetaData;

    public override void Awake()
    {
        base.Awake();
        aiEntity = gameObject.AddComponentEX<AIGolemEntity>();
        aiEntity.SetData(this);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="golemMetaData"></param>
    public void SetData(ItemMetaGolem golemMetaData)
    {
        this.golemMetaData = golemMetaData;
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