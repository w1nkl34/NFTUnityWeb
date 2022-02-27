using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorkerHolder : MonoBehaviour
{
    public Workers worker;
    public GameObject NFTTitle;
    public Image workerImage;

    void OnEnable()
    {
        if(worker.isNFT == true)
        NFTTitle.SetActive(true);
    }

    public void OpenPopDetails()
    {
        FindObjectOfType<PopUpController>().OpenWorkerDetails(worker,workerImage.sprite);
    }

}
