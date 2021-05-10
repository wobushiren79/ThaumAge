using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System;

public class SceneElementClouds : SceneElementBase
{
    //高度
    public int heightForCloud = 200;
    //速度
    public float speedForCloud = 10;
    //颜色
    public Color colorForCloud = Color.white;
    //模型
    public GameObject objCloudModel;

    //范围
    protected float rangeForHide = 500;

    //列表
    public List<GameObject> listShowCloudObj = new List<GameObject>();
    public Queue<GameObject> listHideCloudObj = new Queue<GameObject>();

    protected void Awake()
    {
        objCloudModel.gameObject.SetActive(false);
    }

    protected void Update()
    {
        HandleForClouds();
    }

    public void HandleForClouds()
    {
        //位置处理
        Vector3 playerPosition = GameHandler.Instance.manager.player.transform.position;
        for (int i = 0; i < listShowCloudObj.Count; i++)
        {
            GameObject objItemCloud = listShowCloudObj[i];
            objItemCloud.transform.Translate(Vector3.left * speedForCloud * Time.deltaTime);
            //超出范围则移除
            if (objItemCloud.transform.position.x < playerPosition.x - rangeForHide)
            {
                ChangeCloudsColorAlpha(objItemCloud, 0, 1,()=> 
                {
                    objItemCloud.gameObject.SetActive(false);
                    listHideCloudObj.Enqueue(objItemCloud);
                });
                listShowCloudObj.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// 创建云
    /// </summary>
    /// <param name="size"></param>
    /// <param name="colorClouds"></param>
    public void CreateCloud(Vector3 size, Color colorClouds)
    {
        GameObject objCloud;
        if (listHideCloudObj.Count > 0)
        {
            objCloud = listHideCloudObj.Dequeue();
            objCloud.gameObject.SetActive(true);
        }
        else
        {
            objCloud = Instantiate(gameObject, objCloudModel);
        }
        listShowCloudObj.Add(objCloud);

        Vector3 startPosition = GameHandler.Instance.manager.player.transform.position + new Vector3(rangeForHide, 0, UnityEngine.Random.Range(-rangeForHide, rangeForHide));
        objCloud.transform.position = new Vector3(startPosition.x, heightForCloud, startPosition.z);
        objCloud.transform.localScale = size;
        objCloud.transform.DOScale(Vector3.zero,0.5f).From().SetEase(Ease.OutBack);
        ChangeCloudsColor(objCloud, colorClouds, 0.1f, null);
    }

    /// <summary>
    /// 改变云的颜色
    /// </summary>
    /// <param name="objCloud"></param>
    /// <param name="color"></param>
    /// <param name="changeTime"></param>
    /// <param name="callBack"></param>
    public void ChangeCloudsColor(GameObject objCloud, Color color, float changeTime, Action callBack)
    {
        MeshRenderer meshRenderer = objCloud.GetComponent<MeshRenderer>();
        meshRenderer.material
            .DOColor(color, changeTime)
            .OnComplete(() =>
            {
                callBack?.Invoke();
            });
    }

    /// <summary>
    /// 改变云的透明度
    /// </summary>
    /// <param name="objCloud"></param>
    /// <param name="alpha"></param>
    /// <param name="changeTime"></param>
    /// <param name="callBack"></param>
    public void ChangeCloudsColorAlpha(GameObject objCloud, float alpha, float changeTime, Action callBack)
    {
        MeshRenderer meshRenderer = objCloud.GetComponent<MeshRenderer>();
        Color currentColor = meshRenderer.material.color;
        meshRenderer.material
            .DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, alpha), changeTime)
            .OnComplete(() =>
            {
                callBack?.Invoke();
            });
    }
}