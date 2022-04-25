using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject townScene;
    public UIController uIController;
    public WorkerInventoryController workerInventoryController;
    public WorkerBuildingController workerBuildingController;
    public InventoryController inventoryController;
    public GameObject loadingBar;
    public GameObject connecttingBar;
    public TownManager tm;
    public WorldManager wm;
    public CameraController cameraController;
    public PopUpController popUpController;
    public PublicImages publicImages;

    private void Awake() 
    {
        Application.targetFrameRate = 60;
    }

    public void OpenCloseConnecttingBar(bool value)
    {
        connecttingBar.SetActive(value);
    }

    public void OpenCloseLoadingBar(bool value)
    {
        loadingBar.SetActive(value);
        if(value)
        Constants.onMenu = true;
        else
        uIController.SetOnMenuToFalseCorCall();
    }

    public void OpenCloseLoadingBarOnMenuFalse(bool value)
    {
        loadingBar.SetActive(value);
    }


    public void StartTown(string json)
    {
        CreateUser(json);
        StartCoroutine(workerInventoryController.GenerateWorkers());
        workerBuildingController.GenerateWorkerCreate();
        inventoryController.GenerateItems();
        townScene.SetActive(true);
        townScene.GetComponent<TownManager>().GenerateTown();
        uIController.GenerateUserData(true);
        wm.GenerateZones();
        CheckAllBuildingsUpgrade();

    }

    public void GetUser(string json)
    {
        CreateUser(json);
        StartCoroutine(workerInventoryController.GenerateWorkers());
        workerBuildingController.GenerateWorkerCreate();
        inventoryController.GenerateItems();
        townScene.GetComponent<TownManager>().GenerateTown();
        uIController.GenerateUserData(false);
        OpenCloseLoadingBar(false);
        CheckAllBuildingsUpgrade();
     
    }

    public void UpdateUserLocally()
    {
        inventoryController.GenerateItems();
        uIController.GenerateUserData(false);
    }

    public void CheckAllBuildingsUpgrade()
    {
        foreach(BuildingController buildingController in FindObjectsOfType<BuildingController>())
        {
            buildingController.CheckHasUpgrade();
        }
    }

    public void CreateUser(string json)
    {
        
        Dictionary<string, object> jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
         Dictionary<string, object>  playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["userData"].ToString());
        int mainTowerLevel = int.Parse(playerData["mainTower"].ToString());
        int stoneDepositLevel = int.Parse(playerData["stoneDeposit"].ToString());
        int woodDepositLevel = int.Parse(playerData["woodDeposit"].ToString());
        int workerHome = int.Parse(playerData["workerHome"].ToString());
        int workerBuildingLevel = playerData.ContainsKey("workerBuilding") ? int.Parse(playerData["workerBuilding"].ToString()) : 0;
        int warriorBuildingLevel = playerData.ContainsKey("warriorBuilding") ? int.Parse(playerData["warriorBuilding"].ToString()) : 0;
        float peridotShardCount = float.Parse(playerData["peridotShard"].ToString());
        float stoneCount = float.Parse(playerData["stone"].ToString());
        float woodCount = float.Parse(playerData["wood"].ToString());

       List<Dictionary<string, object>> workersData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(playerData["workers"].ToString());   
        List<Workers> workers = new List<Workers>(); 
        foreach (Dictionary<string, object> worker in workersData)
        {
            Workers newWorker = new Workers();
            newWorker.onWork = bool.Parse(worker["onWork"].ToString());
            newWorker.level = int.Parse(worker["level"].ToString());
            newWorker.currentStamina = float.Parse(worker["currentStamina"].ToString());
            newWorker.stamina = float.Parse(worker["stamina"].ToString());
            newWorker.isNFT = bool.Parse(worker["isNFT"].ToString());

            if(worker.ContainsKey("sellPrice"))
            newWorker.sellPrice = float.Parse(worker["sellPrice"].ToString());

            newWorker.onSale = bool.Parse(worker["onSale"].ToString());
            newWorker.rarity = worker["rarity"].ToString();
            newWorker.peridotWorkSpeed = float.Parse(worker["peridotWorkSpeed"].ToString());
            newWorker.woodWorkSpeed = float.Parse(worker["woodWorkSpeed"].ToString());
            newWorker.stoneWorkSpeed = float.Parse(worker["stoneWorkSpeed"].ToString());
            newWorker.url = worker["url"].ToString();
            newWorker.type = worker["worker"].ToString();
            newWorker.docId = worker["docId"].ToString();
            newWorker.luck =  float.Parse(worker["luck"].ToString());
            workers.Add(newWorker);
        }
        List<InventoryItems> inventoryItems = new List<InventoryItems>();
        if(jsonData.ContainsKey("inventoryItemsData"))
        {
             List<Dictionary<string, object>> inventoryItemsData = 
             JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["inventoryItemsData"].ToString());   
            for(int i = 0; i< inventoryItemsData.Count; i++)
            {
                foreach(KeyValuePair<string,object> inventoryItem in inventoryItemsData[i])
                {
                    Dictionary<string,object> item = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(inventoryItem.Value.ToString());   
                    InventoryItems newItem = new InventoryItems();
                    newItem.itemName = item["itemName"].ToString();
                    newItem.amount = int.Parse(item["amount"].ToString());
                    inventoryItems.Add(newItem);
                }
            }
        }

        if(jsonData.ContainsKey("allInventoryItemsData"))
        {
             List<Dictionary<string, object>> inventoryItemsData = 
             JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["allInventoryItemsData"].ToString());   
            for(int i = 0; i< inventoryItemsData.Count; i++)
            {
                foreach(KeyValuePair<string,object> inventoryItem in inventoryItemsData[i])
                {
                    Dictionary<string,object> item = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(inventoryItem.Value.ToString());   
                    InventoryItems newItem = new InventoryItems();
                    newItem.itemName = item["itemName"].ToString();
                    newItem.iconUrl = item["iconUrl"].ToString();
                    newItem.type = item["type"].ToString();
                    if(item["type"].ToString() == "workerEnergyConsumable")
                    {
                        newItem.energyIncrease = int.Parse(item["energyIncrease"].ToString());
                    }
                    Constants.allInventoryItems.Add(newItem);
                }
            }
        }

        List<BuildingUpgrade> buildingUpgrades = new List<BuildingUpgrade>();
        List<WorkerWork> workerWorks = new List<WorkerWork>();

        if(jsonData.ContainsKey("workerWorkData"))
        {
             List<Dictionary<string, object>> workerWorksData = 
             JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["workerWorkData"].ToString());   
            for(int i = 0; i< workerWorksData.Count; i++)
            {
                foreach(KeyValuePair<string,object> workerWork in workerWorksData[i])
                {
                    Dictionary<string,object> work = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(workerWork.Value.ToString());   
                    DateTime startDate = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(work["startTime"].ToString()));
                    WorkerWork workerWork1 = new WorkerWork();
                    workerWork1.startTime = startDate;
                    workerWork1.workerWorkDocId = workerWork.Key.ToString();
                     workerWork1.times = int.Parse(work["times"].ToString());
                      workerWork1.currentTime = int.Parse(work["currentTime"].ToString());
                      workerWork1.fieldDocId = work["fieldDocId"].ToString();
                     workerWork1.zoneDocId = work["zoneDocId"].ToString();
                     workerWork1.workerDocId = work["workerDocId"].ToString();
                    workerWorks.Add(workerWork1);
                }
            }
        }
        if(jsonData.ContainsKey("upgradeData"))
        {
            List<Dictionary<string, object>> upgradeData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["upgradeData"].ToString());   
            for(int i = 0; i< upgradeData.Count; i++)
            {
                foreach(KeyValuePair<string,object> buildingUpgrade in upgradeData[i])
                {
                    Dictionary<string,object> build = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(buildingUpgrade.Value.ToString());   
                    DateTime date = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(build["endTime"].ToString()));

                    DateTime startDate = (new DateTime(1970, 1, 1)).AddMilliseconds
                    (double.Parse(build["startTime"].ToString()));
                    BuildingUpgrade buildingUpgrade1 = new BuildingUpgrade();
                    buildingUpgrade1.endTime = date;
                    buildingUpgrade1.startTime = startDate;

                    if(buildingUpgrade.Key== "stoneDeposit")
                    {
                        buildingUpgrade1.buildingName = Building.stoneDeposit;
                    }
                    if(buildingUpgrade.Key== "woodDeposit")
                    {
                        buildingUpgrade1.buildingName = Building.woodDeposit;
                    }
                    if(buildingUpgrade.Key== "workerHome")
                    {
                        buildingUpgrade1.buildingName = Building.workerHome;
                    }
                    if(buildingUpgrade.Key== "warriorBuilding")
                    {
                        buildingUpgrade1.buildingName = Building.warriorBuilding;
                    }
                    if(buildingUpgrade.Key== "workerBuilding")
                    {
                        buildingUpgrade1.buildingName = Building.workerBuilding;
                    }
                    if(buildingUpgrade.Key== "mainTower")
                    {
                        buildingUpgrade1.buildingName = Building.mainTower;
                    }
                    buildingUpgrades.Add(buildingUpgrade1);
                }
            }
            

        }

        if(jsonData.ContainsKey("zoneData"))
        {
            Constants.allZones = new List<Zones>();
            List<Dictionary<string, object>> allZones = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData["zoneData"].ToString());   
            foreach(Dictionary<string, object> zone in allZones)
            {
                Zones newZone = new Zones();
                List<Field> allFieldsToAdd = new List<Field>();
                newZone.zoneName = zone["name"].ToString();
                newZone.docId = zone["docId"].ToString();
                newZone.zoneIndex = int.Parse(zone["zoneIndex"].ToString());
                newZone.requiredMainTowerLevel = int.Parse(zone["requiredMainTowerLevel"].ToString());
                List<Dictionary<string,object>> allFields = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(zone["fieldsData"].ToString());
                foreach(Dictionary<string, object> field in allFields)
                {
                    Field newFiled = new Field();
                    newFiled.docId = field["docId"].ToString();
                    newFiled.fieldType = field["fieldType"].ToString();
                    if(newFiled.fieldType == "production")
                    {
                    List<Dictionary<string, object>> allProducts = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(field["productItem"].ToString());   
                    newFiled.productItem = allProducts;
                    if(field.ContainsKey("extraProductItem"))
                    {
                    List<Dictionary<string, object>> allProductsExtra = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(field["extraProductItem"].ToString());
                    newFiled.extraProductItem = allProductsExtra;
                    }

                    }
                    allFieldsToAdd.Add(newFiled);
                }
                newZone.fields = allFieldsToAdd;
                Constants.allZones.Add(newZone);
            }

        }

        if(jsonData.ContainsKey("buildingsData"))
        {
            Constants.allBuildings = new List<Buildings>();

            Dictionary<string, object> allBuildings = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["buildingsData"].ToString());   
           foreach(KeyValuePair<string, object> entry in allBuildings)
            {
                Buildings newBuilding = new Buildings();
                Dictionary<string, object> buildingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(allBuildings[entry.Key].ToString());   
                foreach(KeyValuePair<string, object> innerEntry in buildingData)
                {
                    if(innerEntry.Key == "main")
                    {
                    Dictionary<string, object> building = JsonConvert.DeserializeObject<Dictionary<string, object>>(buildingData["main"].ToString());   
                    newBuilding.maxLevel = int.Parse(building["maxLevel"].ToString());
                    newBuilding.buildingName = building["buildingName"].ToString();
                    }
                    else
                    {
                    BuildingLevels buildingLevel = new BuildingLevels();
                    Dictionary<string, object> buildingLevelData = JsonConvert.DeserializeObject<Dictionary<string, object>>(buildingData[innerEntry.Key].ToString());
                    

                    if(buildingLevelData.ContainsKey("input"))   
                    {
                     List<Dictionary<string, object>> buildingLevelInput = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(buildingLevelData["input"].ToString());   
                    buildingLevel.input = buildingLevelInput;
                    }
                    if(buildingLevelData.ContainsKey("output"))   
                    {
                     List<Dictionary<string, object>>  buildingLevelOutput = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(buildingLevelData["output"].ToString());   
                    buildingLevel.output = buildingLevelOutput;
                    }
                    List<Dictionary<string, object>> buildingLevelPrice = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(buildingLevelData["price"].ToString());   
                    buildingLevel.level = int.Parse(innerEntry.Key);
                    if(buildingLevelData.ContainsKey("id"))
                    buildingLevel.id = buildingLevelData["id"].ToString();

                    if(buildingLevelData.ContainsKey("requiredMainTowerLevel"))
                    buildingLevel.requiredMainTowerLevel = int.Parse(buildingLevelData["requiredMainTowerLevel"].ToString());

                    buildingLevel.price = buildingLevelPrice;
                    newBuilding.levels.Add(buildingLevel);
                    }
                }
                Constants.allBuildings.Add(newBuilding);
            }
        }
        Constants.currentUser = new User(mainTowerLevel,stoneDepositLevel,woodDepositLevel,workerHome,
        workerBuildingLevel,warriorBuildingLevel,peridotShardCount,stoneCount,woodCount,workers,inventoryItems,buildingUpgrades,workerWorks);
    }
}
