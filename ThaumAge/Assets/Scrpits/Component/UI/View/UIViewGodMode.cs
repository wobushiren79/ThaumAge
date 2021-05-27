using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIViewGodMode : BaseUIView
{
    public Button ui_BTGodItems;

    public void Start()
    {
        ui_BTGodItems.onClick.AddListener(OnClickForGodItems);    
    }

    public void OnClickForGodItems()
    {
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGodItems>(UIEnum.GodItems);
    }
}