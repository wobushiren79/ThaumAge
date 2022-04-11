using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookShowDetails : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    public override void OpenUI()
    {
        base.OpenUI();
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemChange, EventForMapItemChange);
    }

    /// <summary>
    /// 事件-地图点击改变
    /// </summary>
    /// <param name=""></param>
    public void EventForMapItemChange(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        ui_ViewGameBookShowItemTitle.SetData(bookModelDetailsInfo.title);
        ui_ViewGameBookShowItemContent.SetData(bookModelDetailsInfo.content);
        ui_ViewGameBookShowItemSubmit.SetData();
    }
}