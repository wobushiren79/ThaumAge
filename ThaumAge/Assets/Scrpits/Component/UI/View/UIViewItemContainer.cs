using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class UIViewItemContainer : BaseUIView
{
    //位置
    public Vector2Int viewIndex;
    //道具
    protected UIViewItem currentViewItem;
    //容器指向的数据
    protected ItemsBean itemsData;
    public override void Awake()
    {
        base.Awake();
        ui_ViewItemModel.ShowObj(false);
    }

    public void SetData(ItemsBean itemsData, Vector2Int viewIndex)
    {
        this.itemsData = itemsData;
        this.viewIndex = viewIndex;
        SetViewItem(itemsData);

        //设置暂时信息
        ui_ViewItemContainer.SetItemId(itemsData.itemId);
    }

    /// <summary>
    /// 清空容器
    /// </summary>
    public void ClearViewItem()
    {
        this.currentViewItem = null;
        itemsData.itemId = 0;
        itemsData.number = 0;
        itemsData.meta = null;
    }

    /// <summary>
    /// 获取道具
    /// </summary>
    /// <returns></returns>
    public UIViewItem GetViewItem()
    {
        return currentViewItem;
    }

    /// <summary>
    /// 设置容器道具
    /// </summary>
    /// <param name="uiView"></param>
    public void SetViewItem(UIViewItem uiView)
    {
        this.currentViewItem = uiView;
        this.currentViewItem.originalParent = this;
        this.currentViewItem.transform.SetParent(rectTransform);

        itemsData.itemId = uiView.itemId;
        itemsData.number = uiView.itemNumber;
        itemsData.meta = uiView.meta;
    }

    /// <summary>
    /// 设置容器道具
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetViewItem(ItemsBean itemsData)
    {
        //如果没有东西，则删除原来存在的
        if (itemsData == null || itemsData.itemId == 0)
        {
            if (currentViewItem != null)
            {
                Destroy(currentViewItem.gameObject);
            }
            currentViewItem = null;
            return;
        }
        //如果有东西，则先实例化再设置数据
        if (currentViewItem == null)
        {
            GameObject obj = Instantiate(gameObject, ui_ViewItemModel.gameObject);
            obj.name = "ViewItem";
            currentViewItem = obj.GetComponent<UIViewItem>();
            currentViewItem.originalParent = this;
            currentViewItem.transform.position = ui_ViewItemModel.transform.position;
            currentViewItem.transform.localScale = ui_ViewItemModel.transform.localScale;
            currentViewItem.transform.rotation = ui_ViewItemModel.transform.rotation;
        }
        currentViewItem.SetData(itemsData.itemId, itemsData.number, itemsData.meta);
    }


    /// <summary>
    /// 设置选择状态
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetSelectState(bool isSelect)
    {
        if (ui_IVBackground == null)
            return;
        if (isSelect)
        {
            ui_IVBackground.color = Color.green;
        }
        else
        {
            ui_IVBackground.color = Color.white;
        }
    }
}