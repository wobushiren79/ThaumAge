using System.Collections.Generic;
using System.Diagnostics;
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
        Dictionary<int, string> dic = new Dictionary<int, string>();

        for (int i = 0; i < 1000000; i++)
        {
            dic.Add(i, "index_:" + i);
        }
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        foreach (var itemData in dic)
        {
            string data = itemData.Value;
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        foreach (int itemData in dic.Keys)
        {
            string data = dic[itemData];
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        foreach (string itemData in dic.Values)
        {
            string data = itemData;
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        List<int> test = new List<int>(dic.Keys);
        for (int i = 0; i < test.Count; i++)
        {
            string data = dic[test[i]];
        }
        TimeUtil.GetMethodTimeEnd("4", stopwatch);
    }



}
