using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
{
    public Image ui_IVIcon;
    public Text ui_TVNumber;

    public float timeForBackOriginal = 0.2f;//返回原始位置的时间
    public float timeForMove = 0.1f;//移动到指定位置的时间

    public ItemsInfoBean itemsInfo;

    protected UIViewItemContainer originalParent;//原始父级

    protected bool isRaycastLocationValid = true;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetData(ItemsInfoBean itemsInfo)
    {
        this.itemsInfo = itemsInfo;
        SetIcon(itemsInfo.icon_key);
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
            spIcon = IconHandler.Instance.manager.GetItemsSpriteByName("icon_unknow");
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
        GameObject objTarget = eventData.pointerCurrentRaycast.gameObject;
        if (objTarget != null)
        {
            UIViewItemContainer viewItemContainer = objTarget.GetComponent<UIViewItemContainer>();
            //如果是空的格子
            if (viewItemContainer != null)
            {    
                transform.SetParent(viewItemContainer.transform);
                rectTransform
                    .DOAnchorPos(Vector2.zero, timeForMove)
                    .SetEase(Ease.OutBack)
                    .OnComplete(()=> 
                    {
                        isRaycastLocationValid = true;//设置为不能穿透
                     });
  
                return;
            }
            UIViewItem viewItem = objTarget.GetComponent<UIViewItem>();
            //如果不是空的格子
            if (viewItem != null)
            {
                //交换位置
                transform.SetParent(viewItem.transform.parent);
                rectTransform
                    .DOAnchorPos(Vector2.zero, timeForMove)
                    .SetEase(Ease.OutBack)
                    .OnComplete(()=> 
                    {
                        isRaycastLocationValid = true;//设置为不能穿透
                    });
                transform.localScale = Vector3.one;

                viewItem.transform.SetParent(originalParent.transform);
                viewItem.rectTransform.anchoredPosition = Vector2.zero;
                viewItem.transform.localScale = Vector3.one;

                return;
            }
        }
        //返回原来的位置
        transform.SetParent(originalParent.transform);
        rectTransform
            .DOAnchorPos(Vector2.zero, timeForBackOriginal)
            .SetEase(Ease.OutBack)
            .OnComplete(()=> 
            {
                isRaycastLocationValid = true;//设置为不能穿透
            });
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