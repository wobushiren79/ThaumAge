using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class ItemDrop : BaseMonoBehaviour
{
    protected SpriteRenderer srIcon;
    protected Rigidbody rbItem;
    protected Collider colliderItem;

    public ItemsInfoBean itemsInfo;
    public ItemsBean itemData;

    //掉落状态 0：掉落不可拾取 1：掉落可拾取 2：拾取中
    public ItemDropStateEnum itemDrapState = ItemDropStateEnum.DropPick;

    //删除的时间
    public float timeForItemsDestory = 0;
    //删除的距离
    public float disForItemsDestory = 0;
    //不可拾取的距离
    public float disForDropNoPick = 0;
    //存在时间
    public float timeForCreate = 0;

    public void Awake()
    {
        srIcon = GetComponent<SpriteRenderer>();
        rbItem = GetComponent<Rigidbody>();
        colliderItem = GetComponent<Collider>();
        InvokeRepeating("UpdateItemData", 1, 1);
    }

    public void OnDestroy()
    {
        CancelInvoke();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void UpdateItemData()
    {
        //每隔1秒检测一次
        timeForCreate++;
        //如果正在捡取中 则不做处理
        if (itemDrapState == ItemDropStateEnum.Picking)
            return;
        Player player = GameHandler.Instance.manager.player;
        if (player == null) return;

        float dis = Vector3.Distance(player.transform.position, transform.position);
        //如果超过可拾取距离 则变为可拾取
        if(itemDrapState == ItemDropStateEnum.DropNoPick && dis > disForDropNoPick)
        {
            SetItemDropState(ItemDropStateEnum.DropPick);
        }
        //如果玩家距离物体过远，或者超过存在时间，则删除物体
        if (dis > disForItemsDestory || timeForCreate > timeForItemsDestory)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 获取道具状态
    /// </summary>
    /// <returns></returns>
    public ItemDropStateEnum GetItemDropState()
    {
        return itemDrapState;
    }

    /// <summary>
    /// 设置道具状态
    /// </summary>
    /// <param name="itemDropState"></param>
    public void SetItemDropState(ItemDropStateEnum itemDropState)
    {
        this.itemDrapState = itemDropState;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemData, Vector3 position)
    {
        this.itemData = itemData;
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        transform.position = position;
        //设置头像
        SetIcon(itemsInfo.icon_key);
        //增加一个跳动的力
        rbItem.AddForce(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100));

        //初始化数据
        SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
        timeForItemsDestory = gameInitData.timeForItemsDestory;
        disForItemsDestory = gameInitData.disForItemsDestory;
        disForDropNoPick = gameInitData.disForItemsDestory;
        timeForCreate = 0;

    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="itemId"></param>
    public void SetIcon(long itemId)
    {
        Sprite spIcon = ItemsHandler.Instance.manager.GetItemsIconById(itemId);
        if (spIcon == null)
        {
            spIcon = IconHandler.Instance.GetUnKnowSprite();
            SetIcon(spIcon);
        }
        else
        {
            SetIcon(spIcon);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(string iconKey)
    {
        Sprite spIcon = IconHandler.Instance.manager.GetItemsSpriteByName(iconKey);
        if (spIcon == null)
        {
            spIcon = IconHandler.Instance.GetUnKnowSprite();
            SetIcon(spIcon);
        }
        else
        {
            SetIcon(spIcon);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="sprite"></param>
    public void SetIcon(Sprite sprite)
    {
        srIcon.sprite = sprite;
    }

    /// <summary>
    /// 捡起道具
    /// </summary>
    /// <param name="targetTF"></param>
    /// <param name="pickSpeed">捡道具速度</param>
    public void PickItem(Transform targetTF, float pickSpeed)
    {
        //修改状态
        SetItemDropState(ItemDropStateEnum.Picking);
        //关闭碰撞
        rbItem.isKinematic = false;
        colliderItem.isTrigger = true;
        colliderItem.enabled = false;
        float dis = Vector3.Distance(targetTF.position, transform.position);
        //飞向玩家
        transform
            .DOMove(targetTF.position, dis / pickSpeed)
            .OnComplete(() =>
            {
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                int number = userData.AddItems(itemData.itemId, itemData.number);
                if (number == 0)
                {
                    //如果都加完了 则删除
                    Destroy(gameObject);
                }
                else
                {
                    //如果还有剩余
                    itemData.number = number;
                    SetItemDropState(ItemDropStateEnum.DropNoPick);
                }
            });
    }
}