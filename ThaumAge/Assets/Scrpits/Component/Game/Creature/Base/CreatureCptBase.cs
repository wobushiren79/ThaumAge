using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureCptBase : BaseMonoBehaviour
{
    //生物基础动画
    [HideInInspector]
    public CreatureAnim creatureAnim;
    //生物基础战斗
    [HideInInspector]
    public CreatureBattle creatureBattle;
    //生物碰撞和触发
    [HideInInspector]
    public CreatureCollisionAndTrigger creatureCollisionAndTrigger;

    //生物信息
    [HideInInspector]
    public CreatureInfoBean creatureInfo;
    //生物数据
    [HideInInspector]
    public CreatureBean creatureData;

    //动画-生物
    protected Animator animCreature;
    //检测-生物
    protected Collider colliderCreature;

    protected float timeUpdateForCollisionAndTrigger = 0;
    protected float timeUpdateMaxForCollisionAndTrigger = 0.1f;
    public virtual void Awake()
    {
        animCreature = GetComponentInChildren<Animator>();
        colliderCreature = transform.GetComponent<Collider>();

        creatureAnim = new CreatureAnim(this, animCreature);
        creatureBattle = new CreatureBattle(this);
        creatureCollisionAndTrigger = new CreatureCollisionAndTrigger(this);
    }

    public virtual void Update()
    {
        timeUpdateForCollisionAndTrigger += Time.deltaTime;
        if (timeUpdateForCollisionAndTrigger >= timeUpdateMaxForCollisionAndTrigger)
        {
            creatureCollisionAndTrigger.UpdateCollisionAndTrigger();
            timeUpdateForCollisionAndTrigger = 0;
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public virtual void SetData(CreatureInfoBean creatureInfo)
    {
        this.creatureInfo = creatureInfo;
        creatureData = new CreatureBean();
        CreatureStatusBean creatureStatus = creatureData.GetCreatureStatus();
        creatureStatus.health = creatureInfo.life;
        creatureStatus.curHealth = creatureInfo.life;
        creatureData.creatureType = creatureInfo.creature_type;
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="atkObj"></param>
    /// <param name="damage"></param>
    public virtual void UnderAttack(GameObject atkObj, DamageBean damageData)
    {
        creatureBattle.UnderAttack(atkObj, damageData);
    }


    protected float maxFallHeight = 5;
    /// <summary>
    /// 受到掉落伤害
    /// </summary>
    public virtual void UnderFall(float jumpStartPositionY)
    {
        //获取坠落高度
        float fallHeight = jumpStartPositionY - transform.position.y;
        if (fallHeight > maxFallHeight)
        {
            float heightAdd = fallHeight - maxFallHeight;
            //按比例减少
            CreatureStatusBean creatureStatus = creatureData.GetCreatureStatus();
            int maxHealth = creatureStatus.health;
            //扣除伤害
            int damage = (int)(maxHealth * 0.1f * heightAdd);
            UnderAttack(null, new DamageBean(damage));
            //播放音效
            AudioHandler.Instance.PlaySound(501);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Dead()
    {
        //展示死亡特效
        EffectBean effectDataForBody = new EffectBean();
        effectDataForBody.effectName = EffectInfo.Effect_DeadBody_1;
        effectDataForBody.effectType = EffectTypeEnum.Normal;
        effectDataForBody.timeForShow = 5f;
        effectDataForBody.effectPosition = transform.position + new Vector3(0,0.3f,0);

        SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        Mesh bodyMesh = skinnedMeshRenderer.sharedMesh;
        Texture2D bodyTex = (Texture2D)skinnedMeshRenderer.material.mainTexture;
        EffectHandler.Instance.ShowEffect(effectDataForBody, (effect) =>
        {
            EffectDeadBody deadBody = effect as EffectDeadBody;
            deadBody.SetData(skinnedMeshRenderer.sharedMesh, bodyTex);

            //删除此物体
            Destroy(gameObject);
        });

        //查询身体位置
        Transform bodyTF = CptUtil.GetCptInChildrenByName<Transform>(gameObject, "BoneBody");
        //消失烟雾
        EffectBean effectDataForSmoke = new EffectBean();
        effectDataForSmoke.effectName = EffectInfo.Effect_Dead_1;
        effectDataForSmoke.effectType = EffectTypeEnum.Visual;
        effectDataForSmoke.timeForShow = 5;
        effectDataForSmoke.effectPosition = bodyTF.position;
        EffectHandler.Instance.ShowEffect(effectDataForSmoke);

        ////身体侧翻
        //Vector3 randomRotate;
        //int random = WorldRandTools.Range(0, 4);
        //switch (random)
        //{
        //    case 0:
        //        randomRotate = new Vector3(90, 0, 0);
        //        break;
        //    case 1:
        //        randomRotate = new Vector3(-90, 0, 0);
        //        break;
        //    case 2:
        //        randomRotate = new Vector3(0, 0, 90);
        //        break;
        //    case 3:
        //        randomRotate = new Vector3(0, 0, -90);
        //        break;
        //    default:
        //        randomRotate = new Vector3(-90, 0, 0);
        //        break;
        //}
        //transform
        //    .DOLocalRotate(randomRotate, 0.5f, RotateMode.Fast)
        //    .OnComplete(() =>
        //    {
        //        //查询身体位置
        //        Transform bodyTF = CptUtil.GetCptInChildrenByName<Transform>(gameObject, "BoneBody");
        //        //消失烟雾
        //        EffectBean effectData = new EffectBean();
        //        effectData.effectName = EffectInfo.Effect_Dead_1;
        //        effectData.effectType = EffectTypeEnum.Visual;
        //        effectData.timeForShow = 5;
        //        effectData.effectPosition = bodyTF.position;
        //        EffectHandler.Instance.ShowEffect(effectData, (effect) => { effect.PlayEffect(); });
        //        //删除此物体
        //        Destroy(gameObject);
        //    });


        //关闭动画
        creatureAnim.EnabledAnim(false);
        //关闭检测
        if (colliderCreature != null)
            colliderCreature.enabled = false;
        //创建掉落物
        CreateDropItems();
    }

    /// <summary>
    /// 创建掉落物
    /// </summary>
    public void CreateDropItems()
    {
        //创建道具
        List<ItemsBean> listDropItems = ItemsHandler.Instance.GetItemsDrop(creatureInfo.drop);
        ItemsHandler.Instance.CreateItemCptDropList(listDropItems, ItemDropStateEnum.DropPick, transform.position + Vector3.up * 0.5f);
    }

    /// <summary>
    /// 创建产出
    /// </summary>
    public void CreateOutputItems()
    {
        List<ItemsBean> listDropItems = ItemsHandler.Instance.GetItemsDrop(creatureInfo.output_res);
        ItemsHandler.Instance.CreateItemCptDropList(listDropItems, ItemDropStateEnum.DropPick, transform.position + Vector3.up * 0.5f);
    }
}