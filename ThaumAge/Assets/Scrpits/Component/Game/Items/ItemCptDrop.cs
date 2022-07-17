using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class ItemCptDrop : BaseMonoBehaviour
{
    protected SpriteRenderer srIcon;
    protected Rigidbody rbItem;
    protected Collider colliderItem;

    public ItemsInfoBean itemsInfo;
    public ItemDropBean itemDropData;

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
        if (itemDropData.itemDrapState == ItemDropStateEnum.Picking)
            return;
        Player player = GameHandler.Instance.manager.player;
        if (player == null) return;

        float dis = Vector3.Distance(player.transform.position, transform.position);
        //如果超过可拾取距离 则变为可拾取
        if (itemDropData.itemDrapState == ItemDropStateEnum.DropNoPick && dis > disForDropNoPick)
        {
            itemDropData.itemDrapState = ItemDropStateEnum.DropPick;
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
        return itemDropData.itemDrapState;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemDropBean itemDropData)
    {
        this.itemDropData = itemDropData;
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemDropData.itemData.itemId);
        transform.position = itemDropData.dropPosition;
        //设置头像
        SetIcon(itemDropData.itemData.itemId);
        //增加一个跳动的力
        //随机方向
        if(itemDropData.dropDirection == Vector3.zero)
        {
            System.Random random = new System.Random();
            rbItem.AddForce(random.Next(-100, 100), random.Next(-100, 100), random.Next(-100, 100));
        }
        //指定方向
        else
        {
            rbItem.AddForce(itemDropData.dropDirection.x * 100, itemDropData.dropDirection.y * 100, itemDropData.dropDirection.z * 100);
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
        itemDropData.itemDrapState = ItemDropStateEnum.Picking;
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
                int number = userData.AddItems(itemDropData.itemData.itemId, itemDropData.itemData.number, itemDropData.itemData.meta);
                if (number == 0)
                {
                    //如果都加完了 则删除
                    Destroy(gameObject);
                }
                else
                {
                    //如果还有剩余
                    itemDropData.itemData.number = number;
                    itemDropData.itemDrapState = ItemDropStateEnum.DropNoPick;
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