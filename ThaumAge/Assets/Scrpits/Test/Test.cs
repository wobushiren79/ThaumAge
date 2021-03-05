using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Test : BaseHandler<Test,BaseManager> {



    private void Start()
    {
        WorldCreateHandler.Instance.CreateChunk();
    }

}
