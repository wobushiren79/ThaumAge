using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class BlockCptBreak : BaseMonoBehaviour
{
    public MeshRenderer mrBlockBreak;
    public MeshFilter mfBlockBreak;
    public Transform tfCenter;

    public Block block;
    public Vector3Int worldPosition;

    public int blockLife = 0;
    public int blockLifeMax = 0;
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

    private Tween animForBreakShake;

    protected Vector2[] uvList = new Vector2[]
    {
        new Vector2(0,0),
        new Vector2(0,1),
        new Vector2(1,1),
        new Vector2(1,0)
    };

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
    public void SetData(Block block, Vector3Int worldPosition)
    {
        if (block == null)
            return;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out Chunk targetChunk);
        this.worldPosition = worldPosition;
        this.block = block;
        blockLifeMax = block.GetBlockLife(targetChunk, worldPosition - targetChunk.chunkData.positionForWorld);
        blockLife = blockLifeMax;

        transform.position = worldPosition;
    }


    /// <summary>
    /// 破碎方块
    /// </summary>
    /// <param name="damage"></param>
    public void Break(int damage)
    {
        //重置刷新时间
        timeForUpdate = 0;

        //生命值扣除
        blockLife -= damage;
        if (blockLife < 0)
        {
            blockLife = 0;
        }
        else if (blockLife > blockLifeMax)
        {
            blockLife = blockLifeMax;
        }

        float breakPro;
        if (blockLifeMax != 0)
        {
            breakPro = 1 - ((float)blockLife / blockLifeMax);
        }
        else
        {
            breakPro = 1;
        }
        SetBreakPro(breakPro);

        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //设置破坏的形状
        Mesh newMeshData = block.blockShape.GetCompleteMeshData(targetChunk, localPosition, targetDirection);

        //设置方向
        tfCenter.eulerAngles = targetBlock.GetRotateAngles(targetDirection);
        tfCenter.localScale = new Vector3(1.001f, 1.001f, 1.001f);
        tfCenter.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
        //如果没有顶点 则直接取实例模型 并且让他抖动一下
        if (newMeshData.vertices.Length == 0)
        {
            if (!block.blockInfo.model_name.IsNull() && damage > 0)
            {
                GameObject objTarget = block.GetBlockObj(targetChunk, localPosition);
                if (animForBreakShake != null && !animForBreakShake.IsComplete())
                {
                    animForBreakShake.Complete();
                }
                animForBreakShake = objTarget.transform.DOShakePosition(0.2f, 0.02f, 20, 180);
            }
        }

        Vector2[] newUVS = new Vector2[newMeshData.vertices.Length];
        for (int i = 0; i < newMeshData.vertices.Length; i++)
        {
            int indexUV = i % 4;
            newUVS[i] = uvList[indexUV];
        }
        newMeshData.SetUVs(0, newUVS);
        mfBlockBreak.mesh = newMeshData;

        //如果是link类型，
        if (targetBlock.blockType == BlockTypeEnum.LinkChild)
        {
            block.GetBlockMetaData(targetChunk, localPosition,out BlockBean blockData, out BlockMetaBaseLink blockMetaBaseLink);
            transform.position = blockMetaBaseLink.GetBasePosition();
        }
    }

    /// <summary>
    /// 恢复方块
    /// </summary>
    /// <param name="pro"></param>
    public void Reply(float pro)
    {
        int life = Mathf.CeilToInt(blockLifeMax * pro);
        Break(-life);
        //如果生命值回满了
        if (blockLife >= blockLifeMax)
        {
            BlockHandler.Instance.DestroyBreakBlock(worldPosition);
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
        Texture2D tex2D = listBreakTex[index];
        mrBlockBreak.material.SetTexture("_Tex", tex2D);
        currentProIndex = index;
    }

    /// <summary>
    /// 播放粒子破碎特效
    /// </summary>    
    public static void PlayBlockCptBreakEffect(BlockTypeEnum blockType, Vector3 position)
    {
        Block block = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
        PlayBlockCptBreakEffect(block, position);
    }

    public static void PlayBlockCptBreakEffect(Block block, Vector3 position)
    {
        EffectBean effectData = new EffectBean();
        effectData.timeForShow = 5f;
        effectData.effectPosition = position + Vector3.one * 0.5f;
        effectData.effectName = EffectInfo.BlockBreak_1;
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {
            //设置粒子颜色
            Material matNomral = BlockHandler.Instance.manager.GetBlockMaterial(block.blockInfo.GetBlockMaterialType());
            Texture2D texBlock = matNomral.mainTexture as Texture2D;

            Color colorStart;
            Color colorEnd;

            if ((int)block.blockInfo.GetBlockShape() > 90000)
            {
                //如果是自定义模型的方块 则直接随机获取颜色
                //colorStart = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                //colorEnd = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                colorStart = Color.gray;
                colorEnd = Color.gray;
            }
            else
            {
                Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
                if (arrayUVData == null)
                {
                    arrayUVData = new Vector2Int[] { Vector2Int.zero };
                }
                int randomUV = Random.Range(0, arrayUVData.Length);
                Vector2 uvStartPosition = new Vector2(texBlock.width * (arrayUVData[randomUV].y * BlockShape.uvWidth), texBlock.width * (arrayUVData[randomUV].x * BlockShape.uvWidth));

                int randomNumber = 0;
                do
                {
                    int randomXStart = Random.Range((int)uvStartPosition.x, (int)(uvStartPosition.x + (texBlock.width * BlockShape.uvWidth)));
                    int randomYStart = Random.Range((int)uvStartPosition.y, (int)(uvStartPosition.y + (texBlock.height * BlockShape.uvWidth)));

                    int randomXEnd = Random.Range((int)uvStartPosition.x, (int)(uvStartPosition.x + (texBlock.width * BlockShape.uvWidth)));
                    int randomYEnd = Random.Range((int)uvStartPosition.y, (int)(uvStartPosition.y + (texBlock.height * BlockShape.uvWidth)));

                    colorStart = TextureUtil.GetPixel(texBlock, new Vector2Int(randomXStart, randomYStart));
                    colorEnd = TextureUtil.GetPixel(texBlock, new Vector2Int(randomXEnd, randomYEnd));
                    randomNumber++;
                }
                while ((colorStart.a == 0 || colorEnd.a == 0) && randomNumber < 10);

            }

            EffectBlockBreak effectBlockCptBreak = (EffectBlockBreak)effect;
            effectBlockCptBreak.SetEffectColor(colorStart, colorEnd);
        });
    }
}