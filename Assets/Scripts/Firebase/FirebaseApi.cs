using System.Collections;
using System.Collections.Generic;
using Firebase.Functions;
using UnityEngine;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;

public  class FirebaseApi : MonoBehaviour
{
    public GameManager gm;
    public PopUpController popUpController;
    public TownManager tm;
    public UIController uIController;


    public void WorkerToWork(string zoneDocId,string fieldDocId,string workerDocId,string times)
    {
        //         gm.OpenCloseLoadingBar(true);

        //  var functions = FirebaseFunctions.DefaultInstance;
        // var data = new Dictionary<string,object>();
        // data["zoneDocId"] = zoneDocId;
        // data["fieldDocId"] = fieldDocId;
        // data["workerDocId"] = workerDocId;
        // data["times"] = times;

        // var function = functions.GetHttpsCallable("workerToWork");
        // function.CallAsync(data).ContinueWithOnMainThread( (task) => {
        //    if(task.IsCompleted)
        //    {
        //         string x =  JsonConvert.SerializeObject(task.Result.Data);
        //         Debug.Log(x);
        //        gm.OpenCloseLoadingBar(false);
        //    }
        // });


         var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        String x = "6sl3ScM9S2g5hj9EHFKh";
        data["workerWorkDocId"] = x;

        var function = functions.GetHttpsCallable("workerToWorkCheck");
        function.CallAsync(data).ContinueWithOnMainThread( (task) => {
           if(task.IsCompleted)
           {
                string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);
           }
        });
    }

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
                        Constants.currentUser.workerHomeLevel +=1;
                    }
                    if(buildingName.ToString() == "warriorBuilding")
                    {
                        Constants.currentUser.warriorBuildingLevel +=1;
                    }
                    if(buildingName.ToString() == "workerBuilding")
                    {
                        Constants.currentUser.workerBuildingLevel +=1;
                        gm.workerBuildingController.GenerateWorkerCreate();

                    }
                    if(buildingName.ToString() == "mainTower")
                    {
                        Constants.currentUser.mainTowerLevel +=1;
                    }     
                        gm.tm.GenerateTown();    
                        gm.tm.CloseAllOnClicks();
                        break;
                    }
                }
           }
        });
    }

    public void UpgradeBuildingTimer(string buildingName,int requiredWood,int requiredStone)
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
                Dictionary<string,object> buildingUpgradeData = JsonConvert.DeserializeObject<Dictionary<string, object>>(x);   

       
                    BuildingUpgrade newBuildingData = new BuildingUpgrade();

                    DateTime date = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(buildingUpgradeData["endTime"].ToString()));

                    DateTime startDate = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(buildingUpgradeData["startTime"].ToString()));

                    newBuildingData.endTime = date;
                    newBuildingData.startTime = startDate;

                    if( buildingUpgradeData["buildingName"].ToString() == "stoneDeposit")
                    {
                        newBuildingData.buildingName = Building.stoneDeposit;
                    }
                    if( buildingUpgradeData["buildingName"].ToString() == "woodDeposit")
                    {
                        newBuildingData.buildingName = Building.woodDeposit;
                    }
                    if( buildingUpgradeData["buildingName"].ToString() == "workerHome")
                    {
                        newBuildingData.buildingName = Building.workerHome;
                    }
                    if(buildingUpgradeData["buildingName"].ToString() == "warriorBuilding")
                    {
                        newBuildingData.buildingName = Building.warriorBuilding;
                    }
                    if(buildingUpgradeData["buildingName"].ToString() == "workerBuilding")
                    {
                        newBuildingData.buildingName = Building.workerBuilding;
                    }
                    if(buildingUpgradeData["buildingName"].ToString() == "mainTower")
                    {
                        newBuildingData.buildingName = Building.mainTower;
                    }
                     
                Constants.currentUser.buildingUpgrades.Add(newBuildingData);
                foreach(BuildingController buildingController in FindObjectsOfType<BuildingController>())
                {
                    if(buildingController.buildingType ==newBuildingData.buildingName )
                    {
                        buildingController.CheckHasUpgrade();
                        break;
                    }
                }

                Constants.currentUser.woodCount -= requiredWood;
                Constants.currentUser.stoneCount -= requiredStone;
                uIController.GenerateUserData();
                popUpController.CloseAllPops();
                gm.OpenCloseLoadingBar(false);
                gm.tm.CloseAllOnClicks();
                popUpController.OpenInfoPop("Upgrade Started!");
           }
        });
    }

    public void DestroyWorker(string workerDocId,int summonCrystal,int blessedSummonCrystal,int legendarySummonCrystal)
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

                Dictionary<string,string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(x);   
                if(res["result"] == "success")
                {


                for(int i = 0; i<Constants.currentUser.workers.Count; i++)
                {
                    if(Constants.currentUser.workers[i].docId == workerDocId)
                    {
                        Constants.currentUser.workers.RemoveAt(i);
                        break;
                    }
                }
                Constants.currentUser.inventoryItems.blessedSummonCrystal -= blessedSummonCrystal;
                Constants.currentUser.inventoryItems.summonCrystal -= summonCrystal;
                Constants.currentUser.inventoryItems.legendarySummonCrystal -= legendarySummonCrystal;
                gm.workerInventoryController.DestroySpecificWorker(workerDocId);
                for(int i = 0; i< Constants.workersData.Count; i++)
                {
                        if(Constants.workersData[i].docId == workerDocId)
                        {
                            Constants.workersData.RemoveAt(i);
                            Constants.workerSprites.RemoveAt(i);
                        }
                }
                gm.inventoryController.GenerateItems();
                popUpController.CloseAllPops();
                gm.OpenCloseLoadingBar(false);
                gm.tm.CloseAllOnClicks();
                popUpController.OpenInfoPop("Worker Destroyed!");
                }
                else
                {
                popUpController.CloseAllPops();
                gm.OpenCloseLoadingBar(false);
                gm.tm.CloseAllOnClicks();
                popUpController.OpenInfoPop("Erorr");
                }
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
