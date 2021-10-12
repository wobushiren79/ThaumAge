using UnityEditor;
using UnityEngine;

public class ItemDrop : BaseMonoBehaviour
{
    public SpriteRenderer srIcon;
    public Rigidbody rbItems;

    public ItemsInfoBean itemsInfo;
    public ItemsBean itemData;

    public void Awake()
    {
        srIcon = GetComponent<SpriteRenderer>();
        rbItems = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //如果摄像头距离物体过远，则删除物体
        float dis = Vector3.Distance(CameraHandler.Instance.manager.mainCamera.transform.position, transform.position);
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
        rbItems.AddExplosionForce(10, position, 0, 5);
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