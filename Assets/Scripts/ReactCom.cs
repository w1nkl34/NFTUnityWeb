using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ReactCom : MonoBehaviour
{       
    GameManager gm;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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

}
