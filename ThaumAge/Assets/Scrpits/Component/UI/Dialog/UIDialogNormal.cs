using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDialogNormal : DialogView
{
    public override void SetData(DialogBean dialogData)
    {
        base.SetData(dialogData);
        SetSubmitStr(TextHandler.Instance.GetTextById(10003));
        SetCancelStr(TextHandler.Instance.GetTextById(10004));
    }

    public override void SetContent(string content)
    {
        base.SetContent(content);
        UGUIUtil.RefreshUISize(ui_Content.rectTransform);
        UGUIUtil.RefreshUISize(ui_ContentShow);
    }
}
