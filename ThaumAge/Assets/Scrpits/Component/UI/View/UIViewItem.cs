using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
{
    protected Transform originalParent;
    protected Vector3 originalPosition;

    protected bool isRaycastLocationValid = true;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = ((RectTransform)transform).anchoredPosition;
        isRaycastLocationValid = false;//设置射线忽略自身
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rtfContainer = UIHandler.Instance.manager.GetContainer();
        transform.SetParent(rtfContainer);
        //将生成的物体设为canvas的最后一个子物体，一般来说最后一个子物体是可操作的
        transform.SetAsLastSibling();
        //需要将鼠标的坐标转换成UGUI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtfContainer, eventData.position, Camera.main, out Vector2 vecMouse);
        ((RectTransform)transform).anchoredPosition = vecMouse;
    }

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
                ((RectTransform)transform).anchoredPosition = viewShortcuts.transform.position;
                isRaycastLocationValid = true;//设置为不能穿透
                return;
            }
            UIViewItem viewItem = objDrop.GetComponent<UIViewItem>();
            //如果不是空的格子
            if (viewItem != null)
            {
                //交换位置
                transform.SetParent(viewItem.transform.parent);
                ((RectTransform)transform).anchoredPosition = Vector2.zero;
                transform.localScale = Vector3.one;
                transform.eulerAngles = Vector3.one;

                viewItem.transform.SetParent(originalParent);
                ((RectTransform)viewItem.transform).anchoredPosition = Vector2.zero;
                viewItem.transform.localScale = Vector3.one;
                viewItem.transform.eulerAngles = Vector3.one;

                isRaycastLocationValid = true;//设置为不能穿透
                return;
            }
        }
        transform.SetParent(originalParent);
        ((RectTransform)transform).anchoredPosition = originalPosition;
        isRaycastLocationValid = true;//设置为不能穿透
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycastLocationValid;
    }
}