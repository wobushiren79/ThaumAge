using System.Collections;
using UnityEngine;

public class TextHandler : BaseHandler<TextHandler, TextManager>
{

    /// <summary>
    /// 通过ID获取文本
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetTextById(long id)
    {
        return manager.GetTextById(id);
    }

}