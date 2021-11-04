using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBreak : BaseMonoBehaviour
{
    public MeshRenderer mrBlockBreak;
    public BlockInfoBean blockInfo;

    public int blockLife = 0;

    //贴图列表
    public List<Texture2D> listBreakTex = new List<Texture2D>();
    //当前破碎的进度
    protected int currentProIndex;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(BlockInfoBean blockInfo)
    {
        if (blockInfo == null)
            return;
        this.blockInfo = blockInfo;
        blockLife = blockInfo.life;
    }

    /// <summary>
    /// 设置破碎进度
    /// </summary>
    /// <param name="pro"></param>
    public void SetBreakPro(float pro)
    {
        int index = Mathf.RoundToInt(pro * 10);
        if (index >= listBreakTex.Count)
        {
            index = listBreakTex.Count - 1;
        }
        //如果进度没有变，则不修改贴图
        if (index == currentProIndex)
        {

        }
        //修改贴图
        else 
        {
            Texture2D tex2D = listBreakTex[index];
            mrBlockBreak.material.SetTexture("_MainTex",tex2D);
        }
        currentProIndex = index;
    }
}