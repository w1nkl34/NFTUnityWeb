using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWorkIncreaseDecreaseTime : MonoBehaviour
{

    public Button increaseWorkerWorkTimesButton;

    public Button decreaseWorkerWorkTimesButton;

    public Button applyButton;

    public Text WorkerEfficiencyTotalText;
    WorkerWork workerWork;

    public int tempTimes;

    public string zoneDocId;

    public string fieldDocId;

    public void ClosePop()
    {
        WorkerEfficiencyTotalText.text = "";
        gameObject.SetActive(false);
    }

    public void OpenPop(WorkerWork workerWork,string zoneDocId,string fieldDocId)
    {
        this.workerWork = workerWork;
        this.zoneDocId = zoneDocId;
        this.fieldDocId = fieldDocId;
        tempTimes = workerWork.times;
        CheckInteracts();

    }

    public void ApplyWorkerTime()
    {
        FindObjectOfType<FirebaseApi>().UpdateWorkerWorkTimes(workerWork.workerWorkDocId,tempTimes,zoneDocId,fieldDocId);
        ClosePop();
    }

    public void Update()
    {
        WorkerEfficiencyTotalText.text =  workerWork.currentTime.ToString() + "/" + tempTimes.ToString();
        if(tempTimes <= workerWork.currentTime)
        tempTimes+=1;
       
    }

    public void CheckInteracts()
    {
        Workers tempWorker = new Workers();
        for(int i = 0; i<Constants.currentUser.workers.Count; i++)
        {
            if(workerWork.workerDocId == Constants.currentUser.workers[i].docId)
            {
                tempWorker = Constants.currentUser.workers[i];
                break;
            }
        }
        if(tempWorker.currentStamina >= (tempTimes -workerWork.currentTime) + 1)
        {
            increaseWorkerWorkTimesButton.interactable = true;
        }
        else
        {
            increaseWorkerWorkTimesButton.interactable = false;
        }

        if(tempTimes > 1 && tempTimes > workerWork.currentTime +1)
        {
            decreaseWorkerWorkTimesButton.interactable = true;
        }
        else
        {
            decreaseWorkerWorkTimesButton.interactable = false;
        }

        if(workerWork.times == tempTimes)
        applyButton.interactable = false;
        else
        applyButton.interactable = true;
    }

    public void IncreaseWorkerWorkTime()
    {
        Workers tempWorker = new Workers();
        for(int i = 0; i<Constants.currentUser.workers.Count; i++)
        {
            if(workerWork.workerDocId == Constants.currentUser.workers[i].docId)
            {
                tempWorker = Constants.currentUser.workers[i];
                break;
            }
        }
        if(tempWorker.currentStamina >= (tempTimes -workerWork.currentTime) + 1)
        {
            tempTimes++;
            
                CheckInteracts();
        }

    }
    
    public void DecreaseWorkerWorkTime()
    {
        if(tempTimes > 1 &&tempTimes > workerWork.currentTime +1)
        {
            tempTimes--;
            
                CheckInteracts();
        }
    }
}
