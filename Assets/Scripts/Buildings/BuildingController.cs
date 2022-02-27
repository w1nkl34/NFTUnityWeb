using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public Building buildingType;
    public GameObject OnClick;
    public GameObject openClick;
    public GameObject upgradeClick;
    public GameObject infoClick;
    public TownManager townManager;
    public UIController uIController;
    public PopUpController popUpController;

    public void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        popUpController = FindObjectOfType<PopUpController>();
    }

    public void OnClickCall()
    {
        bool found = false;
        townManager.OpenOnClicks(this);
        int currentBuildingLevel = 0;
                
        if(buildingType.ToString() == "warriorBuilding")
        currentBuildingLevel = Constants.currentUser.warriorBuildingLevel;
        
        if(buildingType.ToString() == "woodDeposit")
        currentBuildingLevel = Constants.currentUser.woodDepositLevel;
        
        if(buildingType.ToString() == "workerBuilding")
        currentBuildingLevel = Constants.currentUser.workerBuildingLevel;

        if(buildingType.ToString() == "stoneDeposit")
        currentBuildingLevel = Constants.currentUser.stoneDepositLevel;

        if(buildingType.ToString() == "mainTower")
        currentBuildingLevel = Constants.currentUser.mainTowerLevel;

        for(int a = 0; a<Constants.allBuildings.Count; a++)
        {
            if(Constants.allBuildings[a].buildingName == buildingType.ToString())
            {
                found = true;
                if(currentBuildingLevel == Constants.allBuildings[a].maxLevel)
                {
                    upgradeClick.SetActive(false);
                    openClick.transform.localPosition = new Vector3(-1,0,0);
                    infoClick.transform.localPosition = new Vector3(1,0,0);
                    break;
                }
            }
        }
        if(found == false)
        {
            upgradeClick.SetActive(false);
            openClick.transform.localPosition = new Vector3(-1,0,0);
            infoClick.transform.localPosition = new Vector3(1,0,0);
        }
    }

    public void OpenMainTab()
    {
        uIController.OpenBuildingMainTabs(buildingType);
    }

    public void OpenInfoTab()
    {

    }

    public void OpenUpgradeTab()
    {
        popUpController.OpenUpgradeBuilding(buildingType.ToString());
    }

}
