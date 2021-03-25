using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public Button ui_Save;
    public Button ui_Load;

    public void Start()
    {
        ui_Save.onClick.AddListener(Save);
        ui_Load.onClick.AddListener(Load);
    }

    public void Save()
    {


    }

    public void Load()
    {
        GameDataHandler.Instance.manager.DeletGameData();
    }
}
