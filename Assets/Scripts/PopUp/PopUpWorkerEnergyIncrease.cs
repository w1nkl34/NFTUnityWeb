using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWorkerEnergyIncrease : MonoBehaviour
{
    Workers worker;
    public GameObject itemParent;
    public Text staminaText;
    public Text itemCountText;
    public GameManager gm;
    public InventoryItems selectedInventoryItem;
    public int increaseTempAmount = 1;
    public GameObject increaseButtons;

    public Button applyButton;

    public Button increaseButton;

    public Button decreaseButton;
    PopUpWorkerDetail popUpWorkerDetail;

    public void ClosePop()
   {
        StopAllCoroutines();
        ResetData();
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
        gameObject.SetActive(false);
   }

   public void GenerateData(Workers worker,GameManager gm,PopUpWorkerDetail popUpWorkerDetail)
   {
       this.popUpWorkerDetail = popUpWorkerDetail;
       this.gm = gm;
       this.worker = worker;           
       ResetData();
       GetProductItemsToFeed();
       staminaText.text = "Current Stamina: " +  worker.currentStamina.ToString() + "/" + worker.stamina.ToString();
   }

   public void GetProductItemsToFeed()
   {
       foreach(InventoryItems item in Constants.allInventoryItems)
       {
           if(item.type == "workerEnergyConsumable")
           {
               int myItemIndex = 0;
               foreach(InventoryItems myItem in Constants.currentUser.inventoryItems)
               {
                   if(myItem.itemName == item.itemName)
                   {
                       if(myItem.amount > 0)
                       {
                       Constants.currentUser.inventoryItems[myItemIndex].energyIncrease = item.energyIncrease;
                       InstantiateItem(myItemIndex);
                       break;
                       }
                   }
                   myItemIndex++;
               }
           }
       }
   }
   public void ResetData()
   {
       applyButton.interactable = false;
       increaseTempAmount = 1;
       increaseButtons.SetActive(false);
       selectedInventoryItem = null;
       itemCountText.text = "Select Item";
       staminaText.text = "";
        int index = 0;
        foreach(Transform child in itemParent.transform)
        {
            if(index != 0)
            Destroy(child.gameObject);
            index++;
        }
   }

   public void IncreaseAmount()
   {
       if(increaseTempAmount < selectedInventoryItem.amount && worker.currentStamina  + (increaseTempAmount * selectedInventoryItem.energyIncrease) < worker.stamina)
       {
           increaseTempAmount++;
       }
       ChangeTextes();
   }

    public void MaxAmount()
   {
       for(int i = 1; i<=selectedInventoryItem.amount; i++)
       {
            if(increaseTempAmount < selectedInventoryItem.amount && worker.currentStamina  + (increaseTempAmount * selectedInventoryItem.energyIncrease) < worker.stamina)
            {
                increaseTempAmount++;
            }
       }
        ChangeTextes();
   }


   public void DecreaseAmount()
   {
        if(increaseTempAmount > 1 )
       {
           increaseTempAmount--;
       }
       ChangeTextes();
   }

   public void MinAmount()
   {
        increaseTempAmount = 1;
        ChangeTextes();
   }

    public void ChangeTextes()
    {
        itemCountText.text = "x" + increaseTempAmount.ToString();
       if(worker.currentStamina  + (increaseTempAmount * selectedInventoryItem.energyIncrease) >= worker.stamina)
        staminaText.text =  "Current Stamina: " +  worker.stamina.ToString() + 
        "/" + worker.stamina.ToString() + " (+" + (increaseTempAmount * selectedInventoryItem.energyIncrease).ToString() +")";
        else
       staminaText.text = "Current Stamina: " +  (worker.currentStamina  +
        (increaseTempAmount * selectedInventoryItem.energyIncrease)).ToString() + "/" + worker.stamina.ToString() + " (+" + (increaseTempAmount * selectedInventoryItem.energyIncrease).ToString() +")";
    
       if(worker.currentStamina  + (increaseTempAmount * selectedInventoryItem.energyIncrease) >= worker.stamina)
        {
            increaseButton.interactable = false;
        }
        else
        {
            increaseButton.interactable = true;
        }

        if(increaseTempAmount == 1)
        {
            decreaseButton.interactable = false;
        }
        else
        {
            decreaseButton.interactable = true;
        }
    }

   public void ChangeSelectedItem(InventoryItems inventoryItems)
   {
        increaseButtons.SetActive(true);
        applyButton.interactable = true;
       this.selectedInventoryItem = inventoryItems;
       foreach(Transform child in itemParent.transform)
        {
            if(child.GetComponent<ItemHolderWorkerEnergy>().inventoryItem == selectedInventoryItem)
            {
               
               child.GetComponent<ItemHolderWorkerEnergy>().selectedObj.SetActive(true); 
            }
            else
            {
               child.GetComponent<ItemHolderWorkerEnergy>().selectedObj.SetActive(false); 

            }
        }
        increaseTempAmount = 1;
        ChangeTextes();
   }
    public void InstantiateItem(int itemIndex)
    {
         GameObject newItem = Instantiate(itemParent.transform.GetChild(0).gameObject,itemParent.transform.GetChild(0).gameObject.transform.parent);
            string productName = Constants.currentUser.inventoryItems[itemIndex].itemName;
            int productAmount = Constants.currentUser.inventoryItems[itemIndex].amount;
                string iconUrl = "";
                foreach(InventoryItems allItem in Constants.allInventoryItems)
                {
                    if(allItem.itemName == productName)
                    {
                        iconUrl = allItem.iconUrl;
                        break;
                    }
                }
                newItem.GetComponent<ItemHolderWorkerEnergy>().productImage.sprite = gm.publicImages.loadingSprite;
                newItem.GetComponent<ItemHolderWorkerEnergy>().inventoryItem =  Constants.currentUser.inventoryItems[itemIndex];
                newItem.GetComponent<ItemHolderWorkerEnergy>().productAmount.text = "x" + productAmount.ToString();
                gm.publicImages.StartCoroutine(gm.publicImages.GetTexture(iconUrl,newItem.GetComponent<ItemHolderWorkerEnergy>().productImage));  
                newItem.gameObject.SetActive(true);
    }
   public void Apply()
   {
       FindObjectOfType<FirebaseApi>().IncreaseWorkerStamina(increaseTempAmount,selectedInventoryItem.itemName,worker.docId,popUpWorkerDetail);
       ClosePop();
   }

}
