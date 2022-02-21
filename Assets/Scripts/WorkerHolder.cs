using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHolder : MonoBehaviour
{
    public Workers worker;


    public GameObject NFTTitle;
    public GameObject createNFTButton;

    void OnEnable()
    {
        if(worker.isNFT == true)
        NFTTitle.SetActive(true);
        else
        createNFTButton.SetActive(true);
    }
    public void CreateNFT()
    {
        FindObjectOfType<ReactSend>().CreateNFTCall(worker.docId);
    }

}
