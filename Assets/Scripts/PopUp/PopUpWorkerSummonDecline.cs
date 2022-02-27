using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWorkerSummonDecline : MonoBehaviour
{
    public Text limitText;
    public void OpenSummonWorkerDecline()
    {
        limitText.text = Constants.currentUser.workers.Count.ToString() + "/" + Constants.currentUser.workerCapacity.ToString();
    }

    public void ClosePop()
    {
        gameObject.SetActive(false);
    }
}
