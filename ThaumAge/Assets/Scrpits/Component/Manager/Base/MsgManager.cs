using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MsgManager : BaseManager
{
    public Dictionary<string, GameObject> listObjModel = new Dictionary<string, GameObject>();
    protected string resUrl = "UI/Msg/";

    public void ShowMsg(Camera camera, string content)
    {
        ShowMsg<MsgView>(MsgEnum.Normal, content, GameUtil.MousePointToUGUIPoint(camera, (RectTransform)gameObject.transform));
    }

    public void ShowMsg(string content)
    {
        ShowMsg<MsgView>(MsgEnum.Normal, content, GameUtil.MousePointToUGUIPoint(Camera.main, (RectTransform)gameObject.transform));
    }

    public void ShowMsg(string content, Vector3 msgPosition)
    {
        ShowMsg<MsgView>(MsgEnum.Normal, content, msgPosition);
    }

    public T ShowMsg<T>(MsgEnum msgType, string content, Vector3 msgPosition) where T : MsgView
    {
        string msgName = EnumUtil.GetEnumName(msgType);
        GameObject objMsg = CreateMsg(msgName);
        if (objMsg)
        {
            MsgView msgView = objMsg.GetComponent<MsgView>();
            msgView.SetContent(content);
            RectTransform rtfMsg = (RectTransform)msgView.transform;
            rtfMsg.anchoredPosition = msgPosition;
            return msgView as T;
        }
        else
        {
            LogUtil.LogError("没有找到指定Msg：" + "Resources/" + resUrl + msgName);
            return null;
        }
    }

    public GameObject CreateMsg(string name)
    {
        GameObject objModel = null;
        if (listObjModel.TryGetValue(name, out objModel))
        {

        }
        else
        {
            objModel = CreatMsgModel(name);
        }
        if (objModel == null)
            return null;
        GameObject obj = Instantiate(gameObject, objModel);
        return obj;
    }

    private GameObject CreatMsgModel(string name)
    {
        GameObject objModel = Resources.Load<GameObject>(resUrl + name);
        objModel.name = name;
        listObjModel.Add(name, objModel);
        return objModel;
    }

    public RectTransform GetContainer()
    {
        return (RectTransform)gameObject.transform;
    }
}
