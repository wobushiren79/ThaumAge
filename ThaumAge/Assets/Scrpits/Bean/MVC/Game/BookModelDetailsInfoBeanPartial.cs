using UnityEditor;
using UnityEngine;

public partial class BookModelDetailsInfoBean
{
    public string title
    {
        get
        {
            return GetBaseText("title");
        }
    }

    public string content
    {
        get
        {
            return GetBaseText("content");
        }
    }
}