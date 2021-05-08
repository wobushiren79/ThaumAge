using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerExitHandler,IPointerEnterHandler
{
    protected Transform originalParent;
    protected Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = ((RectTransform)transform).anchoredPosition;
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
        if (eventData.pointerDrag != null)
        {

        }
        else
        {
            transform.SetParent(originalParent);
            ((RectTransform)transform).anchoredPosition = originalPosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}