using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class WorkerBuildingController : MonoBehaviour
{
   public GameObject createWorker;
   public Text inputText;
   public Text outputText;
   public Text requiredLevel;
   public Text id;
   public void CreateWorkerRequest(int index)
   {
      FindObjectOfType<ReactSend>().CreateWorkerCall(index);
   }

   public void OnEnable()
   {
      ResetAllItems();
      GenerateWorkerCreationList();
   }

   public void GenerateWorkerCreationList()
   {
      int index = 0;
      foreach(Buildings building in Constants.allBuildings)
      {
         if(building.buildingName == "workerBuilding")
         {
            for(int i = 0; i<building.levels.Count; i++)
            {
               inputText.text = "";
               outputText.text = "Result : ";
               int keyIndex = 0;
              for(int j = 0; j<Constants.allBuildings[index].levels[i].input.Count; j++)
              {
                 string key = "";
                 string value = "";
                  foreach(KeyValuePair<string, object> entry in Constants.allBuildings[index].levels[i].input[j])
                  {
                     if(keyIndex == j)
                     {
                     key = entry.Key;
                     value = entry.Value.ToString();
                     break;
                     }
                  }
                  keyIndex++;
                 inputText.text = inputText.text + key + ":" + value;
              }   
              keyIndex = 0;
               for(int j = 0; j<Constants.allBuildings[index].levels[i].output.Count; j++)
              {
                   string key = "";
                 string value = "";
                 
                  foreach(KeyValuePair<string, object> entry in Constants.allBuildings[index].levels[i].output[j])
                  {
                     if(keyIndex == j)
                     {
                     key = entry.Key;
                     value = entry.Value.ToString();
                     break;
                     }
                  }
                  keyIndex++;
                 outputText.text = outputText.text + key + ":" + "%" + value;
              }
              
              int requiredLevelInt = Constants.allBuildings[index].levels[i].level;
              id.text = Constants.allBuildings[index].levels[i].id;
              requiredLevel.text = requiredLevelInt.ToString();
               GameObject newWorkerList = Instantiate(createWorker,createWorker.transform.parent);
               newWorkerList.SetActive(true);
               newWorkerList.GetComponent<WorkerCreateHolder>().onClickCreateButton.onClick.AddListener(delegate{CreateWorkerRequest(requiredLevelInt);});
            }
         }
         index++;
      }
   }

   public void ResetAllItems()
    {
        int count = 0;
        foreach(Transform child in createWorker.transform.parent)
        {
            if(count != 0)
            Destroy(child.gameObject);
            count++;
        }
    }
}
