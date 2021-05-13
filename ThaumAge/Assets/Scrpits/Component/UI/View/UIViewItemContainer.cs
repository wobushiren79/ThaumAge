using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIViewItemContainer : BaseUIView
{
    public UIViewItem ui_ViewItem;

    protected UIViewItem currentViewItem;

    public override void Awake()
    {
        ui_ViewItem.gameObject.SetActive(false);
    }

    public void SetData(BlockInfoBean blockInfo)
    {
        SetViewItem(blockInfo);
    }

    public void SetViewItem(BlockInfoBean blockInfo)
    {
        if (currentViewItem == null)
        {
            GameObject obj = Instantiate(gameObject, ui_ViewItem.gameObject);
            currentViewItem = obj.GetComponent<UIViewItem>();
            currentViewItem.transform.position = ui_ViewItem.transform.position;
            currentViewItem.transform.localScale = ui_ViewItem.transform.localScale;
            currentViewItem.transform.rotation = ui_ViewItem.transform.rotation;
        }
        currentViewItem.SetData(blockInfo);
    }
}