using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject UpgradeLeft;
    public Image upgradeSlider;
    public Text upgradeTimeText;
    public bool startCounter = false;
    DateTime endTime;
    DateTime startTime;
    bool upgradeFinished = false;
    

    public void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        popUpController = FindObjectOfType<PopUpController>();
    }

    public void Start()
    {
        CheckHasUpgrade();
    }


    public void CheckHasUpgrade()
    {
        for(int i = 0; i<Constants.currentUser.buildingUpgrades.Count; i++)
        {
                if(Constants.currentUser.buildingUpgrades[i].buildingName == buildingType)
                {
                    endTime = Constants.currentUser.buildingUpgrades[i].endTime;
                    startTime = Constants.currentUser.buildingUpgrades[i].startTime;
                    UpgradeLeft.SetActive(true);
                    upgradeFinished = false;
                    startCounter = true;
                    break;
                }
        }
    }

    public void Update()
    {
        if(startCounter && !upgradeFinished)
        {
                TimeSpan ts1 =  endTime - startTime;
                TimeSpan ts = endTime - DateTime.UtcNow;
                upgradeSlider.fillAmount = 1 - ((float)ts.TotalSeconds / (float)ts1.TotalSeconds);
                upgradeTimeText.text = string.Format("{1}:{2}:{3}", ts.Days, (ts.Days*24) + ts.Hours, ts.Minutes, ts.Seconds);
        }
        if(DateTime.Compare(DateTime.UtcNow ,endTime) > 0 && !upgradeFinished && startCounter)
        {
            upgradeFinished = true;
            FindObjectOfType<FirebaseApi>().CheckUpgradeBuildingFinished(buildingType);
            UpgradeLeft.SetActive(false);
            startCounter = false;
        }
    }

    public void OnClickCall(TownManager townManager)
    {
        bool found = false;
        townManager.OpenOnClicks(this);
        int currentBuildingLevel = 0;
        bool openable = false;
        bool onUpgrade = false;
                
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

              for(int i = 0; i<Constants.currentUser.buildingUpgrades.Count; i++)
                {
                    if(Constants.currentUser.buildingUpgrades[i].buildingName == buildingType)
                    {
                        onUpgrade = true;
                        break;
                    }
                }
            if(Constants.allBuildings[a].buildingName == buildingType.ToString())
            {
                found = true;
                if(currentBuildingLevel == Constants.allBuildings[a].maxLevel || onUpgrade)
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
