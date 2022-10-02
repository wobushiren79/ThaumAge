using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookShowDetails : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    public override void OpenUI()
    {
        base.OpenUI();
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemChange, EventForMapItemChange);
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemRefresh, EventForMapItemChange);
        RegisterEvent(EventsInfo.UIGameBook_MapItemClean, EventForMapItemClean);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Null.text = TextHandler.Instance.GetTextById(10);
    }

    /// <summary>
    /// 事件-地图点击改变
    /// </summary>
    /// <param name=""></param>
    public void EventForMapItemChange(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        ui_ListContentDetails.ShowObj(true);
        ui_Null.ShowObj(false);
        ui_ViewGameBookShowItemTitle.SetData(bookModelDetailsInfo.title);
        ui_ViewGameBookShowItemContent.SetData(bookModelDetailsInfo.content);
        ui_ViewGameBookShowItemSubmit.SetData(bookModelDetailsInfo);
    }

    /// <summary>
    /// 事件-清空
    /// </summary>
    public void EventForMapItemClean()
    {
        ui_Null.ShowObj(true);
        ui_ListContentDetails.ShowObj(false);
    }
}