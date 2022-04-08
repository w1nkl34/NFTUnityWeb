using System.Collections;
using System.Collections.Generic;
using Firebase.Functions;
using UnityEngine;
using Firebase.Extensions;
using Newtonsoft.Json;

public  class FirebaseApi : MonoBehaviour
{
    public GameManager gm;
    public PopUpController popUpController;
    public TownManager tm;

     public void CheckUpgradeBuildingFinished(Building buildingName) {
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["buildingName"] = buildingName.ToString();
        var function = functions.GetHttpsCallable("checkIfBuildingUpgradeIsFinished");
        function.CallAsync(data).ContinueWithOnMainThread( (task) => {
           if(task.IsCompleted)
           {
                string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);

                for(int i = 0; i<Constants.currentUser.buildingUpgrades.Count; i++)
                {

                    if(Constants.currentUser.buildingUpgrades[i].buildingName == buildingName)
                    {
                    Constants.currentUser.buildingUpgrades.RemoveAt(i);

                    if( buildingName.ToString() == "stoneDeposit")
                    {
                        Constants.currentUser.stoneDepositLevel +=1;
                    }
                    if(buildingName.ToString()== "woodDeposit")
                    {
                        Constants.currentUser.woodDepositLevel +=1;
                    }
                    if(buildingName.ToString() == "workerHome")
                    {
                        Constants.currentUser.workerCapacity +=1;
                    }
                    if(buildingName.ToString() == "warriorBuilding")
                    {
                        Constants.currentUser.warriorBuildingLevel +=1;
                    }
                    if(buildingName.ToString() == "workerBuilding")
                    {
                        Constants.currentUser.workerBuildingLevel +=1;
                    }
                    if(buildingName.ToString() == "mainTower")
                    {
                        Constants.currentUser.mainTowerLevel +=1;
                    }     
                        tm.GenerateTown();
                        break;
                    }
                }
           }
        });
    }

    public void UpgradeBuildingTimer(string buildingName)
    {
        gm.OpenCloseLoadingBar(true);
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["buildingName"] = buildingName;
        var function = functions.GetHttpsCallable("upgradeBuildingTimer");
       function.CallAsync(data).ContinueWithOnMainThread( (task) => {
           if(task.IsCompleted)
           {
                string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);
                popUpController.CloseAllPops();
                gm.OpenCloseLoadingBar(false);
                gm.tm.CloseAllOnClicks();
                popUpController.OpenInfoPop("Upgrade Started!");
           }
        });
    }

    public void DestroyWorker(string workerDocId)
    {

       gm.OpenCloseLoadingBar(true);
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["workerDocId"] = workerDocId;
        var function = functions.GetHttpsCallable("destroyWorker");
       function.CallAsync(data).ContinueWithOnMainThread((task) => {
           if(task.IsCompleted)
           {
                 string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);
               StartCoroutine(GetUserData("Worker Destroyed!"));
           }
        });  
    }

    public void CreateWorker(int index)
    {
         gm.OpenCloseLoadingBar(true);
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["index"] = index;
        var function = functions.GetHttpsCallable("createWorker");
       function.CallAsync(data).ContinueWithOnMainThread( (task) => {
           if(task.IsCompleted)
           {
                string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);
                StartCoroutine(GetUserData("Worker Created!"));
           }
        });
    }

    
    public  IEnumerator GetUserData(string message) {
        var functions = FirebaseFunctions.DefaultInstance;
        var function = functions.GetHttpsCallable("getOnlyUserData");
        var task = function.CallAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
           
        }
        else
        {
            gm.GetUser(task.Result.Data.ToString());
            popUpController.CloseAllPops();
            gm.OpenCloseLoadingBar(false);
            gm.tm.CloseAllOnClicks();
            popUpController.OpenInfoPop(message);
        }
    }
}
