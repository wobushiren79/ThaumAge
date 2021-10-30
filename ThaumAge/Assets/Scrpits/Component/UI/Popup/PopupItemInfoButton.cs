using UnityEditor;
using UnityEngine;

public class PopupItemInfoButton : PopupButtonView<PopupItemInfo>
{
    public override void Awake()
    {
        base.Awake();
        popupType = PopupEnum.ItemInfo;
    }
}