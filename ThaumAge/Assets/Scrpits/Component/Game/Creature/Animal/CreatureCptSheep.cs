using UnityEditor;
using UnityEngine;

public class CreatureCptSheep : CreatureCptBaseAnimal
{

    //羊毛位置
    protected GameObject objWoolBody;
    protected GameObject objWoolHead;

    protected CreatureMetaSheep creatureMetaSheep;
    protected float timeForWool = 10;
    protected float timeUpdateForWool =0;

    public override void Awake()
    {
        base.Awake();
        objWoolBody = transform.GetComponentInChildren<Transform,Transform>("WoolBody").gameObject;
        objWoolHead = transform.GetComponentInChildren<Transform, Transform>("WoolHead").gameObject;
    }

    public override void Update()
    {
        base.Update();
        //如果没有毛
        if (creatureMetaSheep.stateWool == 0)
        {
            timeUpdateForWool += Time.deltaTime;
            if (timeUpdateForWool > timeForWool)
            {
                creatureMetaSheep.proWool += 0.01f;
                timeUpdateForWool = 0;
                if (creatureMetaSheep.proWool >= 1)
                {
                    creatureMetaSheep.stateWool = 1;
                    creatureMetaSheep.proWool = 0;
                    SetShowWool(true);
                }
                creatureData.SetMetaData(creatureMetaSheep);
            }
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="creatureInfo"></param>
    public override void SetData(CreatureInfoBean creatureInfo)
    {
        base.SetData(creatureInfo);
        creatureMetaSheep = new CreatureMetaSheep();
        creatureMetaSheep.stateWool = 1;
        creatureMetaSheep.proWool = 0;
        creatureData.SetMetaData(creatureMetaSheep);
    }

    //受到攻击
    public override void UnderAttack(GameObject atkObj, int damage)
    {
        //如果有羊毛
        if (creatureMetaSheep != null && creatureMetaSheep.stateWool == 1)
        {
            //创造羊毛
            CreateOutputItems();
            creatureMetaSheep.stateWool = 0;
            creatureMetaSheep.proWool = 0;
            creatureData.SetMetaData(creatureMetaSheep);
            //模型修改
            SetShowWool(false);

        }
        //如果没有羊毛
        else
        {
            base.UnderAttack(atkObj, damage);
        }
    }

    /// <summary>
    /// 设置展示羊毛
    /// </summary>
    /// <param name="isShowWool"></param>
    public void SetShowWool(bool isShowWool)
    {
        objWoolBody.gameObject.ShowObj(isShowWool);
        objWoolHead.gameObject.ShowObj(isShowWool);
    }
}