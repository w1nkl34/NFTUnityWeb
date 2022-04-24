using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldController : MonoBehaviour
{

    GameManager gm;
    public Field field;
    public Zones zone;
    bool isOnWork = false;
    public GameObject UpgradeLeft;
    public Image upgradeSlider;
    public Text upgradeTimeText;
    public bool startCounter = false;
    DateTime endTime;
    DateTime startTime;
    DateTime totalEndTime;
    public bool upgradeFinished = false;
    public WorkerWork workerWork;
    public bool thisIsOnQuery = false;
    bool foundAnyWork = false;
    public Dictionary<string,int> rewardPool = new Dictionary<string,int>();
    public GameObject rewardPoolObject;
    public bool rewardAwaits = false;

    public void OpenRewardPoolObject()
    {
        rewardAwaits = true;
        if(GetComponent<Image>().enabled == true)
        rewardPoolObject.SetActive(true);
    }

    public void OpenRewardPoolPop()
    {
        if(!thisIsOnQuery)
        {
            rewardAwaits = false;
            rewardPoolObject.SetActive(false);
            gm.popUpController.OpenPopUpRewardPool(rewardPool);
            rewardPool = new Dictionary<string, int>();
        }
    }


    public void CheckWorkerWork()
    {
        foundAnyWork = false;
        for(int i = 0; i<Constants.currentUser.workerWorks.Count; i++)
        {
            if(Constants.currentUser.workerWorks[i].fieldDocId == field.docId && Constants.currentUser.workerWorks[i].zoneDocId == zone.docId)
            {   
                foundAnyWork = true;
                this.workerWork = Constants.currentUser.workerWorks[i];
                isOnWork = true;
                startCounter = true;
                upgradeFinished = false;
                startTime = (workerWork.startTime).AddMinutes(2*workerWork.currentTime);
                endTime = startTime.AddMinutes(2);
                totalEndTime = workerWork.startTime.AddMinutes(2*workerWork.times);
                UpgradeLeft.SetActive(true);
            }
        }
        if(foundAnyWork == false && UpgradeLeft != null)
        {
            UpgradeLeft.SetActive(false);
        }
    }

    public void IncreaseCurrentTime()
    {
         for(int i = 0; i<Constants.currentUser.workerWorks.Count; i++)
        {
            if(Constants.currentUser.workerWorks[i].fieldDocId == field.docId && Constants.currentUser.workerWorks[i].zoneDocId == zone.docId)
            {
                Constants.currentUser.workerWorks[i].currentTime++; 
            }
        }
    }


    public void Update()
    {
        if(DateTime.Compare(DateTime.UtcNow,totalEndTime) < 0 )
        {
                TimeSpan ts1 =  totalEndTime - workerWork.startTime;
                TimeSpan ts = totalEndTime - DateTime.UtcNow;
                upgradeSlider.fillAmount = 1 - ((float)ts.TotalSeconds / (float)ts1.TotalSeconds);
                upgradeTimeText.text = string.Format("{1}:{2}:{3}", ts.Days, (ts.Days*24) + ts.Hours, ts.Minutes, ts.Seconds) 
                + " " + workerWork.currentTime.ToString() + "/" +workerWork.times.ToString() ;
        }
        if(DateTime.Compare(DateTime.UtcNow ,endTime) > 0 && !upgradeFinished && startCounter && workerWork.times > workerWork.currentTime &&!thisIsOnQuery)
        {
            // if(DateTime.Compare(DateTime.UtcNow,totalEndTime) < 0)
            //     upgradeTimeText.text = "Loading...";
            Debug.Log("CheckWorkerWork");
            upgradeFinished = true;
            thisIsOnQuery = true;
            StartCoroutine(CheckTimeFinish(workerWork.workerWorkDocId));
        }
          if(DateTime.Compare(DateTime.UtcNow,totalEndTime) > 0 &&thisIsOnQuery)
          {
            upgradeSlider.fillAmount = 1;
            upgradeTimeText.text = "Returning...";
          }
    }

    public IEnumerator CheckTimeFinish(String docId)
    {
        yield return StartCoroutine(FindObjectOfType<FirebaseApi>().CheckWorkerWorkFinishCor(docId,this));
        startTime = endTime;
        UpgradeLeft.SetActive(false);
        startCounter = false;
        CheckWorkerWork();
        thisIsOnQuery = false;
    }

    public void Awake()
    {
        StartField();
    }

    public void StartField()
    {
        ResetAllData();
        CheckWorkerWork();
        gm = FindObjectOfType<GameManager>();
        if(field.fieldType == "production")
        GetComponent<Image>().sprite = gm.wm.productionImage;
        else
        GetComponent<Image>().sprite = gm.wm.battleImage;
  
    }

    public void GenerateProduct()
    {
       gm.popUpController.popUpWorkProductionController.GenerateProduct(field,zone,workerWork,this);
    }

    public void ResetAllData()
    {
        upgradeFinished = true;
        if(UpgradeLeft != null)
        UpgradeLeft.SetActive(false);
        startTime = endTime;
        startCounter = false;

    }
    public void OpenProductDetails()
    {
        if(thisIsOnQuery == false)
        {
        if(field.fieldType == "production")
        {
        gm.popUpController.OpenProductionPop();
        gm.popUpController.popUpWorkProductionController.GenerateProduct(field,zone,workerWork,this);
        }
        else
        {
        gm.popUpController.OpenBattlePop();
        }
        }

    }

    
}
