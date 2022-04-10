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

   public void OpenSummonWorkerPopUp(string title,int index,List<Dictionary<string,object>> input)
   {
      if(Constants.currentUser.workers.Count < Constants.currentUser.workerHomeLevel)
      FindObjectOfType<PopUpController>().OpenSummonWorker(title,index,input);
      else
      FindObjectOfType<PopUpController>().OpenSummonWorkerDecline();

   }

   public void GenerateWorkerCreate()
   {
      ResetAllItems();
      GenerateWorkerCreationList();
   }

   public void GenerateWorkerCreationList()
   {
      for(int a = 0; a<Constants.allBuildings.Count; a++)
      {
         if(Constants.allBuildings[a].buildingName == "workerBuilding")
         {
            for(int i = 0; i<Constants.allBuildings[a].levels.Count; i++)
            {
               inputText.text = "";
               outputText.text = "Result : ";
               int keyIndex = 0;
              for(int j = 0; j<Constants.allBuildings[a].levels[i].input.Count; j++)
              {
                 string key = "";
                 string value = "";
                  foreach(KeyValuePair<string, object> entry in Constants.allBuildings[a].levels[i].input[j])
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
               for(int j = 0; j<Constants.allBuildings[a].levels[i].output.Count; j++)
              {
                   string key = "";
                 string value = "";
                 
                  foreach(KeyValuePair<string, object> entry in Constants.allBuildings[a].levels[i].output[j])
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
              
              int requiredLevelInt = Constants.allBuildings[a].levels[i].level;
              string idString = Constants.allBuildings[a].levels[i].id;
              List<Dictionary<string,object>> inputMap = Constants.allBuildings[a].levels[i].input;
              id.text = Constants.allBuildings[a].levels[i].id;
              requiredLevel.text = requiredLevelInt.ToString();
               GameObject newWorkerList = Instantiate(createWorker,createWorker.transform.parent);
               newWorkerList.SetActive(true);
               if(Constants.currentUser.workerBuildingLevel >= requiredLevelInt)
               {  
               newWorkerList.GetComponent<WorkerCreateHolder>().onClickCreateButton.onClick.AddListener(delegate{OpenSummonWorkerPopUp
               (idString,requiredLevelInt,inputMap);});
               } 
               else
               {
                  newWorkerList.GetComponent<WorkerCreateHolder>().onClickCreateButton.gameObject.SetActive(false); 
               }

            }
         }
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
