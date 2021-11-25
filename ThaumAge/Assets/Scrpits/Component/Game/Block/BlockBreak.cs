using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBreak : BaseMonoBehaviour
{
    public MeshRenderer mrBlockBreak;
    public Block block;
    public Vector3Int position;

    public int blockLife = 0;
    //是否需要删除
    public bool isNeedDestory = false;

    //贴图列表
    public List<Texture2D> listBreakTex = new List<Texture2D>();
    //当前破碎的进度
    protected int currentProIndex;

    //更新时间
    protected float timeForUpdate;
    //闲置删除时间
    protected float timeForIdleDestory = 2;

    public void Update()
    {
        HandleForBreakBlockUpdate();
    }

    /// <summary>
    /// 处理-破碎方块更新
    /// </summary>
    public void HandleForBreakBlockUpdate()
    {
        timeForUpdate += Time.deltaTime;
        if (timeForUpdate >= timeForIdleDestory)
        {
            timeForUpdate = 0;
            Reply(0.1f);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockInfo"></param>
    public void SetData(Block block, Vector3Int position)
    {
        if (block == null)
            return;
        this.position = position;
        this.block = block;
        blockLife = block.blockInfo.life;
        transform.position = position;
    }

    /// <summary>
    /// 破碎方块
    /// </summary>
    /// <param name="damage"></param>
    public void Break(int damage, bool isPlayEffect = true)
    {
        //重置刷新时间
        timeForUpdate = 0;

        //生命值扣除
        blockLife -= damage;
        if (blockLife < 0) blockLife = 0;
        else if (blockLife > block.blockInfo.life) blockLife = block.blockInfo.life;

        float breakPro;
        if (block.blockInfo.life != 0)
        {
            breakPro = 1 - ((float)blockLife / block.blockInfo.life);
        }
        else
        {
            breakPro = 1;
        }
        SetBreakPro(breakPro);

        //播放粒子特效
        if (isPlayEffect)
        {
            EffectBean effectData = new EffectBean();
            effectData.timeForShow = 5f;
            effectData.effectPosition = position + Vector3.one * 0.5f;
            effectData.effectName = EffectInfo.BlockBreak_1;
            EffectHandler.Instance.ShowEffect(effectData, (effect) =>
             {
                 //设置粒子颜色
                 Material matNomral = BlockHandler.Instance.manager.GetBlockMaterial(BlockMaterialEnum.Normal);
                 Texture2D texBlock = matNomral.mainTexture as Texture2D;

                 Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
                 int randomUV = Random.Range(0, arrayUVData.Length);
                 Vector2 uvStartPosition = new Vector2(texBlock.width * (arrayUVData[randomUV].y * Block.uvWidth), texBlock.width * (arrayUVData[randomUV].x * Block.uvWidth));

                 int randomXStart = Random.Range((int)uvStartPosition.x, (int)(uvStartPosition.x + (texBlock.width * Block.uvWidth)));
                 int randomYStart = Random.Range((int)uvStartPosition.y, (int)(uvStartPosition.y + (texBlock.height * Block.uvWidth)));

                 int randomXEnd = Random.Range((int)uvStartPosition.x, (int)(uvStartPosition.x + (texBlock.width * Block.uvWidth)));
                 int randomYEnd = Random.Range((int)uvStartPosition.y, (int)(uvStartPosition.y + (texBlock.height * Block.uvWidth)));

                 Color colorStart = TextureUtil.GetPixel(texBlock, new Vector2Int(randomXStart, randomYStart));
                 Color colorEnd = TextureUtil.GetPixel(texBlock, new Vector2Int(randomXEnd, randomYEnd));
                 EffectBlockBreak effectBlockBreak = (EffectBlockBreak)effect;

                 effectBlockBreak.SetEffectColor(colorStart, colorEnd);
             });
        }
    }

    /// <summary>
    /// 恢复方块
    /// </summary>
    /// <param name="pro"></param>
    public void Reply(float pro)
    {
        int life = (int)(block.blockInfo.life * pro);
        Break(-life, false);
        //如果生命值回满了
        if (blockLife >= block.blockInfo.life)
        {
            BlockHandler.Instance.DestroyBreakBlock(position);
        }
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
            mrBlockBreak.material.SetTexture("_BaseColorMap", tex2D);
        }
        currentProIndex = index;
    }
}