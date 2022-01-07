using UnityEditor;
using UnityEngine;

public class UIViewSynthesisItem : BaseUIView
{
    protected ItemsSynthesisBean itemsSynthesis;

    public void SetData(ItemsSynthesisBean itemsSynthesis)
    {
        this.itemsSynthesis = itemsSynthesis;
    }
}