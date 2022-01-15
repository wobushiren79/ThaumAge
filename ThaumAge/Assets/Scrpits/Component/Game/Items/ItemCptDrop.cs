using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class ItemCptDrop : BaseMonoBehaviour
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

    private void FixedUpdate()
    {
        if (itemDrapState == ItemDropStateEnum.Picking)
        {

        }
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
        if (itemDrapState == ItemDropStateEnum.DropNoPick && dis > disForDropNoPick)
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
    public ItemDropStateEnum GetItemCptDropState()
    {
        return itemDrapState;
    }

    /// <summary>
    /// 设置道具状态
    /// </summary>
    /// <param name="ItemCptDropState"></param>
    public void SetItemDropState(ItemDropStateEnum ItemCptDropState)
    {
        this.itemDrapState = ItemCptDropState;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemData, Vector3 position)
    {
        SetData(itemData, position, Vector3.zero);
    }
    public void SetData(ItemsBean itemData, Vector3 position, Vector3 dropdirection)
    {
        this.itemData = itemData;
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        transform.position = position;
        //设置头像
        SetIcon(itemData.itemId);
        //增加一个跳动的力
        //随机方向
        if(dropdirection == Vector3.zero)
        {
            System.Random random = new System.Random();
            rbItem.AddForce(random.Next(-100, 100), random.Next(-100, 100), random.Next(-100, 100));
        }
        //指定方向
        else
        {
            rbItem.AddForce(dropdirection.x * 100, dropdirection.y * 100, dropdirection.z * 100);
        }
        //初始化数据
        SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
        timeForItemsDestory = gameInitData.timeForItemsDestory;
        disForItemsDestory = gameInitData.disForItemsDestory;
        disForDropNoPick = gameInitData.disForDropNoPick;
        timeForCreate = 0;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="itemId"></param>
    public void SetIcon(long itemId)
    {
        ItemsHandler.Instance.SetItemsIconById(srIcon, itemId);
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
        EnablePhysic(false);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetTF.position + Vector3.up;
        float dis = Vector3.Distance(endPosition, startPosition);
        float timePick = dis / pickSpeed;
        float movePro = 0;
        //飞向玩家
        Tween tween = DOTween
            .To(() => movePro, (data) => movePro = data, 1f, timePick)
            .SetEase(Ease.InQuart)
            .OnUpdate(() =>
            {
                endPosition = targetTF.position + Vector3.up;
                transform.position = Vector3.Lerp(startPosition, endPosition, movePro);
            })
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
                    EnablePhysic(true);
                    //随机散开
                    System.Random random = new System.Random();
                    rbItem.AddForce(random.Next(-100, 100), random.Next(-100, 100), random.Next(-100, 100));
                }
                UIHandler.Instance.RefreshAllUI();
            });
    }

    /// <summary>
    /// 是否开启物理
    /// </summary>
    /// <param name="isEnable"></param>
    public void EnablePhysic(bool isEnable)
    {
        rbItem.isKinematic = !isEnable;
        colliderItem.isTrigger = !isEnable;
        colliderItem.enabled = isEnable;
    }
}