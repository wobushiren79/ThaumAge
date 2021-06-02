using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Test : BaseMonoBehaviour
{
    [Serializable]
    public class TestData
    {
        public short data1;
    }

    public List<TestData> listData = new List<TestData>();
    private void Start()
    {
        //UIHandler.Instance.manager.OpenUI<UIGameMain>(UIEnum.GameMain);

        short count = 32767;
        for (int i=0;i<count;i++)
        {
            TestData testData = new TestData();
            testData.data1 = count;
            listData.Add(testData);
        }
        using (Stream s = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(s, listData);
           long  size = s.Length;
            LogUtil.Log("size:"+ size);
        }
        LogUtil.Log("GetTotalMemory:"+GC.GetTotalMemory(true)+"");
    }

    private int GetObjectSize(object TestObject)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        byte[] Array;
        bf.Serialize(ms, TestObject);
        Array = ms.ToArray();
        return Array.Length;
    }
}
