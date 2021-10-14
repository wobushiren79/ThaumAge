using UnityEditor;
using UnityEngine;

public class ItemDrop : BaseMonoBehaviour
{
    protected SpriteRenderer srIcon;
    protected Rigidbody rbItems;

    public ItemsInfoBean itemsInfo;
    public ItemsBean itemData;

    //掉落状态 0：掉落不可拾取 1：掉落可拾取 2：拾取中
    public ItemDropStateEnum itemDrapState =  ItemDropStateEnum.DropPick;
    //距离玩家的最大距离
    public float disMaxPlayer = 0;

    //删除的时间
    public float timeForItemsDestory = 0;
    //删除的距离
    public float disForItemsDestory = 0;
    //存在时间
    public float timeForCreate = 0;

    public void Awake()
    {
        srIcon = GetComponent<SpriteRenderer>();
        rbItems = GetComponent<Rigidbody>();
    }

    float updateTime = 0;

    public void Update()
    {
        //每隔1秒检测一次
        updateTime += Time.deltaTime;
        if (updateTime > 1)
        {
            updateTime = 0;
            timeForCreate++;
            //如果正在捡取中 则不做处理
            if (itemDrapState == ItemDropStateEnum.Picking)
                return;
            Player player = GameHandler.Instance.manager.player;
            if (player == null) return;

            float dis = Vector3.Distance(player.transform.position, transform.position);

            //如果玩家距离物体过远，或者超过存在时间，则删除物体
            if (dis > disForItemsDestory|| timeForCreate> timeForItemsDestory)
            {
                Destroy(gameObject);
            }
        }
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
        rbItems.AddForce(Random.Range(-100,100), Random.Range(0, 100), Random.Range(-100, 100));

        //初始化数据
        SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
        timeForItemsDestory = gameInitData.timeForItemsDestory;
        disForItemsDestory = gameInitData.disForItemsDestory;
        timeForCreate = 0;

        //设置初始距离玩家距离
        Player player = GameHandler.Instance.manager.player;
        if (player != null) 
        {
            disMaxPlayer = Vector3.Distance(player.transform.position, transform.position);
        }
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
}