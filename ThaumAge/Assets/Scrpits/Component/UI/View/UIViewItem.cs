using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public partial class UIViewItem : BaseUIView,
    IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter,
    IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{

    //返回原始位置的时间
    protected float timeForBackOriginal = 0.25f;
    //移动到指定位置的时间
    protected float timeForMove = 0.25f;

    //UIViewItem 只做数据记录 实际的itemBean保存在容器里
    public long itemId;
    public int itemNumber;
    public string meta;

    public ItemsInfoBean itemsInfo;

    //原始父级
    public UIViewItemContainer originalParent;

    protected bool isRaycastLocationValid = true;

    //是否正在播放返回动画
    protected static bool isAnim = false;
    //是否开始拖拽
    protected static bool isBeginDrag = false;

    protected InputAction inputActionClick;
    protected InputAction inputActionShiftClick;
    protected InputAction inputActionCtrlClick;

    public override void Awake()
    {
        base.Awake();
        inputActionClick = InputHandler.Instance.manager.GetInputUIData(InputActionUIEnum.Click);
        inputActionShiftClick = InputHandler.Instance.manager.GetInputUIData(InputActionUIEnum.Shift);
        inputActionCtrlClick = InputHandler.Instance.manager.GetInputUIData(InputActionUIEnum.Ctrl);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(long itemId, int itemNumber, string meta)
    {
        this.itemId = itemId;
        this.itemNumber = itemNumber;
        this.meta = meta;
        this.itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        RefreshUI();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
        if (itemsInfo != null && itemId != 0)
        {
            SetIcon(itemsInfo.icon_key, itemsInfo.icon_color);
            SetNumber(itemNumber, itemsInfo.max_number);
            SetLife();
        }
    }

    /// <summary>
    /// 设置耐久
    /// </summary>
    public void SetLife()
    {
        AttributeBean attributeData =  itemsInfo.GetAttributeData();
        int druability = attributeData.GetAttributeValue(AttributeTypeEnum.Durability);
        if (druability == 0)
        {
            //如果没有生命值 则没有耐久
            ui_Life.ShowObj(false);
        }
        else
        {
            if (!meta.IsNull())
            {
                ItemsMetaTool itemsDetails = ItemsBean.GetMetaData<ItemsMetaTool>(meta);
                ui_Life.value = (float)itemsDetails.curDurability / itemsDetails.durability;
                ui_Life.ShowObj(true);
            }
            else
            {
                ui_Life.ShowObj(false);
            }
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
    public void SetIcon(string iconKey, string iconColor)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconKey, (spIcon) =>
        {
             if (spIcon == null)
             {
                 IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                 {
                     if (ui_IVIcon != null)
                     {
                         ui_IVIcon.sprite = spIcon;
                     }
                 });
             }
             if (ui_IVIcon != null)
             {
                 ui_IVIcon.sprite = spIcon;
             }
        });
        if (ui_IVIcon != null) 
        {
            if (!iconColor.IsNull())
            {
                ColorUtility.TryParseHtmlString(iconColor, out Color colorSp);
                ui_IVIcon.color = colorSp;
            }
            else
            {
                ui_IVIcon.color = Color.white;
            }
        }
    }

    /// <summary>
    /// shift点击 快速放入 类型：放入背包和快捷栏中
    /// </summary>
    public bool HandleForShiftClickForBackpackAndShortcuts()
    {
        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        //首先放进快捷栏
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
        if (shortcutsUI != null)
        {
            for (int i = 0; i < shortcutsUI.listShortcut.Count; i++)
            {
                UIViewItemContainer itemContainer = shortcutsUI.listShortcut[i];
                //如果有容器VIEW 并且里面没有东西
                if (itemContainer != null && itemContainer.GetViewItem() == null)
                {
                    ExchangeItemForContainer(itemContainer);
                    return true;
                }
            }
        }
        //如果没有成功再放进背包
        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        if (backpackUI != null)
        {
            if (backpackUI.AddItems(this))
                return true;
        }
        return false;
    }

    /// <summary>
    /// shift点击 快速放入 类型：放入背包和盒子
    /// </summary>
    public bool HandleForShiftClickForBoxAndBackpack()
    {
        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        //首先检测是否有箱子 优先放进箱子
        UIViewBoxList boxList = currentUI.GetComponentInChildren<UIViewBoxList>();
        if (boxList != null)
        {
            if (boxList.AddItems(this)) return true;
        }
        UIViewBackpackList backpackUI = currentUI.GetComponentInChildren<UIViewBackpackList>();
        if (backpackUI != null)
        {
            if (backpackUI.AddItems(this)) return true;
        }
        return false;
    }


    /// <summary>
    /// shift点击 快速放入 类型：放入快捷栏和盒子
    /// </summary>
    public bool HandleForShiftClickForBoxAndShortcuts()
    {
        BaseUIComponent currentUI = UIHandler.Instance.GetOpenUI();
        //首先检测是否有箱子 优先放进箱子
        UIViewBoxList boxList = currentUI.GetComponentInChildren<UIViewBoxList>();
        if (boxList != null)
        {
            if (boxList.AddItems(this)) return true;
        }
        //获取快捷栏
        UIViewShortcuts shortcutsUI = currentUI.GetComponentInChildren<UIViewShortcuts>();
        if (shortcutsUI != null)
        {
            for (int i = 0; i < shortcutsUI.listShortcut.Count; i++)
            {
                UIViewItemContainer itemContainer = shortcutsUI.listShortcut[i];
                //如果有容器VIEW 并且里面没有东西
                if (itemContainer != null && itemContainer.GetViewItem() == null)
                {
                    //如果是上帝模式则需要在原位置复制一个
                    if (originalParent.containerType == UIViewItemContainer.ContainerType.God)
                    {
                        CopyItemInOriginal(int.MaxValue);
                    }
                    ExchangeItemForContainer(itemContainer);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        float isShiftClick = inputActionShiftClick.ReadValue<float>();
        //LogUtil.Log($"OnPointerClick dragging:{eventData.dragging} pointerDrag:{eventData.pointerDrag.name} eligibleForClick:{eventData.eligibleForClick} isFastClick:{isFastClick}");
        //如果是快速选择
        if (isShiftClick == 1)
        {
            switch (originalParent.containerType)
            {
                //如果是快捷栏
                case UIViewItemContainer.ContainerType.Shortcuts:
                    HandleForShiftClickForBoxAndBackpack();
                    break;
                //如果是背包或者上帝模式
                case UIViewItemContainer.ContainerType.Backpack:
                case UIViewItemContainer.ContainerType.God:
                    HandleForShiftClickForBoxAndShortcuts();
                    break;
                //如果是箱子里的东西
                case UIViewItemContainer.ContainerType.Box:
                case UIViewItemContainer.ContainerType.Furnaces:
                    HandleForShiftClickForBackpackAndShortcuts();
                    break;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //LogUtil.Log($"OnPointerUp dragging:{eventData.dragging} pointerDrag:{eventData.pointerDrag.name} eligibleForClick:{eventData.eligibleForClick}");
        //如果是右键点击
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(originalParent.containerType== UIViewItemContainer.ContainerType.Shortcuts
                || originalParent.containerType == UIViewItemContainer.ContainerType.Backpack)
            {
                //打开道具操作弹窗
                DialogBean dialogData = new DialogBean();
                dialogData.dialogType = DialogEnum.ItemOptions;
                UIDialogItemOptions dialog = UIHandler.Instance.ShowDialog<UIDialogItemOptions>(dialogData);
                dialog.SetData(this);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioHandler.Instance.PlaySound(1);
        //LogUtil.Log($"OnPointerDown dragging:{eventData.dragging} pointerDrag:{eventData.pointerDrag.name} eligibleForClick:{eventData.eligibleForClick}");
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
        UIHandler.Instance.manager.CanClickUIButtons = false;
        UIHandler.Instance.manager.CanInputActionStarted = false;

        float isCtrlClick = inputActionCtrlClick.ReadValue<float>();
        isBeginDrag = true;
        originalParent = transform.parent.GetComponent<UIViewItemContainer>();
        //如果是无限物品格 则在原位置实例化一个新的
        if (itemNumber == int.MaxValue)
        {
            CopyItemInOriginal(int.MaxValue);
        }
        //如果是ctrl点击 则只拿一半
        else if (isCtrlClick == 1)
        {
            if (itemNumber >= 2)
            {
                int halfNumber = itemNumber / 2;
                CopyItemInOriginal(halfNumber, itemNumber - halfNumber);
            }
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtfContainer, eventData.position, null, out Vector2 vecMouse);
        rectTransform.anchoredPosition = vecMouse;
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        UIHandler.Instance.manager.CanClickUIButtons = true;
        UIHandler.Instance.manager.CanInputActionStarted = true;

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
                ExchangeItemForContainer(viewItemContainer);
                return;
            }
            UIViewItem viewItem = objTarget.GetComponent<UIViewItem>();
            //如果检测到道具 说明容器里有道具
            if (viewItem != null)
            {
                ExchangeItemForItem(viewItem);
                return;
            }
            else
            {
                HandleForBackOriginalContainer();
                return;
            }
        }
        else
        {
            //如果什么都没有检测到，说明是把物体丢到场景中
            DropItem();
            return;
        }
    }

    /// <summary>
    /// 丢掉物品
    /// </summary>
    public void DropItem()
    {
        //如果什么都没有检测到，说明是把物体丢到场景中
        Player player = GameHandler.Instance.manager.player;
        Vector3 randomFroce = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
        ItemDropBean itemDropData = new ItemDropBean
            (
            itemId,
            player.transform.position + Vector3.up,
            player.transform.forward + randomFroce,
            itemNumber,
            meta,
            ItemDropStateEnum.DropNoPick
            );
        ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
        DestroyImmediate(gameObject);
        if (originalParent != null)
        {
            //如果原容器的是本道具
            if (originalParent.GetViewItem() == this)
            {
                originalParent.ClearItemsData();
            }
            //如果不是本道具 说明是拆分出来的 拆分的道具丢弃
            else
            {

            }
        }
    }

    /// <summary>
    /// 处理 返回原容器
    /// </summary>
    public void HandleForBackOriginalContainer()
    {
        //其他情况下 则返回原来的位置
        transform.SetParent(originalParent.transform);
        UIViewItem originViewItem = originalParent.GetViewItem();
        if (originViewItem != this)
        {
            //如果原来的位置已经被占据 
            //1.从god模式拿取道具 在原来的地方生成了1个新的
            //2.拆分了一半的道具 在原来的地方有另外一半的道具
            if (originViewItem.itemNumber == int.MaxValue)
            {
                //1
            }
            else
            {
                //2
                originalParent.itemsData.number += itemNumber;
                originViewItem.itemNumber = originalParent.itemsData.number;
            }

            AnimForPositionChange(timeForBackOriginal, () => 
            {
                originViewItem.RefreshUI();
                //删除拖拽的UI
                DestroyImmediate(gameObject);
            });
        }
        else
        {
            AnimForPositionChange(timeForBackOriginal, () => 
            {

            });
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
        rectTransform.DOKill();
        rectTransform
            .DOAnchorPos(Vector2.zero, changeTime)
            .SetEase(Ease.OutCubic)
            .OnStart(() =>
            {
                ui_IVIcon.maskable = false;//设置最层级显示
            })
            .OnComplete(() =>
            {
                ui_IVIcon.maskable = true;//设置最层级显示
                isRaycastLocationValid = true;//设置为不能穿透
                callBack?.Invoke();
                isAnim = false;
            });
    }

    /// <summary>
    /// 在原位置复制一个道具
    /// </summary>
    public void CopyItemInOriginal(int copyItemNumber = -1, int curNumber = -1)
    {
        if (copyItemNumber == -1)
        {
            copyItemNumber = itemsInfo.max_number;
        }
        if (curNumber == -1)
        {
            curNumber = itemsInfo.max_number;
        }
        GameObject objOriginal = Instantiate(originalParent.gameObject, gameObject);
        objOriginal.name = "ViewItem";
        UIViewItem viewItem = objOriginal.GetComponent<UIViewItem>();
        viewItem.itemNumber = copyItemNumber;
        objOriginal.transform.position = gameObject.transform.position;
        originalParent.SetViewItem(viewItem);
        originalParent.itemsData.number = copyItemNumber;
        viewItem.RefreshUI();

        //设置拿出的物体数量为该物体的最大数量
        itemNumber = curNumber;
        //刷新一下UI
        RefreshUI();
    }

    /// <summary>
    /// 交换道具-容器
    /// </summary>
    /// <param name="viewItemContainer"></param>
    public bool ExchangeItemForContainer(UIViewItemContainer viewItemContainer)
    {
        UIViewItem tempViewItem = viewItemContainer.GetViewItem();
        if (tempViewItem == null)
        {
            //检测容器是否能放置当前物品
            bool canSetItem = viewItemContainer.CheckCanSetItem(itemsInfo.GetItemsType());
            //如果能放置
            if (canSetItem)
            {
                //如果容器里没有道具 则直接设置容器的道具为当前道具
                UIViewItem originViewItem = originalParent.GetViewItem();
                if (originViewItem != this)
                {
                    //首先判断一下原容器里面是不是创建了新物体

                }
                else
                {
                    //如果原容器的物体就是拖拽的物体 首先清空原容器里的数据
                    originalParent.ClearViewItem();
                }
                //设置新的容器
                viewItemContainer.SetViewItem(this);
                AnimForPositionChange(timeForMove, null);
            }
            else
            {
                //如果没有设置成功（不能放置该类型），则返回原容器
                HandleForBackOriginalContainer();
            }
            return true;
        }
        else
        {
            //如果容器里有道具 则返回原容器
            HandleForBackOriginalContainer();
            return true;
        }
    }

    /// <summary>
    /// 道具和道具交换
    /// </summary>
    /// <param name="viewItem"></param>
    /// <returns></returns>
    protected void ExchangeItemForItem(UIViewItem viewItem)
    {
        //如果目标是同一物品
        if (viewItem.itemsInfo.id == itemsInfo.id)
        {
            //如果目标是无限物品 则删除现有物品
            if (viewItem.itemNumber == int.MaxValue)
            {
                transform.SetParent(viewItem.transform.parent);
                transform.localScale = Vector3.one;
                AnimForPositionChange(timeForMove, () => 
                {
                    DestroyImmediate(gameObject); 
                });
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
                    viewItem.originalParent.itemsData.number = viewItem.itemNumber;
                }
                else
                {
                    //如果没有 则自己的数量为0
                    itemNumber = 0;
                }
                viewItem.originalParent.itemsData.number = viewItem.itemNumber;
                //刷新一下UI
                viewItem.RefreshUI();
                RefreshUI();

                if (itemNumber == 0)
                {
                    //如果自己没有数量了，则删除
                    DestroyImmediate(gameObject);
                    UIViewItem originViewItem = originalParent.GetViewItem();
                    //如果是自己 还要清除数据
                    if (originViewItem == this)
                    {
                        originalParent.ClearViewItem();
                    }
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
                //检测容器是否能放置当前物品
                bool canSetItem = viewItem.originalParent.CheckCanSetItem(itemsInfo.GetItemsType());
                if (canSetItem)
                {
                    UIViewItem originViewItem = originalParent.GetViewItem();
                    if (originViewItem != this)
                    {
                        //首先判断一下原容器里面是不是创建了新物体 如果创建了则返回
                        HandleForBackOriginalContainer();
                    }
                    else
                    {
                        //如果没有创建 则直接交换位置
                        UIViewItemContainer dargContainer = this.originalParent;
                        UIViewItemContainer targetContainer = viewItem.originalParent;
                        //交换父级
                        if (dargContainer.GetViewItem() != null && dargContainer.GetViewItem().itemNumber == int.MaxValue)
                        {
                            //如果原父级有东西 则把目标容器里的物品丢出来
                            viewItem.DropItem();
                        }
                        else
                        {
                            //如果原父级没有东西 则交换父级
                            dargContainer.SetViewItem(viewItem);
                            //设置位置
                            viewItem.rectTransform.anchoredPosition = Vector2.zero;
                            viewItem.transform.localScale = Vector3.one;
                        }
                        targetContainer.SetViewItem(this);
                        //设置位置
                        transform.localScale = Vector3.one;
                        AnimForPositionChange(timeForMove, () => 
                        {
                        
                        });
                    }
                    return;
                }
                else
                {
                    //如果不能设置该物品（容器不能装该类型） 则返回
                    HandleForBackOriginalContainer();
                    return;
                }
            }
        }
    }
}