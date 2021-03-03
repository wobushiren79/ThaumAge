using UnityEditor;
using UnityEngine;

public class BaseUIView : BaseMonoBehaviour
{
    public virtual void Awake()
    {
        AutoLinkUI();
    }

}