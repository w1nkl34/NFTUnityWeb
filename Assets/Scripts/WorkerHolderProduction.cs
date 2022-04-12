using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerHolderProduction : MonoBehaviour
{
    public Workers worker;
    public GameObject NFTTitle;
    public Image workerImage;
    public GameObject selectedObj;

    [HideInInspector]
    public PopUpWorkProductionController popUpWorkProductionController;

    void OnEnable()
    {
        if(worker.isNFT == true)
        NFTTitle.SetActive(true);
    }
    

    public void ChangeSelectedWorker()
    {
        popUpWorkProductionController.ChangeSelectedWorker(worker);
    }
}
