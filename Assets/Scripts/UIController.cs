using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public Text stoneCountText;
    public Text woodCountText;
    public Text peridotShardCountText;
    public GameObject workerInventory;
    public GameObject inventory;
    public GameObject warriorBuildingTab;
    public GameObject workerBuildingTab;
    public GameObject woodDepositBuildingTab;
    public GameObject stoneDepositBuildingTab;
    public GameObject mainTowerBuildingTab;
    private GameObject currentBuildingMainTab;
    public GameObject authenticateTab;
    public GameObject backgroundNoSafeArea;
    public void GenerateUserData()
    {
        stoneCountText.text = "Stone: " + Constants.currentUser.stoneCount.ToString();
        woodCountText.text = "Wood: " + Constants.currentUser.woodCount.ToString();
        peridotShardCountText.text = "Peridot: " + Constants.currentUser.peridotShardCount.ToString();
    }

    public void ShowAuthenticationScreen(bool what)
    {
        authenticateTab.SetActive(what);
    }

    public void OpenWorkerInventory()
    {
        backgroundNoSafeArea.SetActive(true);
        workerInventory.SetActive(true);
    }

    public void CloseWorkerInventory()
    {
        backgroundNoSafeArea.SetActive(false);
        workerInventory.SetActive(false);
    }

    public void OpenInventory()
    {
        backgroundNoSafeArea.SetActive(true);
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        backgroundNoSafeArea.SetActive(false);
        inventory.SetActive(false);
    }

    public void CloseBuildingMainTab()
    {
        backgroundNoSafeArea.SetActive(false);
        currentBuildingMainTab.SetActive(false);
    }

    public void OpenBuildingMainTabs(Building building)
    {
        if(building == Building.mainTower)
        {
        mainTowerBuildingTab.SetActive(true);
        currentBuildingMainTab = mainTowerBuildingTab;
        }
        if(building == Building.stoneDeposit)
        {
        stoneDepositBuildingTab.SetActive(true);
        currentBuildingMainTab = stoneDepositBuildingTab;
        }
        if(building == Building.woodDeposit)
        {
        woodDepositBuildingTab.SetActive(true);
        currentBuildingMainTab = woodDepositBuildingTab;
        }
        if(building == Building.workerBuilding)
        {
        workerBuildingTab.SetActive(true);
        currentBuildingMainTab = workerBuildingTab;
        }
        if(building == Building.warriorBuilding)
        {
        warriorBuildingTab.SetActive(true);
        currentBuildingMainTab = warriorBuildingTab;
        }
        backgroundNoSafeArea.SetActive(true);
    }
}
