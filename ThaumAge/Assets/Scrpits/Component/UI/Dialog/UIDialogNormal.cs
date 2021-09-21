using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIDialogNormal : DialogView
{
    public override void SetData(DialogBean dialogData)
    {
        base.SetData(dialogData);
        UGUIUtil.RefreshUISize(ui_ContentShow);
    }
}
