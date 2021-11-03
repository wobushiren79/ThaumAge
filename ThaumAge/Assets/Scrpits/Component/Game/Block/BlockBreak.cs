using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBreak : BaseMonoBehaviour
{
    public MeshRenderer mrBlockBreak;

    //贴图列表
    public List<Texture2D> listBreakTex = new List<Texture2D>();

    /// <summary>
    /// 设置破碎进度
    /// </summary>
    /// <param name="pro"></param>
    public void SetBreakPro(float pro)
    {

    }
}