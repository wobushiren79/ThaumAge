using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;

public class JsonUtil : ScriptableObject
{
    public static IEnumerable<T> DeserializeValues<T>(Stream stream)
    {
        return DeserializeValues<T>(new StreamReader(stream));
    }

    public static IEnumerable<T> DeserializeValues<T>(TextReader textReader)
    {
        var serializer = JsonSerializer.CreateDefault();
        var reader = new JsonTextReader(textReader);
        reader.SupportMultipleContent = true;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.Comment)
                        continue; // Do nothing
                    else if (reader.TokenType == JsonToken.EndArray)
                        break; // Break from the loop
                    else
                        yield return serializer.Deserialize<T>(reader);
                }
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.Comment)
                        continue; // Do nothing
                    else if (reader.TokenType == JsonToken.PropertyName)
                        continue; // Eat the property name
                    else if (reader.TokenType == JsonToken.EndObject)
                        break; // Break from the loop
                    else
                        yield return serializer.Deserialize<T>(reader);
                }
            }
        }
    }
    /// <summary>
    /// Json转换成类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strData"></param>
    /// <returns></returns>
    public static T FromJson<T>(string strData)
    {
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        T dataBean = JsonUtility.FromJson<T>(strData);
        TimeUtil.GetMethodTimeEnd("1:",stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        //dataBean = JsonConvert.DeserializeObject<T>(strData);
        using (WebClient client = new WebClient())
        using (Stream stream = client.OpenRead(strData))
        using (StreamReader streamReader = new StreamReader(stream))
        using (JsonTextReader reader = new JsonTextReader(streamReader))
        {
            reader.SupportMultipleContent = true;

            var serializer = new JsonSerializer();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    dataBean = serializer.Deserialize<T>(reader);
                }
            }
        }

        TimeUtil.GetMethodTimeEnd("2:", stopwatch);
        return dataBean;
    }

    /// <summary>
    /// 类转换成Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataBean"></param>
    /// <returns></returns>
    public static string ToJson<T>(T dataBean)
    {
        //string json = JsonUtility.ToJson(dataBean);
        string json = JsonConvert.SerializeObject(dataBean);
        return json;
    }

}