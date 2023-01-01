
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
public class Test : BaseMonoBehaviour
{
    public MagicBean magicData;

    protected List<Vector3Int> listData1 = new List<Vector3Int>();
    protected HashSet<Vector3Int> listData2 = new HashSet<Vector3Int>();
    protected Dictionary<Vector3Int, string> listData3 = new Dictionary<Vector3Int, string>();
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            //Debug.Log("tEST1");
            //Player player = GameHandler.Instance.manager.player;
            //magicData.createPosition = player.transform.position + Vector3.up;
            //magicData.direction = Camera.main.transform.forward;
            //magicData.createTargetId = player.gameObject.GetInstanceID();
            //magicData.createTargetObj = player.gameObject;
            //MagicHandler.Instance.CreateMagic(magicData);
            listData1.Clear();
            listData2.Clear();
            listData3.Clear();
            Vector3Int itemTest = new Vector3Int(123,167,12361);
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        listData1.Add(new Vector3Int(x, y, z));
                        listData2.Add(new Vector3Int(x, y, z));
                        listData3.Add(new Vector3Int(x, y, z), "Test");
                    }
                }
            }

            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();

            foreach (var itemData in listData3)
            {
                var item = itemData.Key;
  
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("3 "+ stopwatch.ElapsedTicks);
            stopwatch.Reset();
            stopwatch.Restart();
            foreach (var itemData in listData2)
            {
           
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("2 " + stopwatch.ElapsedTicks);
            stopwatch.Reset();
            stopwatch.Restart();
            var demoEnum = listData2.GetEnumerator();
            while (demoEnum.MoveNext())
            {
                Vector3Int res = demoEnum.Current;
                if(res.x==1&&res.y==1&&res.z==1)
                    listData2.Add(new Vector3Int(1111,1111,1111));
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log("1 " + stopwatch.ElapsedTicks);
           // stopwatch.Reset();
            //stopwatch.Restart();

            //for (int i = 0; i < 16 * 16 * 256; i++)
            //{
            //    if (listData3.ContainsKey(itemTest))
            //    {

            //    }
            //}

            //stopwatch.Stop();
            //UnityEngine.Debug.Log("33 " + stopwatch.ElapsedTicks);
            //stopwatch.Reset();
            //stopwatch.Restart();
            //for (int i = 0; i < 16 * 16 * 256; i++)
            //{
            //    if (listData2.Contains(itemTest))
            //    {

            //    }
            //}

            //stopwatch.Stop();
            //UnityEngine.Debug.Log("22 " + stopwatch.ElapsedTicks);
            //stopwatch.Reset();
            //stopwatch.Restart();
            ////for (int i = 0; i < 16 * 16 * 256; i++)
            ////{
            ////    if (listData1.Contains(itemTest))
            ////    {

            ////    }
            ////}
            //stopwatch.Stop();
            //UnityEngine.Debug.Log("11 " + stopwatch.ElapsedTicks);
        }
    }


}
