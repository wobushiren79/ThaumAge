using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIViewItemContainer : BaseUIView
{
    public Image ui_IVBackground;
    public UIViewItem ui_ViewItem;

    public Vector2Int viewIndex;
    public ItemsBean itemsData;
    protected UIViewItem currentViewItem;

    public override void Awake()
    {
        base.Awake();
        ui_ViewItem.gameObject.SetActive(false);
    }

    public void SetData(ItemsBean itemsData, Vector2Int viewIndex)
    {
        this.viewIndex = viewIndex;
        this.itemsData = itemsData;
        SetViewItem(itemsData);
    }

    public UIViewItem GetViewItem()
    {
        return currentViewItem;
    }

    public void SetViewItem(UIViewItem uiView)
    {
        this.currentViewItem = uiView;
        this.currentViewItem.originalParent = this;
        this.currentViewItem.transform.SetParent(rectTransform);

        this.itemsData.itemsId = uiView.itemsData.itemsId;
        this.itemsData.number = uiView.itemsData.number;
        this.itemsData.meta = uiView.itemsData.meta;
    }

    public void SetViewItem(ItemsBean itemsData)
    {
        //如果没有东西，则删除原来存在的
        if (itemsData == null || itemsData.itemsId == 0)
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
            GameObject obj = Instantiate(gameObject, ui_ViewItem.gameObject);
            currentViewItem = obj.GetComponent<UIViewItem>();
            currentViewItem.originalParent = this;
            currentViewItem.transform.position = ui_ViewItem.transform.position;
            currentViewItem.transform.localScale = ui_ViewItem.transform.localScale;
            currentViewItem.transform.rotation = ui_ViewItem.transform.rotation;
        }
        currentViewItem.SetData(itemsData);
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