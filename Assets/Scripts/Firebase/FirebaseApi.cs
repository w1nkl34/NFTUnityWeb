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

    public void UpdateWorkerWorkTimes(String workerWorkDocId,int times,String zoneDocId,String fieldDocId)
    {
        gm.OpenCloseLoadingBar(true);
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["workerWorkDocId"] = workerWorkDocId;
        data["updateAmount"] = times.ToString();

        var function = functions.GetHttpsCallable("updateWorkerWorkTimes");
        function.CallAsync(data).ContinueWithOnMainThread( (task) => {
            
           if(task.IsCompleted)
           {
                if(task.Result.Data.ToString() == "success")
                {
                for(int i= 0; i<Constants.currentUser.workerWorks.Count; i++)
                {
                    if(Constants.currentUser.workerWorks[i].workerWorkDocId == workerWorkDocId)
                    {
                        Constants.currentUser.workerWorks[i].times = times;
                        break;
                    }
                }
                gm.wm.GenerateSpecificZone(workerWorkDocId,zoneDocId,fieldDocId);
                gm.OpenCloseLoadingBar(false);
                }
                else
                {
                gm.OpenCloseLoadingBar(false);
                }
           }
        });
    }

   public void CancelWorkerWork(String workerWorkDocId,String workerDocId)
    {
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["workerWorkDocId"] = workerWorkDocId;
        gm.OpenCloseLoadingBar(true);
        var function = functions.GetHttpsCallable("removeWorkerWork");
        function.CallAsync(data).ContinueWithOnMainThread( (task) => {
              if(task.IsCompleted)
           {
                if( task.Result.Data.ToString() == "success")
                {
                for(int i= 0; i<Constants.currentUser.workerWorks.Count; i++)
                    {
                        if(Constants.currentUser.workerWorks[i].workerWorkDocId == workerWorkDocId)
                        {
                                Constants.currentUser.workerWorks.RemoveAt(i);
                                break;
                        }
                    }
                    for(int i = 0; i<Constants.currentUser.workers.Count; i++)
                    {
                        if(workerDocId == Constants.currentUser.workers[i].docId)
                        {
                            Constants.currentUser.workers[i].onWork = false;
                            break;
                        }
                    }
                    gm.UpdateUserLocally();
                    gm.wm.GenerateZones();
                    gm.popUpController.CloseAllPops();
                    gm.OpenCloseLoadingBar(false);
                    popUpController.OpenInfoPop("Work Declined!");
                }
                else
                {
                gm.OpenCloseLoadingBar(false);
                }
           }
        });
    }

    public void WorkerToWork(string zoneDocId,string fieldDocId,string workerDocId,string times)
    {
        gm.OpenCloseLoadingBar(true);

        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["zoneDocId"] = zoneDocId;
        data["fieldDocId"] = fieldDocId;
        data["workerDocId"] = workerDocId;
        data["times"] = times;

        var function = functions.GetHttpsCallable("workerToWork");
        function.CallAsync(data).ContinueWithOnMainThread( (task) => {

           if(task.IsCompleted)
           {
             string x =  JsonConvert.SerializeObject(task.Result.Data);
                Dictionary<string,object> work = JsonConvert.DeserializeObject<Dictionary<string, object>>(x);   
                    DateTime startDate = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(work["startTime"].ToString()));
                    WorkerWork workerWork1 = new WorkerWork();
                    workerWork1.startTime = startDate;
                     workerWork1.times = int.Parse(work["times"].ToString());
                     workerWork1.workerWorkDocId = work["workerWorkDocId"].ToString();
                      workerWork1.fieldDocId = work["fieldDocId"].ToString();
                     workerWork1.currentTime = int.Parse(work["currentTime"].ToString());
                     workerWork1.zoneDocId = work["zoneDocId"].ToString();
                     workerWork1.workerDocId = work["workerDocId"].ToString();

                      for(int i = 0; i<Constants.currentUser.workers.Count; i++)
                    {
                        if(workerDocId == Constants.currentUser.workers[i].docId)
                        {
                            Constants.currentUser.workers[i].onWork = true;
                            break;
                        }
                    }



                    Constants.currentUser.workerWorks.Add(workerWork1);
                    gm.wm.GenerateZones();
                    gm.popUpController.CloseAllPops();
                    gm.OpenCloseLoadingBar(false);
                    popUpController.OpenInfoPop("Work Started!");

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

    public void UpgradeBuildingTimer(string buildingName,float requiredWood,float requiredStone)
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
                uIController.GenerateUserData(false);
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
                bool found = false;
                for(int i = 0; i<Constants.currentUser.inventoryItems.Count; i++)
                {
                    if(Constants.currentUser.inventoryItems[i].itemName == "blessedSummonCrystal")
                    {
                       Constants.currentUser.inventoryItems[i].amount += blessedSummonCrystal;  
                        if(blessedSummonCrystal != 0)
                       found = true;
                    }
                    if(Constants.currentUser.inventoryItems[i].itemName == "summonCrystal")
                    {
                       Constants.currentUser.inventoryItems[i].amount += summonCrystal;  
                       if(summonCrystal != 0)
                        found = true;
                    }
                    if(Constants.currentUser.inventoryItems[i].itemName == "legendarySummonCrystal")
                    {
                       Constants.currentUser.inventoryItems[i].amount += legendarySummonCrystal;  
                       if(legendarySummonCrystal != 0)
                        found = true;
                    }
                }
                if(found == false)
                {
                    InventoryItems newItem = new InventoryItems();
                    if(summonCrystal != 0)
                    {
                        newItem.amount = summonCrystal;
                        newItem.itemName = "summonCrystal";
                        Constants.currentUser.inventoryItems.Add(newItem);
                    }
                    if(blessedSummonCrystal != 0)
                    {
                        newItem.amount = blessedSummonCrystal;
                        newItem.itemName = "blessedSummonCrystal";
                        Constants.currentUser.inventoryItems.Add(newItem);
                    }
                    if(legendarySummonCrystal != 0)
                    {
                        newItem.amount = legendarySummonCrystal;
                        newItem.itemName = "legendarySummonCrystal";
                        Constants.currentUser.inventoryItems.Add(newItem);
                    }
                }
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

     public IEnumerator CheckWorkerWorkFinishCor(String workerWorkDocId,FieldController fieldController)
    {

        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["workerWorkDocId"] = workerWorkDocId;

        var function = functions.GetHttpsCallable("workerToWorkCheck");
                var task = function.CallAsync(data);
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
           Debug.Log("nb");
        }
        else
        {    
                    string x =  JsonConvert.SerializeObject(task.Result.Data);
                    Debug.Log(x);
                    if(task.Result.Data.ToString() != "error")
                    {
                    Dictionary<string,object> work = JsonConvert.DeserializeObject<Dictionary<string, object>>(x);   
                    int currentTimePlus = int.Parse(work["currentTimePlus"].ToString());
                    string result = work["result"].ToString();

                    List<Dictionary<string,object>> items = JsonConvert.DeserializeObject<List<Dictionary<string,object>>>(work["items"].ToString());
                    
                    string workerDocId = "";
                    for(int i= 0; i<Constants.currentUser.workerWorks.Count; i++)
                    {
                        if(Constants.currentUser.workerWorks[i].workerWorkDocId == workerWorkDocId)
                        {
                            workerDocId = Constants.currentUser.workerWorks[i].workerDocId;
                            if(result == "finish")
                            {
                                Constants.currentUser.workerWorks.RemoveAt(i);
                                break;
                            }
                            else
                            {
                                Constants.currentUser.workerWorks[i].currentTime +=currentTimePlus;
                            }
                        }
                    }
                    for(int i = 0; i<Constants.currentUser.workers.Count; i++)
                    {
                        if(workerDocId == Constants.currentUser.workers[i].docId)
                        {
                             if(result == "finish")
                            Constants.currentUser.workers[i].onWork = false;
                            Constants.currentUser.workers[i].currentStamina -= currentTimePlus;
                            break;
                        }
                    }
                    
                    
                    foreach(Dictionary<string,object> item in items)
                    {                    
                                if(!fieldController.rewardPool.ContainsKey(item["itemName"].ToString()))
                                {       
                                    fieldController.rewardPool[item["itemName"].ToString()] = int.Parse(item["amount"].ToString());
                                }
                                else
                                {
                                    fieldController.rewardPool[item["itemName"].ToString()]+=  int.Parse(item["amount"].ToString());
                                } 
                            
                    }
                    
                    fieldController.OpenRewardPoolObject();
                    gm.UpdateUserLocally();
                    yield return null;
                    }
        }

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
