using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Clouds : BaseMonoBehaviour
{
    //高度
    public int heightForCloud = 200;
    //范围
    public float rangeForHide = 100;
    //速度
    public float speedForCloud = 1;
    //颜色
    public Color colorForCloud = Color.white;
    //模型
    public GameObject objCloudModel;

    //材质
    protected Material materialForCloud;

    //列表
    public List<GameObject> listShowCloudObj = new List<GameObject>();
    public Queue<GameObject> listHideCloudObj = new Queue<GameObject>();

    protected void Awake()
    {
        objCloudModel.gameObject.SetActive(false);
        materialForCloud = objCloudModel.GetComponent<MeshRenderer>().sharedMaterial;
        materialForCloud.color = colorForCloud;
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
            if (objItemCloud.transform.position.x < playerPosition.x - rangeForHide
                || objItemCloud.transform.position.x > playerPosition.x + rangeForHide
                || objItemCloud.transform.position.z < playerPosition.z - rangeForHide
                || objItemCloud.transform.position.z > playerPosition.z + rangeForHide)
            {
                objItemCloud.gameObject.SetActive(false);
                listShowCloudObj.RemoveAt(i);
                listHideCloudObj.Enqueue(objItemCloud);
                i--;
            }
        }
        //颜色处理
        Color lerpColorCloud = Color.Lerp(materialForCloud.color, colorForCloud,Time.deltaTime);
        materialForCloud.color = lerpColorCloud;
    }

    public void CreateCloud(Vector3 startPosition, Vector3 size)
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
        objCloud.transform.position = startPosition;
        objCloud.transform.localScale = size;
    }

    public void ChangeCloudsColor(Color color)
    {
        this.colorForCloud = color;
    }
}