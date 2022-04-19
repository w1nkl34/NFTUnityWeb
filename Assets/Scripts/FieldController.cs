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
    public bool upgradeFinished = false;
    WorkerWork workerWork;


    public void CheckWorkerWork()
    {
        for(int i = 0; i<Constants.currentUser.workerWorks.Count; i++)
        {
            if(Constants.currentUser.workerWorks[i].fieldDocId == field.docId && Constants.currentUser.workerWorks[i].zoneDocId == zone.docId)
            {   
                this.workerWork = Constants.currentUser.workerWorks[i];
                isOnWork = true;
                startCounter = true;
                upgradeFinished = false;
                startTime = (workerWork.startTime).AddMinutes(2*workerWork.currentTime);
                endTime = startTime.AddMinutes(2);
                if(DateTime.UtcNow < ( Constants.currentUser.workerWorks[i].startTime).AddMinutes(2* Constants.currentUser.workerWorks[i].times))
                UpgradeLeft.SetActive(true);
                else
                UpgradeLeft.SetActive(false);
            }
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
        if(startCounter && !upgradeFinished)
        {
                TimeSpan ts1 =  endTime - startTime;
                TimeSpan ts = endTime - DateTime.UtcNow;
                upgradeSlider.fillAmount = 1 - ((float)ts.TotalSeconds / (float)ts1.TotalSeconds);
                upgradeTimeText.text = string.Format("{1}:{2}:{3}", ts.Days, (ts.Days*24) + ts.Hours, ts.Minutes, ts.Seconds);
        }
        if(DateTime.Compare(DateTime.UtcNow ,endTime) > 0 && !upgradeFinished && startCounter)
        {
            upgradeFinished = true;
            FindObjectOfType<FirebaseApi>().CheckWorkerWorkFinish(workerWork.workerWorkDocId);
            startTime = endTime;
            UpgradeLeft.SetActive(false);
            startCounter = false;
            IncreaseCurrentTime();
            CheckWorkerWork();
        }
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
        if(field.fieldType == "production")
        {
        gm.popUpController.OpenProductionPop();
        gm.popUpController.popUpWorkProductionController.GenerateProduct(field,zone,workerWork);
        }
        else
        {
        gm.popUpController.OpenBattlePop();
        }
    }

    
}
