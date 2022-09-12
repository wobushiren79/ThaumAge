using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class UIDialogItemOptions : DialogView,
    IPointerUpHandler, IPointerClickHandler, IPointerDownHandler
{
    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;
    public Vector2 offsetPivot = Vector2.zero;

    protected UIViewItem uiViewItem;
    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(UIViewItem uiViewItem)
    {
        this.uiViewItem = uiViewItem;
        //屏幕坐标转换为UI坐标
        Vector2 outPosition = GameUtil.MousePointToUGUIPoint(null, rectTransform);
        ui_DialogContent.localPosition = new Vector3(outPosition.x + offsetX, outPosition.y + offsetY, ui_DialogContent.localPosition.z);

        float offsetTotalX;
        float offsetTotalY;
        //判断鼠标在屏幕的左右
        if (Input.mousePosition.x <= (Screen.width / 2))
        {
            //左
            offsetTotalX = 0 - offsetPivot.x;
        }
        else
        {
            //右
            offsetTotalX = 1 + offsetPivot.x;
        }

        //屏幕上下修正
        if (Input.mousePosition.y <= (Screen.height / 2))
        {
            offsetTotalY = 0 + offsetPivot.y;
        }
        else
        {
            offsetTotalY = 1 + offsetPivot.y;
        }
        ui_DialogContent.pivot = new Vector2(offsetTotalX, offsetTotalY);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Drop)
        {
            HandleForDropItem();
        }
        else if (viewButton == ui_Split)
        {
            HandleForSplit();
        }
        AudioHandler.Instance.PlaySound(1);
        CancelOnClick();
    }

    /// <summary>
    /// 处理丢弃
    /// </summary>
    public void HandleForDropItem()
    {
        uiViewItem.DropItem();
    }

    /// <summary>
    /// 处理拆分
    /// </summary>
    public void HandleForSplit()
    {
        //首先创建一个原本的一半复制
        if (uiViewItem.itemNumber >= 2)
        {
            int halfNumber = uiViewItem.itemNumber / 2;
            uiViewItem.CopyItemInOriginal(halfNumber, uiViewItem.itemNumber - halfNumber);

            //把这个道具快速放入背包或者道具栏
            bool isExchange = uiViewItem.HandleForShiftClickForBackpackAndShortcuts();
            //如果没有放入 则丢弃这个道具
            if (!isExchange)
                uiViewItem.DropItem();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CancelOnClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            CancelOnClick();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}