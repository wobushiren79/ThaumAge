using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIViewItemContainer : BaseUIView
{
    public UIViewItem ui_ViewItem;

    protected UIViewItem currentViewItem;

    public override void Awake()
    {
        base.Awake();
        ui_ViewItem.gameObject.SetActive(false);
    }

    public void SetData(ItemsBean itemsData)
    {
        SetViewItem(itemsData);
    }

    public void SetViewItem(UIViewItem uiView)
    {
        this.currentViewItem = uiView;
        this.currentViewItem.transform.SetParent(rectTransform);
    }

    public void SetViewItem(ItemsBean itemsData)
    {
        //如果没有东西，则删除原来存在的
        if (itemsData == null)
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
            currentViewItem.transform.position = ui_ViewItem.transform.position;
            currentViewItem.transform.localScale = ui_ViewItem.transform.localScale;
            currentViewItem.transform.rotation = ui_ViewItem.transform.rotation;
        }
        currentViewItem.SetData(itemsData);
    }

    public UIViewItem GetViewItem()
    {
        return currentViewItem;
    }
}