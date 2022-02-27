using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ReactCom : MonoBehaviour
{       
    GameManager gm;
    PopUpController popUpController;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        popUpController = FindObjectOfType<PopUpController>();
    }
    public void StartTown(string json)
    {
        gm.StartTown(json);
    }

    public void GetUser(string json)
    {
        gm.GetUser(json);
    }

    public void OpenCloseLoadingBar(string value)
    {
        if(value == "true")
        {
            gm.OpenCloseLoadingBar(true);
        }
        else
        {
            gm.OpenCloseLoadingBar(false);       
        }
    }

    public void CloseAllPops(string n)
    {
        popUpController.CloseAllPops();
    }

    public void OpenInfoPop(string message)
    {
        popUpController.OpenInfoPop(message);
    }
}
