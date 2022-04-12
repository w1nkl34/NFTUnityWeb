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
    public Text WorkerEfficiencyText;
    public Text WorkerEfficiencyTotalText;
    public GameObject increaseDecreaseButtons;

    public int amountToWork = 1;

    public void ResetData()
    {
        increaseDecreaseButtons.SetActive(false);
        WorkerEfficiencyText.text = "Select Worker";
        WorkerEfficiencyTotalText.text = "";
        amountToWork =1;
        startToWorkButton.interactable = false;
    }

    public void IncreaseAmountToWork()
    {
     if(amountToWork < (int)selectedWorker.currentStamina)
        amountToWork++;
    UpdateWorkerEfficiencyTotalText();
    }

    public void UpdateWorkerEfficiencyTotalText()
    {
        if(field.productItem == "wood")
        WorkerEfficiencyTotalText.text = (selectedWorker.woodWorkSpeed * amountToWork).ToString() + " " + "Wood"+ " After " 
        + (10 * amountToWork).ToString() + " Minutes " + "(x" + amountToWork + ")";
        if(field.productItem == "stone")
        WorkerEfficiencyTotalText.text = (selectedWorker.stoneWorkSpeed * amountToWork).ToString() + " " + "Stone" + " After " 
        + (10 * amountToWork).ToString() + " Minutes " + "(x" + amountToWork + ")";
        if(field.productItem == "peridotShard")
        WorkerEfficiencyTotalText.text = (selectedWorker.peridotWorkSpeed * amountToWork).ToString() + " " + "Peridot" + " After " 
        + (10 * amountToWork).ToString() + " Minutes " + "(x" + amountToWork + ")";
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

   public void GenerateProduct(Field field,Zones zone)
   {
        ResetData();
        this.zone = zone;
       this.field = field;
       productTypeText.text = field.productItem;
       if(field.productItem == "wood")
       {
        productImage.sprite = gm.publicImages.woodImage;

       }
       if(field.productItem == "stone")
       {
        productImage.sprite = gm.publicImages.stoneImage;

       }
       if(field.productItem == "peridotShard")
       {
        productImage.sprite = gm.publicImages.peridotImage;   

       }

       StartCoroutine(GenerateWorkers());
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

        StopAllCoroutines();
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
                startToWorkButton.interactable = true;
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
        if(field.productItem == "wood")
       {
        WorkerEfficiencyText.text = selectedWorker.woodWorkSpeed.ToString() + " Wood Per 10 Minutes";
       }
       if(field.productItem == "stone")
       {
        WorkerEfficiencyText.text = selectedWorker.stoneWorkSpeed.ToString() + " Stone Per 10 Minutes";
       }
       if(field.productItem == "peridotShard")
       {
        WorkerEfficiencyText.text = selectedWorker.peridotWorkSpeed.ToString() + " Peridot Per 10 Minutes";
       } 
    }


     public void GetTexture() {
         for(int i = 0; i<Constants.workerSprites.Count; i++)
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
