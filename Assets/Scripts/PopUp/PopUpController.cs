using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopUpController : MonoBehaviour
{
    public Text summonWorkerTitleText;
    public Button summonWorkerButton;
    public GameObject summonWorkerRequiredItems;
    public GameObject summonWorkerRequiredItemsTextGameObject;
    public GameObject summonWorkerMain;

    
    public void CloseSummonWorker()
    {
        summonWorkerMain.SetActive(false);
    }

    public void ResetSummonWorkerRequiredItems()
    {
        foreach(Transform child in summonWorkerRequiredItems.transform.parent)
        {
            if(child.gameObject != summonWorkerRequiredItems && child.gameObject != summonWorkerRequiredItemsTextGameObject)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void OpenSummonWorker(string title,int index,List<Dictionary<string,object>> input)
    {
        ResetSummonWorkerRequiredItems();
        summonWorkerTitleText.text = title;
        int keyIndex = 0;
        bool error = false;

        for(int j = 0; j<input.Count; j++)
        {       
            string key = "";
            string value = "";      
            string myValue = "";    
            foreach(KeyValuePair<string, object> entry in input[j])
            {
                if(keyIndex == j)
                {
                     key = entry.Key;
                     value = entry.Value.ToString();
                     if(key == "summonCrystal")
                     {
                         myValue = Constants.currentUser.inventoryItems.summonCrystal.ToString();
                     }
                     if(key == "blessedSummonCrystal")
                     {
                         myValue = Constants.currentUser.inventoryItems.blessedSummonCrystal.ToString();
                     }
                    if(key == "legendarySummonCrystal")
                     {
                         myValue = Constants.currentUser.inventoryItems.legendarySummonCrystal.ToString();
                     }

                     if(int.Parse(myValue) < int.Parse(value))
                     {
                         error = true;
                     }
                     break;
                }
            }

            keyIndex++;
            GameObject newRequiredText = Instantiate(summonWorkerRequiredItems,summonWorkerRequiredItems.transform.parent);
            newRequiredText.GetComponent<Text>().text =  key + ": " + value + "/" + myValue; 
            newRequiredText.SetActive(true);
        }
        if(error)
        {
        summonWorkerButton.transform.GetChild(0).GetComponent<Text>().text = "Not Enough Crystals!";
        summonWorkerButton.enabled = false;
        }
        else
        {
        summonWorkerButton.transform.GetChild(0).GetComponent<Text>().text = "Summon!";
        summonWorkerButton.enabled = true;
        summonWorkerButton.onClick.AddListener(delegate{CreateWorkerRequest(index);});

        }
        summonWorkerMain.SetActive(true);
    }
    public void CreateWorkerRequest(int index)
    {
        FindObjectOfType<ReactSend>().CreateWorkerCall(index);
        summonWorkerMain.SetActive(false);
    }
}
