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
    public GameObject bottomNavigationBar;
    public GameObject loadingBar;

    public void OpenCloseLoadingBar(bool value)
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
        bottomNavigationBar.SetActive(true);
        townScene.GetComponent<TownManager>().GenerateTown();
        uIController.GenerateUserData();
    }

    public void GetUser(string json)
    {
        CreateUser(json);
        StartCoroutine(workerInventoryController.GenerateWorkers());
        workerBuildingController.GenerateWorkerCreate();
        inventoryController.GenerateItems();
        townScene.GetComponent<TownManager>().GenerateTown();
        uIController.GenerateUserData();
        OpenCloseLoadingBar(false);
    }

    public void CreateUser(string json)
    {
        
        Dictionary<string, object> jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
         Dictionary<string, object>  playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData["userData"].ToString());
        int mainTowerLevel = int.Parse(playerData["mainTower"].ToString());
        int stoneDepositLevel = int.Parse(playerData["stoneDeposit"].ToString());
        int woodDepositLevel = int.Parse(playerData["woodDeposit"].ToString());
        int workerCapacity = int.Parse(playerData["workerCapacity"].ToString());
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
        InventoryItems inventoryItems = new InventoryItems();
        inventoryItems.powerCrystal = playerData.ContainsKey("powerCrystal") ? int.Parse(playerData["powerCrystal"].ToString()) : 0;
        inventoryItems.summonCrystal = playerData.ContainsKey("summonCrystal") ? int.Parse(playerData["summonCrystal"].ToString()) : 0;
        inventoryItems.upgradeCrystal = playerData.ContainsKey("upgradeCrystal") ? int.Parse(playerData["upgradeCrystal"].ToString()) : 0;
        inventoryItems.blessedPowerCrystal = playerData.ContainsKey("blessedPowerCrystal") ? int.Parse(playerData["blessedPowerCrystal"].ToString()) : 0;
        inventoryItems.blessedSummonCrystal = playerData.ContainsKey("blessedSummonCrystal") ? int.Parse(playerData["blessedSummonCrystal"].ToString()) : 0;
        inventoryItems.blessedUpgradeCrystal = playerData.ContainsKey("blessedUpgradeCrystal") ? int.Parse(playerData["blessedUpgradeCrystal"].ToString()) : 0;
        inventoryItems.legendaryPowerCrystal = playerData.ContainsKey("legendaryPowerCrystal") ? int.Parse(playerData["legendaryPowerCrystal"].ToString()) : 0;
        inventoryItems.legendarySummonCrystal = playerData.ContainsKey("legendarySummonCrystal") ? int.Parse(playerData["legendarySummonCrystal"].ToString()) : 0;
        inventoryItems.legendaryUpgradeCrystal = playerData.ContainsKey("legendaryUpgradeCrystal") ? int.Parse(playerData["legendaryUpgradeCrystal"].ToString()) : 0;

        Constants.allBuildings = new List<Buildings>();

        if(jsonData.ContainsKey("buildingsData"))
        {
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
        Constants.currentUser = new User(mainTowerLevel,stoneDepositLevel,woodDepositLevel,workerCapacity,
        workerBuildingLevel,warriorBuildingLevel,peridotShardCount,stoneCount,woodCount,workers,inventoryItems);
    }
}
