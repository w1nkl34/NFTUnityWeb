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
    private UIController uIController;
    private PopUpController popUpController;

    public void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        popUpController = FindObjectOfType<PopUpController>();
    }


    public void OnClickCall(TownManager townManager)
    {
        bool found = false;
        townManager.OpenOnClicks(this);
        int currentBuildingLevel = 0;
        bool openable = false;
                
        if(buildingType.ToString() == "warriorBuilding")
        {            
            openable = true;
            currentBuildingLevel = Constants.currentUser.warriorBuildingLevel;
             foreach(Transform child in GameObject.FindGameObjectWithTag("warriorBuilding").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }
        }
        
        if(buildingType.ToString() == "woodDeposit")
        {
            currentBuildingLevel = Constants.currentUser.woodDepositLevel;
            foreach(Transform child in GameObject.FindGameObjectWithTag("woodDeposit").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }

        }
        
        if(buildingType.ToString() == "workerBuilding")
        {
            openable = true;
            currentBuildingLevel = Constants.currentUser.workerBuildingLevel;
             foreach(Transform child in GameObject.FindGameObjectWithTag("workerBuilding").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }
        }

        if(buildingType.ToString() == "stoneDeposit")
        {
            currentBuildingLevel = Constants.currentUser.stoneDepositLevel;
             foreach(Transform child in GameObject.FindGameObjectWithTag("stoneDeposit").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }

        }

        if(buildingType.ToString() == "mainTower")
        {
        currentBuildingLevel = Constants.currentUser.mainTowerLevel;
         foreach(Transform child in GameObject.FindGameObjectWithTag("mainTower").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }

        }

        if(buildingType.ToString() == "workerHome")
        {
            currentBuildingLevel = Constants.currentUser.mainTowerLevel;
            foreach(Transform child in GameObject.FindGameObjectWithTag("workerHome").transform)
            {
                child.GetComponent<SpriteRenderer>().material = townManager.selectedMaterial;
            }
        }

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
                    if(!openable)
                    infoClick.transform.localPosition = new Vector3(0,0,0);
                    break;
                }
                else
                {
                    openClick.transform.localPosition = new Vector3(-2,0,0); 
                    infoClick.transform.localPosition = new Vector3(2,0,0);
                    upgradeClick.transform.localPosition = new Vector3(0,0,0);
                    if(!openable)
                    {
                        infoClick.transform.localPosition = new Vector3(-1,0,0);
                        upgradeClick.transform.localPosition = new Vector3(1,0,0);
                    }
                    break;
                }
            }
        }
        if(found == false)
        {
            upgradeClick.SetActive(false);
            openClick.transform.localPosition = new Vector3(-1,0,0);
            infoClick.transform.localPosition = new Vector3(1,0,0);
            if(!openable)
            infoClick.transform.localPosition = new Vector3(0,0,0);
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
