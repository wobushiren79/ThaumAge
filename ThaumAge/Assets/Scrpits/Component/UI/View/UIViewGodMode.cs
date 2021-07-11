using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIViewGodMode : BaseUIView
{
    public Button ui_BTGodItems;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BTGodItems)
        {
            UIHandler.Instance.manager.OpenUIAndCloseOther<UIGodItems>(UIEnum.GodItems);
        }
    }
}