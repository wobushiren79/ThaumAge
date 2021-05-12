using UnityEditor;
using UnityEngine;

public class BaseUIView : BaseMonoBehaviour
{
    protected RectTransform rectTransform;
    public virtual void Awake()
    {
        AutoLinkUI();
        rectTransform = ((RectTransform)transform);
    }


}