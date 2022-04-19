using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PopUpWorkProductionController : MonoBehaviour
{

    public GameManager gm;
    public GameObject workerMain;
    public Field field;
    public Zones zone;
    public Text productTypeText;
    public Image productImage;
    public Workers selectedWorker = null;
    public Button startToWorkButton;
    public GameObject onWorkTimer;
    public Text WorkerEfficiencyText;
    public Text WorkerEfficiencyTotalText;
    public GameObject increaseDecreaseButtons;
    public GameObject increaseDecreaseButtonsOnWork;
    bool dataReceived = false;

    WorkerWork workerWork;

    DateTime endTime;
    DateTime startTime;

    public Image upgradeSlider;
    public Text upgradeTimeText;

    public int amountToWork = 1;

    public Button increaseWorkerWorkTimesButton;
    public Button decreaseWorkerWorkTimesButton;

    public void ResetData()
    {
        ResetAllImages();
        dataReceived = false;
        onWork = false;
        workerWork = null;
        increaseDecreaseButtons.SetActive(false);
        WorkerEfficiencyText.text = "Select Worker";
        WorkerEfficiencyTotalText.text = "";
        amountToWork =1;
        startToWorkButton.gameObject.SetActive(false);
    }

    public void IncreaseAmountToWork()
    {
     if(amountToWork < (int)selectedWorker.currentStamina)
        amountToWork++;
    UpdateWorkerEfficiencyTotalText();
    }

    public void UpdateWorkerEfficiencyTotalText()
    {
          WorkerEfficiencyTotalText.text =  "(x" + amountToWork + ")";
    }

    public void UpdateAmountToWorkOnWorkerChange()
    {
        if((int)selectedWorker.currentStamina < amountToWork)
        amountToWork = (int)selectedWorker.currentStamina;
        UpdateWorkerEfficiencyTotalText();

    }

    public void ResetAmountToWork()
    {
        amountToWork = 1;
        UpdateWorkerEfficiencyTotalText();
    }

    public void StartToWork()
    {
        FindObjectOfType<FirebaseApi>().WorkerToWork(zone.docId,field.docId,selectedWorker.docId,amountToWork.ToString());
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
        if(tempWorker.currentStamina >= workerWork.times + 1)
        {
            increaseWorkerWorkTimesButton.interactable = true;
        }
        else
        {
            increaseWorkerWorkTimesButton.interactable = false;
        }

        if(workerWork.times > 1 && workerWork.times > workerWork.currentTime +1)
        {
            decreaseWorkerWorkTimesButton.interactable = true;
        }
        else
        {
            decreaseWorkerWorkTimesButton.interactable = false;
        }
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
        if(tempWorker.currentStamina >= workerWork.times + 1)
        {
                for(int i= 0; i<Constants.currentUser.workerWorks.Count; i++)
                {
                    if(Constants.currentUser.workerWorks[i].workerWorkDocId == workerWork.workerWorkDocId)
                    {
                        Constants.currentUser.workerWorks[i].times++;
                        break;
                    }
                }
                CheckInteracts();
                CheckWorkerWork();
                FindObjectOfType<FirebaseApi>().IncreaseWorkerWorkTime(workerWork.workerWorkDocId);
        }

    }
    
    public void DecreaseWorkerWorkTime()
    {
        if(workerWork.times > 1 && workerWork.times > workerWork.currentTime +1)
        {
                for(int i= 0; i<Constants.currentUser.workerWorks.Count; i++)
                {
                    if(Constants.currentUser.workerWorks[i].workerWorkDocId == workerWork.workerWorkDocId)
                    {
                        Constants.currentUser.workerWorks[i].times--;
                        break;
                    }
                }
                CheckInteracts();
                CheckWorkerWork();
                FindObjectOfType<FirebaseApi>().DecreaseWorkerWorkTime(workerWork.workerWorkDocId);
        }
    }

    public void RemoveWorkerWorkTime()
    {
        FindObjectOfType<FirebaseApi>().CancelWorkerWork(workerWork.workerWorkDocId,workerWork.workerDocId);
    }

    public void DecreaseAmountToWork()
    {
        if(amountToWork > 1)
        amountToWork--;
        UpdateWorkerEfficiencyTotalText();

    }

    public void MaxAmountToWork()
    {
        amountToWork = (int)selectedWorker.currentStamina;
        UpdateWorkerEfficiencyTotalText();
    }

        public void MinAmountToWork()
    {
        amountToWork = 1;
        UpdateWorkerEfficiencyTotalText();
    }
    bool onWork = false;

   public void GenerateProduct(Field field,Zones zone,WorkerWork workerWork)
   {
        ResetData();
        this.workerWork = workerWork;
        this.zone = zone;
       this.field = field;
       for(int i = 0; i<Constants.currentUser.workerWorks.Count; i++)
       {
           if(Constants.currentUser.workerWorks[i].fieldDocId == field.docId && Constants.currentUser.workerWorks[i].zoneDocId == zone.docId)
           {
               onWork = true;
           }
       }
    for(int i = 0; i<field.productItem.Count; i++)
       {
           foreach(KeyValuePair<string,object> item in field.productItem[i])
           {
               productTypeText.text = item.Key;
               if(item.Key == "wood")
               {
                productImage.sprite = gm.publicImages.woodImage;
               }
               if(item.Key == "stone")
               {
                productImage.sprite = gm.publicImages.stoneImage;
               }
               if(item.Key == "peridotShard")
               {
                productImage.sprite = gm.publicImages.peridotImage;   
               }
           }
       }
    if(!onWork)
    {
       StartCoroutine(GenerateWorkers());
       increaseDecreaseButtonsOnWork.gameObject.SetActive(false);
        onWorkTimer.SetActive(false);
    }
    if(onWork)
    {
            CheckWorkerWork();
            CheckInteracts();
        if(DateTime.Compare(DateTime.UtcNow ,endTime) < 0 )
        {
            WorkerEfficiencyText.text = "On Work";
            increaseDecreaseButtonsOnWork.gameObject.SetActive(true);
            startToWorkButton.gameObject.SetActive(false);
            onWorkTimer.SetActive(true);
            dataReceived = true;
        }
    }

   }
     public void CheckWorkerWork()
    {
        startTime = workerWork.startTime.AddMinutes(2*workerWork.currentTime);
        endTime = workerWork.startTime.AddMinutes(2*workerWork.times);
    }
   public void Update()
   {
       if(onWork && dataReceived)
       {
           WorkerEfficiencyTotalText.text =  workerWork.currentTime.ToString() + "/" + workerWork.times.ToString();
            TimeSpan ts1 =  endTime - startTime;
            TimeSpan ts = endTime - DateTime.UtcNow;
            upgradeSlider.fillAmount = 1 - ((float)ts.TotalSeconds / (float)ts1.TotalSeconds);
            upgradeTimeText.text = string.Format("{1}:{2}:{3}", ts.Days, (ts.Days*24) + ts.Hours, ts.Minutes, ts.Seconds);
       }
        if(DateTime.Compare(DateTime.UtcNow ,endTime) > 0 &&onWork && dataReceived)
        {
           onWork = false;
           OnWorkFinishedLocally();
        }
   }

   public void OnWorkFinishedLocally()
   {
            WorkerEfficiencyText.text = "On Work";
            increaseDecreaseButtonsOnWork.gameObject.SetActive(false);
            startToWorkButton.gameObject.SetActive(false);
            onWorkTimer.SetActive(false);
   }

   public void ClosePop()
   {
        ResetData();
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
        gameObject.SetActive(false);
   }

    public void ResetAllImages()
    {
        int count = 0;
        foreach(Transform child in workerMain.transform.parent)
        {
            if(count != 0)
            Destroy(child.gameObject);
            count++;
        }
    }
    public IEnumerator GenerateWorkers()
    {
        ResetAllImages();
        GenerateWorkersFaster();
        yield return null;
    }

    public void GenerateWorkersFaster()
    {
        GetTexture();
    }

    public void ChangeSelectedWorker(Workers worker)
    {
        selectedWorker = worker;
        foreach(Transform child in workerMain.transform.parent)
        {
            if(child.GetComponent<WorkerHolderProduction>().worker == worker)
            {
                startToWorkButton.gameObject.SetActive(true);
                increaseDecreaseButtons.SetActive(true);
               child.GetComponent<WorkerHolderProduction>().selectedObj.SetActive(true); 
            }
            else
            {
               child.GetComponent<WorkerHolderProduction>().selectedObj.SetActive(false); 

            }
        }
        UpdateAmountToWorkOnWorkerChange();
        UpdateWorkerEfficiencyText();
    }

    public void UpdateWorkerEfficiencyText()
    {
        WorkerEfficiencyText.text = "";
    //     if(field.productItem == "wood")
    //    {
    //     WorkerEfficiencyText.text = selectedWorker.woodWorkSpeed.ToString() + " Wood Per 10 Minutes";
    //    }
    //    if(field.productItem == "stone")
    //    {
    //     WorkerEfficiencyText.text = selectedWorker.stoneWorkSpeed.ToString() + " Stone Per 10 Minutes";
    //    }
    //    if(field.productItem == "peridotShard")
    //    {
    //     WorkerEfficiencyText.text = selectedWorker.peridotWorkSpeed.ToString() + " Peridot Per 10 Minutes";
    //    } 
    }


     public void GetTexture() {
         for(int i = 0; i<Constants.workerSprites.Count; i++)
         {
             if(Constants.workersData[i].onWork == false && Constants.workersData[i].currentStamina > 0)
             {
                Sprite mySprite = Constants.workerSprites[i];
                GameObject newImage = Instantiate(workerMain.gameObject,workerMain.transform.parent);
                newImage.GetComponent<WorkerHolderProduction>().workerImage.sprite = mySprite;
                WorkerHolderProduction workerHolder = newImage.GetComponent<WorkerHolderProduction>();
                workerHolder.popUpWorkProductionController = this;
                workerHolder.worker = new Workers();
                workerHolder.worker = Constants.workersData[i];
                newImage.SetActive(true);
             }
         }
       
    }

 
}
