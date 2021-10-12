using UnityEditor;
using UnityEngine;

public class ItemDrop : BaseMonoBehaviour
{
    protected SpriteRenderer srIcon;
    protected Rigidbody rbItems;

    public ItemsInfoBean itemsInfo;
    public ItemsBean itemData;

    //掉落状态 0：掉落不可拾取 1：掉落可拾取 2：拾取中
    public int itemDrapState = 0;

    public void Awake()
    {
        srIcon = GetComponent<SpriteRenderer>();
        rbItems = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //如果摄像头距离物体过远，则删除物体
        Player player = GameHandler.Instance.manager.player;
        if (player == null) return;

        float dis = Vector3.Distance(player.transform.position, transform.position);
        SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
        if (dis > gameInitData.disForItemsDestory)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemData, Vector3 position)
    {
        this.itemData = itemData;
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        transform.position = position;
        SetIcon(itemsInfo.icon_key);
        rbItems.AddForce(Random.Range(-100,100), Random.Range(0, 100), Random.Range(-100, 100));
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