using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
{
    public Image ui_IVIcon;
    public Text ui_TVNumber;

    protected UIViewItemContainer originalParent;
    protected Vector3 originalPosition;

    protected bool isRaycastLocationValid = true;

    public BlockInfoBean blockInfo;
    

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(BlockInfoBean blockInfo)
    {
        this.blockInfo = blockInfo;
        SetIcon(blockInfo.icon_key);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(string iconKey)
    {
        Sprite spIcon= IconHandler.Instance.manager.GetItemsSpriteByName(iconKey);
        if (spIcon == null)
        {
            spIcon = IconHandler.Instance.manager.GetItemsSpriteByName("item_test");
        }
        if (ui_IVIcon != null)
        {
            ui_IVIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent.GetComponent<UIViewItemContainer>();
        originalPosition = rectTransform.anchoredPosition;
        isRaycastLocationValid = false;//设置射线忽略自身
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rtfContainer = UIHandler.Instance.manager.GetContainer();
        transform.SetParent(rtfContainer);
        //将生成的物体设为canvas的最后一个子物体，一般来说最后一个子物体是可操作的
        transform.SetAsLastSibling();
        //需要将鼠标的坐标转换成UGUI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtfContainer, eventData.position, Camera.main, out Vector2 vecMouse);
        rectTransform.anchoredPosition = vecMouse;
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject objDrop = eventData.pointerCurrentRaycast.gameObject;
        if (objDrop != null)
        {
            UIViewShortcuts viewShortcuts = objDrop.GetComponent<UIViewShortcuts>();
            //如果是空的格子
            if (viewShortcuts != null)
            {
                transform.SetParent(viewShortcuts.transform);
                rectTransform.anchoredPosition = viewShortcuts.transform.position;
                isRaycastLocationValid = true;//设置为不能穿透
                return;
            }
            UIViewItem viewItem = objDrop.GetComponent<UIViewItem>();
            //如果不是空的格子
            if (viewItem != null)
            {
                //交换位置
                transform.SetParent(viewItem.transform.parent);
                rectTransform.anchoredPosition = Vector2.zero;
                transform.localScale = Vector3.one;
                transform.eulerAngles = Vector3.one;

                viewItem.transform.SetParent(originalParent.transform);
                rectTransform.anchoredPosition = Vector2.zero;
                viewItem.transform.localScale = Vector3.one;
                viewItem.transform.eulerAngles = Vector3.one;

                isRaycastLocationValid = true;//设置为不能穿透
                return;
            }
        }
        transform.SetParent(originalParent.transform);
        rectTransform.anchoredPosition = originalPosition;
        isRaycastLocationValid = true;//设置为不能穿透
    }

    /// <summary>
    /// 是否忽略本身的射线检测
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycastLocationValid;
    }
}