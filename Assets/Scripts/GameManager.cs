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
    public InventoryController inventoryController;
    public GameObject bottomNavigationBar;

    public void StartTown(string json)
    {
        CreateUser(json);
        StartCoroutine(workerInventoryController.GenerateWorkers());
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
        inventoryController.GenerateItems();
        townScene.GetComponent<TownManager>().GenerateTown();
        uIController.GenerateUserData();
    }

    public void CreateUser(string json)
    {
        Dictionary<string, object> playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(playerData["userData"].ToString());
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
            newWorker.maxStamina = float.Parse(worker["maxStamina"].ToString());
            newWorker.isNFT = bool.Parse(worker["isNFT"].ToString());
            newWorker.rarity = worker["rarity"].ToString();
            newWorker.workSpeed = float.Parse(worker["workSpeed"].ToString());
            newWorker.url = worker["url"].ToString();
            newWorker.type = worker["worker"].ToString();
            newWorker.docId = worker["docId"].ToString();
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

        Constants.currentUser = new User(mainTowerLevel,stoneDepositLevel,woodDepositLevel,workerBuildingLevel,
        workerBuildingLevel,warriorBuildingLevel,peridotShardCount,stoneCount,woodCount,workers,inventoryItems);
    }
}
