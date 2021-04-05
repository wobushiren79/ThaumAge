using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public Vector2Int imageDim;
    public int regionAmount;
    public bool drawByDistance = false;


    private void Start()
    {
        // GetComponent<SpriteRenderer>().sprite = Sprite.Create((drawByDistance ? GetDiagramByDistance() : GetDiagram()), new Rect(0, 0, imageDim.x, imageDim.y), Vector2.one * 0.5f);



        //List<Vector3Int> listCenter = new List<Vector3Int>();
        //for (int i = 0; i < 1; i++)
        //{
        //    int x = random.Next(-10, 10);
        //    int z = random.Next(-10, 10);
        //    LogUtil.Log(x + " " + z);
        //}
        //GetBiomeCenterPosition(new Vector3Int(0, 0, 0));
        //GetBiomeCenterPosition(new Vector3Int(5, 0, 5));
        //GetBiomeCenterPosition(new Vector3Int(10, 0, 10));
        //GetBiomeCenterPosition(new Vector3Int(15, 0, 15));
        //GetBiomeCenterPosition(new Vector3Int(0,0,0));
        Test2();
    }


    public void Test2()
    {
        for (int i = -50; i < 50; i++)
        {
            for (int f = -50; f < 50; f++)
            {

                RandomTools random = RandomUtil.GetRandom(1,i,f);
                int addRate1 = random.NextInt(100);
                LogUtil.Log("addRate:" + addRate1);
                if (addRate1 < 50)
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.position = new Vector3Int(i, 0, f);
                }
            }
        }
    }

    public List<Vector3Int> GetBiomeCenterPosition(Vector3Int wPos)
    {
        List<Vector3Int> listData = new List<Vector3Int>();

        for (int x = -5; x < 5; x++)
        {
            for (int z = -5; z < 5; z++)
            {
                Vector3Int currentPosition = new Vector3Int(wPos.x + x, 0, wPos.z + z);
                RandomTools random = RandomUtil.GetRandom(int.MaxValue, currentPosition.x, currentPosition.z);
                int addRate = random.NextInt(100);
                LogUtil.Log("addRate:" + addRate);
                if (addRate <= 10)
                {
                    listData.Add(currentPosition);
                }
            }
        }

        for (int i = 0; i < listData.Count; i++)
        {

            Vector3Int itemData = listData[i];
            LogUtil.Log("x,z:" + itemData.x + " " + itemData.z);
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = itemData;
        }
        LogUtil.Log("------------------------------");
        return listData;
    }

    Texture2D GetDiagram()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        Color[] regions = new Color[regionAmount];
        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
            regions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
        Color[] pixelColors = new Color[imageDim.x * imageDim.y];
        for (int x = 0; x < imageDim.x; x++)
        {
            for (int y = 0; y < imageDim.y; y++)
            {
                int index = x * imageDim.x + y;
                pixelColors[index] = regions[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)];
            }
        }
        return GetImageFromColorArray(pixelColors);
    }
    Texture2D GetDiagramByDistance()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];

        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDim.x), Random.Range(0, imageDim.y));
        }
        Color[] pixelColors = new Color[imageDim.x * imageDim.y];
        float[] distances = new float[imageDim.x * imageDim.y];

        //you can get the max distance in the same pass as you calculate the distances. :P oops!
        float maxDst = float.MinValue;
        for (int x = 0; x < imageDim.x; x++)
        {
            for (int y = 0; y < imageDim.y; y++)
            {
                int index = x * imageDim.x + y;
                distances[index] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);
                if (distances[index] > maxDst)
                {
                    maxDst = distances[index];
                }
            }
        }

        for (int i = 0; i < distances.Length; i++)
        {
            float colorValue = distances[i] / maxDst;
            pixelColors[i] = new Color(colorValue, colorValue, colorValue, 1f);
        }
        return GetImageFromColorArray(pixelColors);
    }
    Texture2D GetImageFromColorArray(Color[] pixelColors)
    {
        Texture2D tex = new Texture2D(imageDim.x, imageDim.y);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(pixelColors);
        tex.Apply();
        return tex;
    }

    int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallestDst = float.MaxValue;
        int index = 0;
        for (int i = 0; i < centroids.Length; i++)
        {
            if (Vector2.Distance(pixelPos, centroids[i]) < smallestDst)
            {
                smallestDst = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }


}
