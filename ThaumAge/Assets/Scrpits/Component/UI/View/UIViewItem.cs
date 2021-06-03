using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
{
    public Image ui_IVIcon;
    public Text ui_TVNumber;

    public float timeForBackOriginal = 0.2f;//返回原始位置的时间
    public float timeForMove = 0.1f;//移动到指定位置的时间

    public ItemsBean itemsData;
    public ItemsInfoBean itemsInfo;

    protected UIViewItemContainer originalParent;//原始父级

    protected bool isRaycastLocationValid = true;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
        this.itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemsId);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (itemsInfo != null)
        {
            SetIcon(itemsInfo.icon_key);
        }
        if (itemsData != null)
        {
            SetNumber(itemsData.number);
        }
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number)
    {
        if (ui_TVNumber == null)
            return;
        if (number == byte.MaxValue)
        {
            ui_TVNumber.gameObject.SetActive(false);
        }
        else
        {
            ui_TVNumber.gameObject.SetActive(true);
        }
        ui_TVNumber.text = number + "";
        if (number == itemsInfo.max_number)
        {
            ui_TVNumber.color = Color.red;
        }
        else
        {
            ui_TVNumber.color = Color.white;
        }
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

        //如果是无限物品格 则在原位置实例化一个新的
        if (itemsData.number == byte.MaxValue)
        {
            GameObject objOriginal = Instantiate(originalParent.gameObject, gameObject);
            UIViewItem viewItem = objOriginal.GetComponent<UIViewItem>();
            objOriginal.transform.position = gameObject.transform.position;
            originalParent.SetViewItem(viewItem);
        }
        itemsData.number = (byte)itemsInfo.max_number;
        RefreshUI();
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
                viewItemContainer.SetViewItem(this);
                AnimForPositionChange(rectTransform, timeForMove, () => { });
                return;
            }
            UIViewItem viewItem = objTarget.GetComponent<UIViewItem>();
            //如果目标不是空的格子
            if (viewItem != null)
            {
                //如果目标是同一类物品
                if (viewItem.itemsInfo.GetItemsType() == itemsInfo.GetItemsType())
                {
                    //如果目标是无限物品 则删除现有物品
                    if (viewItem.itemsData.number == byte.MaxValue)
                    {
                        transform.SetParent(viewItem.transform.parent);
                        transform.localScale = Vector3.one;
                        AnimForPositionChange(rectTransform, timeForMove, () => { Destroy(gameObject); });
                        return;
                    }
                    //如果目标不是无限物品，则先将目标叠加到最大，自己再返回原位置
                    else
                    {
                        //目标数量叠加到最大   //自己的数量减小
                        viewItem.itemsData.number += itemsData.number;
                        if (viewItem.itemsData.number > viewItem.itemsInfo.max_number)
                        {
                            viewItem.itemsData.number = (byte)viewItem.itemsInfo.max_number;
                            itemsData.number = (byte)(viewItem.itemsData.number - viewItem.itemsInfo.max_number);
                        }
                        //刷新一下UI
                        viewItem.RefreshUI();
                        RefreshUI();
                        //如果自己没有数量了，则删除
                        if (itemsData.number == byte.MaxValue)
                        {
                            AnimForPositionChange(rectTransform, timeForMove, () => { Destroy(gameObject); });
                            return;
                        }
                    }
                }
                //如果目标不是同一类物品
                else
                {
                    //如果目标是无限物品 则回到原来位置
                    if (viewItem.itemsData.number == byte.MaxValue)
                    {

                    }
                    //如果是目标不是无限物品，则交换物品位置
                    else
                    {
                        //交换位置
                        viewItem.originalParent.SetViewItem(this);
                        transform.localScale = Vector3.one;
                        AnimForPositionChange(rectTransform, timeForMove, () => { });

                        originalParent.SetViewItem(viewItem);
                        viewItem.rectTransform.anchoredPosition = Vector2.zero;
                        viewItem.transform.localScale = Vector3.one;
                        return;
                    }
                }

            }
        }
        //返回原来的位置
        transform.SetParent(originalParent.transform);
        if (originalParent.GetViewItem()!=this)
        {          
            //如果原来的位置已经被占据 则删除
            AnimForPositionChange(rectTransform, timeForBackOriginal, () => { Destroy(gameObject); });
        }
        else
        {
            AnimForPositionChange(rectTransform, timeForBackOriginal, () => { });
        }
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

    /// <summary>
    /// 位置变换动画
    /// </summary>
    /// <param name="target"></param>
    /// <param name="changeTime"></param>
    /// <param name="callBack"></param>
    protected void AnimForPositionChange(RectTransform target,float changeTime, Action callBack)
    {
        target
            .DOAnchorPos(Vector2.zero, changeTime)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                isRaycastLocationValid = true;//设置为不能穿透
                callBack?.Invoke();
            });
    }
}