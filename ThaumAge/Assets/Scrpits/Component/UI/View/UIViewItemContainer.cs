using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIViewItemContainer : BaseUIView
{
    public Image ui_IVBackground;
    public UIViewItem ui_ViewItem;

    public Vector2Int viewIndex;
    protected UIViewItem currentViewItem;

    public override void Awake()
    {
        base.Awake();
        ui_ViewItem.gameObject.SetActive(false);
    }

    public void SetData(ItemsBean itemsData, Vector2Int viewIndex)
    {
        this.viewIndex = viewIndex;
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
            ui_IVBackground.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            ui_IVBackground.color = Color.white;
        }
    }
}