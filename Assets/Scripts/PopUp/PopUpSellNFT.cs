using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpSellNFT : MonoBehaviour
{
    public Workers worker;
    public TMP_InputField price;

    public void OpenSellNFT(Workers worker)
    {
        this.worker = worker;
    }

    public void SellNFT()
    {
        if(price.text != "")
        {
        if(float.Parse(price.text.ToString()) >= 0.1)
        {
            FindObjectOfType<ReactSend>().SellNFTCall(worker.docId,float.Parse(price.text.ToString()));
        }
        else
        {
        Debug.Log("error");
        }
        }
        else
        Debug.Log("error");
    }

    public void ClosePop()
    {
        gameObject.SetActive(false);
    }
}
