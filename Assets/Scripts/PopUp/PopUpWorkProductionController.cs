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
    public GameObject productParent;
    public GameObject extraProductParent;
    public GameObject extraItemsTextGameObject;
    public Workers selectedWorker = null;
    public Button startToWorkButton;
    public GameObject onWorkTimer;
    public Text WorkerEfficiencyText;
    public Text WorkerEfficiencyTotalText;
    public GameObject increaseDecreaseButtons;
    public GameObject increaseDecreaseButtonsOnWork;
    bool dataReceived = false;

    public GameObject workersLoadingText;

    WorkerWork workerWork;

    FieldController fieldController;

    DateTime beginStartTime;
    DateTime endTime;
    DateTime startTime;

    public Image upgradeSlider;
    public Text upgradeTimeText;

    public int amountToWork = 1;

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
        if(!fieldController.thisIsOnQuery)
        FindObjectOfType<FirebaseApi>().WorkerToWork(zone.docId,field.docId,selectedWorker.docId,amountToWork.ToString());
    }

    public void OpenPopUpIncreaseDecrease()
    {
        gm.popUpController.OpenPopUpWorkIncreaseDecreaseTimeController(workerWork,zone.docId,field.docId);
    }


    public void RemoveWorkerWorkTime()
    {
        if(!fieldController.thisIsOnQuery)
        FindObjectOfType<FirebaseApi>().CancelWorkerWork(workerWork.workerWorkDocId,workerWork.workerDocId,zone.docId,field.docId);
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

    public void InstantiateProduct(string productName)
    {
            GameObject newItem = Instantiate(productParent.transform.GetChild(0).gameObject,productParent.transform.GetChild(0).gameObject.transform.parent);
                newItem.GetComponent<ProductHolder>().productName.text = productName;

            
                string iconUrl = "";
                foreach(InventoryItems allItem in Constants.allInventoryItems)
                {
                    if(allItem.itemName == productName)
                    {
                        iconUrl = allItem.iconUrl;
                        break;
                    }
                }
                newItem.GetComponent<ProductHolder>().productImage.sprite = gm.publicImages.loadingSprite;
                gm.publicImages.StartCoroutine(gm.publicImages.GetTexture(iconUrl,newItem.GetComponent<ProductHolder>().productImage));

               
                newItem.gameObject.SetActive(true);
    }


    public void InstantiateExtraProduct(string productName)
    {
            GameObject newItem = Instantiate(extraProductParent.transform.GetChild(0).gameObject,extraProductParent.transform.GetChild(0).gameObject.transform.parent);
                newItem.GetComponent<ProductHolder>().productName.text = productName;

                string iconUrl = "";
                foreach(InventoryItems allItem in Constants.allInventoryItems)
                {
                    if(allItem.itemName == productName)
                    {
                        iconUrl = allItem.iconUrl;
                        break;
                    }
                }
                newItem.GetComponent<ProductHolder>().productImage.sprite = gm.publicImages.loadingSprite;
                gm.publicImages.StartCoroutine(gm.publicImages.GetTexture(iconUrl,newItem.GetComponent<ProductHolder>().productImage));
                newItem.gameObject.SetActive(true);
    }

    public void ResetGenerateProductIcons()
    {
        int productIndex = 0;
       foreach(Transform child in productParent.transform)
       {
           if(productIndex != 0)
           {
               Destroy(child.gameObject);
           }
           productIndex++;
       }

        int extraProductIndex = 0;
        foreach(Transform child in extraProductParent.transform)
       {
           if(extraProductIndex != 0)
           {
               Destroy(child.gameObject);
           }
           extraProductIndex++;
       }
    }

    public void ResetAndGenerateProductIcons()
    {

        int productIndex = 0;
       foreach(Transform child in productParent.transform)
       {
           if(productIndex != 0)
           {
               Destroy(child.gameObject);
           }
           productIndex++;
       }

        int extraProductIndex = 0;
        foreach(Transform child in extraProductParent.transform)
       {
           if(extraProductIndex != 0)
           {
               Destroy(child.gameObject);
           }
           extraProductIndex++;
       }


        if(field.extraProductItem.Count == 0)
        {
           extraProductParent.SetActive(false);   
            extraItemsTextGameObject.SetActive(false);
        }
       else
       {
            extraItemsTextGameObject.SetActive(true);
            extraProductParent.SetActive(true);
       }

       



         for(int i = 0; i<field.productItem.Count; i++)
       {
           foreach(KeyValuePair<string,object> item in field.productItem[i])
           {
               InstantiateProduct(item.Key);
           }
       }
        for(int i = 0; i<field.extraProductItem.Count; i++)
       {
           foreach(KeyValuePair<string,object> item in field.extraProductItem[i])
           {
               InstantiateExtraProduct(item.Key);
           }
       }
    }

   public void GenerateProduct(Field field,Zones zone,WorkerWork workerWork,FieldController fieldController)
   {
        ResetData();
        this.fieldController = fieldController;
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

        ResetAndGenerateProductIcons();
       
    if(!onWork)
    {
       StartCoroutine(GenerateWorkers());
       increaseDecreaseButtonsOnWork.gameObject.SetActive(false);
        onWorkTimer.SetActive(false);
    }
    if(onWork)
    {
        CheckWorkerWork();
        if(DateTime.Compare(DateTime.UtcNow ,endTime) <= 0 )
        {
            StartCoroutine(GenerateWorkingWorkers());
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
        beginStartTime = workerWork.startTime;
        startTime = workerWork.startTime.AddMinutes(2*workerWork.currentTime);
        endTime = workerWork.startTime.AddMinutes(2*workerWork.times);
    }
   public void Update()
   {
       if(onWork && dataReceived)
       {
           WorkerEfficiencyTotalText.text =  workerWork.currentTime.ToString() + "/" + workerWork.times.ToString();
            TimeSpan ts1 =  endTime - beginStartTime;
            TimeSpan ts = endTime - DateTime.UtcNow;
            upgradeSlider.fillAmount = 1 - ((float)ts.TotalSeconds / (float)ts1.TotalSeconds);
            upgradeTimeText.text = string.Format("{1}:{2}:{3}", ts.Days, (ts.Days*24) + ts.Hours, ts.Minutes, ts.Seconds);
       }
        if(DateTime.Compare(DateTime.UtcNow ,endTime) > 0 &&onWork && dataReceived)
        {
           upgradeTimeText.text = "Returning...";
           onWork = false;
           OnWorkFinishedLocally();
           gm.popUpController.OpenInfoPop("Work Finished!");
           ClosePop();
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
        StopAllCoroutines();
        ResetGenerateProductIcons();
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
        if(Constants.allWorkersLoaded == true)
        {
            workersLoadingText.SetActive(false);
            ResetAllImages();
            GenerateWorkersFaster();
            yield return null;
        }
        else
        {
            workersLoadingText.SetActive(true);
            yield return StartCoroutine(AwaitCor());
        }
    }

      public IEnumerator GenerateWorkingWorkers()
    {
        if(Constants.allWorkersLoaded == true)
        {
            workersLoadingText.SetActive(false);
            ResetAllImages();
            GenerateWorkingWorkersFaster();
            yield return null;
        }
        else
        {
            workersLoadingText.SetActive(true);
            yield return StartCoroutine(AwaitWorkingCor());
        }
    }

    public void GenerateWorkersFaster()
    {
        GetTexture();
    }

    public void GenerateWorkingWorkersFaster()
    {
        GetWorkingTexture();
    }

    public IEnumerator AwaitWorkingCor()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenerateWorkingWorkers());
    }

    public IEnumerator AwaitCor()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GenerateWorkers());
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
    }

     public void GetWorkingTexture() {
         for(int i = 0; i<Constants.workerSprites.Count; i++)
         {
             if(Constants.workersData[i].docId == workerWork.workerDocId)
             {
                Sprite mySprite = Constants.workerSprites[i];
                GameObject newImage = Instantiate(workerMain.gameObject,workerMain.transform.parent);
                newImage.GetComponent<WorkerHolderProduction>().workerImage.sprite = mySprite;
                WorkerHolderProduction workerHolder = newImage.GetComponent<WorkerHolderProduction>();
                workerHolder.popUpWorkProductionController = this;
                workerHolder.worker = new Workers();
                workerHolder.worker = Constants.workersData[i];
                newImage.GetComponent<WorkerHolderProduction>().workerImage.GetComponent<Button>().enabled = false;
                newImage.SetActive(true);
                break;
             }
         }
       
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
