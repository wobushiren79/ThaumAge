﻿using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public partial class UIViewItem : BaseUIView, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter
{
    public Image ui_IVIcon;
    public Text ui_TVNumber;

    //返回原始位置的时间
    public float timeForBackOriginal = 0.2f;
    //移动到指定位置的时间
    public float timeForMove = 0.1f;

    public long itemId;
    public int itemNumber;
    public string meta;

    public ItemsInfoBean itemsInfo;

    //原始父级
    public UIViewItemContainer originalParent;

    protected bool isRaycastLocationValid = true;

    protected static bool isAnim = false;
    protected static bool isBeginDrag = false;
    protected InputAction inputActionClick;

    public override void Awake()
    {
        base.Awake();
        inputActionClick = InputHandler.Instance.manager.GetInputUIData(InputActionUIEnum.Click);
    }
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(long itemId, int itemNumber,string meta)
    {
        this.itemId = itemId;
        this.itemNumber = itemNumber;
        this.meta = meta;
        this.itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (itemsInfo != null && itemId != 0)
        {
            SetIcon(itemsInfo.icon_key);
            SetNumber(itemNumber, itemsInfo.max_number);
        }
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number, int maxNumber)
    {
        if (ui_TVNumber == null)
            return;

        if (maxNumber == 1)
        {
            ui_TVNumber.ShowObj(false);
        }
        else
        {
            ui_TVNumber.ShowObj(true);
        }
        //如果是无限 则不显示数量
        //如果上限是1 则不显示数量
        if (number == int.MaxValue || maxNumber == 1)
        {
            ui_TVNumber.ShowObj(false);
        }
        else
        {
            ui_TVNumber.ShowObj(true);
        }
        ui_TVNumber.text = number + "";
        if (number == maxNumber)
        {
            //如果物品已经满了 则高亮显示
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
        Sprite spIcon = IconHandler.Instance.manager.GetItemsSpriteByName(iconKey);
        if (spIcon == null)
        {
            spIcon = IconHandler.Instance.GetUnKnowSprite();
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
        if (inputActionClick.ReadValue<float>() != 1 || isAnim)
        {
            isBeginDrag = false;
            return;
        }
        isBeginDrag = true;
        originalParent = transform.parent.GetComponent<UIViewItemContainer>();
        //如果是无限物品格 则在原位置实例化一个新的
        if (itemNumber == int.MaxValue)
        {
            GameObject objOriginal = Instantiate(originalParent.gameObject, gameObject);
            objOriginal.name = "ViewItem";
            UIViewItem viewItem = objOriginal.GetComponent<UIViewItem>();
            objOriginal.transform.position = gameObject.transform.position;
            originalParent.SetViewItem(viewItem);
            //设置拿出的物体数量为该物体的最大数量
            itemNumber = itemsInfo.max_number;
        }
        else
        {

        }
        RefreshUI();
        isRaycastLocationValid = false;//设置射线忽略自身
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (inputActionClick.ReadValue<float>() != 1 || isAnim || !isBeginDrag)
        {
            return;
        }

        //将拖拽的物体移除原父级
        RectTransform rtfContainer = (RectTransform)UIHandler.Instance.manager.GetUITypeContainer(UITypeEnum.UIBase);
        transform.SetParent(rtfContainer);
        //将生成的物体设为canvas的最后一个子物体，一般来说最后一个子物体是可操作的
        transform.SetAsLastSibling();
        //需要将鼠标的坐标转换成UGUI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtfContainer, eventData.position, CameraHandler.Instance.manager.uiCamera, out Vector2 vecMouse);
        rectTransform.anchoredPosition = vecMouse;
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isAnim || !isBeginDrag)
        {
            return;
        }
        isBeginDrag = false;
        GameObject objTarget = eventData.pointerCurrentRaycast.gameObject;
        if (objTarget != null)
        {
            UIViewItemContainer viewItemContainer = objTarget.GetComponent<UIViewItemContainer>();
            //如果检测到容器 说明只有容器 里面没有道具
            if (viewItemContainer != null)
            {
                UIViewItem tempViewItem = viewItemContainer.GetViewItem();
                if (tempViewItem == null)
                {
                    //如果容器里没有道具 则直接设置容器的道具为当前道具
                    //首先清空原容器里的数据
                    originalParent.ClearViewItem();
                    //设置新的容器
                    viewItemContainer.SetViewItem(this);
                    AnimForPositionChange(timeForMove, null);
                    return;
                }
                else
                {
                    //如果容器里有道具 则返回原容器
                    HandleForBackOriginalContainer();
                    return;
                }
            }
            UIViewItem viewItem = objTarget.GetComponent<UIViewItem>();
            //如果检测到道具 说明容器里有道具
            if (viewItem != null)
            {
                //如果目标是同一类物品
                if (viewItem.itemsInfo.type_id == itemsInfo.type_id)
                {
                    //如果目标是无限物品 则删除现有物品
                    if (viewItem.itemNumber == int.MaxValue)
                    {
                        transform.SetParent(viewItem.transform.parent);
                        transform.localScale = Vector3.one;
                        AnimForPositionChange(timeForMove, () => { DestroyImmediate(gameObject); });
                        return;
                    }
                    //如果目标不是无限物品，则先将目标叠加到最大，自己再返回原位置
                    else
                    {
                        //目标数量叠加 
                        viewItem.itemNumber += itemNumber;
                        if (viewItem.itemNumber > viewItem.itemsInfo.max_number)
                        {
                            //如果目标数量超出最大了
                            itemNumber = viewItem.itemNumber - viewItem.itemsInfo.max_number;
                            viewItem.itemNumber = viewItem.itemsInfo.max_number;
                        }
                        else
                        {
                            //如果没有 则自己的数量为0
                            itemNumber = 0;
                        }
                        //刷新一下UI
                        viewItem.RefreshUI();
                        RefreshUI();

                        if (itemNumber == 0)
                        {
                            //如果自己没有数量了，则删除
                            DestroyImmediate(gameObject);
                            return;
                        }
                        else
                        {
                            //如果自己还有数量，则返回原容器
                            HandleForBackOriginalContainer();
                            return;
                        }
                    }
                }
                //如果目标不是同一类物品
                else
                {
                    //如果目标是无限物品 则回到原来位置
                    if (viewItem.itemNumber == int.MaxValue)
                    {
                        HandleForBackOriginalContainer();
                        return;
                    }
                    //如果是目标不是无限物品，则交换物品位置
                    else
                    {
                        //交换位置
                        UIViewItemContainer dargContainer = this.originalParent;
                        UIViewItemContainer targetContainer = viewItem.originalParent;
                        //交换父级
                        dargContainer.SetViewItem(viewItem);
                        targetContainer.SetViewItem(this);
                        //设置位置
                        transform.localScale = Vector3.one;
                        AnimForPositionChange(timeForMove, () => { });

                        viewItem.rectTransform.anchoredPosition = Vector2.zero;
                        viewItem.transform.localScale = Vector3.one;
                        return;
                    }
                }
            }
            else
            {
                HandleForBackOriginalContainer();
            }
        }
        else
        {
            //如果什么都没有检测到，说明是把物体丢到场景中
            Player player = GameHandler.Instance.manager.player;
            ItemsHandler.Instance.CreateItemDrop(itemId, itemNumber, player.transform.position, ItemDropStateEnum.DropNoPick);

            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// 处理 返回原容器
    /// </summary>
    public void HandleForBackOriginalContainer()
    {
        //其他情况下 则返回原来的位置
        transform.SetParent(originalParent.transform);
        if (originalParent.GetViewItem() != this)
        {
            //如果原来的位置已经被占据 则删除
            AnimForPositionChange(timeForBackOriginal, () => { DestroyImmediate(gameObject); });
        }
        else
        {
            AnimForPositionChange(timeForBackOriginal, () => { });
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
    protected void AnimForPositionChange(float changeTime, Action callBack)
    {
        isAnim = true;
        rectTransform
            .DOAnchorPos(Vector2.zero, changeTime)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                isRaycastLocationValid = true;//设置为不能穿透
                callBack?.Invoke();
                isAnim = false;
            });
    }
}