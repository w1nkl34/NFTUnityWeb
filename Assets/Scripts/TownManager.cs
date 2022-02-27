using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownManager : MonoBehaviour
{

    public Text mainTowerLevelText;
    public Text stoneDepositLevelText;
    public Text woodDepositLevelText;
    public Text workerBuildingLevelText;
    public Text warriorBuildingLevelText;
    public GameObject mainTower;
    public GameObject stoneDeposit;
    public GameObject woodDeposit;
    public GameObject workerBuilding;
    public GameObject warriorBuilding;
    public BuildingController mainTowerBuildingController;
    public BuildingController stoneDepositBuildingController;
    public BuildingController woodDepositBuildingController;
    public BuildingController workerBuildingBuildingController;
    public BuildingController warriorBuildingBuildingController;

    private BuildingController selectedBuilding;
    
    void OnEnable()
    {
        GenerateTown();
    }
    public void OpenOnClicks(BuildingController bc)
    {
        mainTowerBuildingController.OnClick.SetActive(false);
        stoneDepositBuildingController.OnClick.SetActive(false);
        woodDepositBuildingController.OnClick.SetActive(false);
        workerBuildingBuildingController.OnClick.SetActive(false);
        warriorBuildingBuildingController.OnClick.SetActive(false);
        bc.OnClick.SetActive(true);
        selectedBuilding = bc;
    }
    public void CloseAllOnClicks()
    {
        mainTowerBuildingController.OnClick.SetActive(false);
        stoneDepositBuildingController.OnClick.SetActive(false);
        woodDepositBuildingController.OnClick.SetActive(false);
        workerBuildingBuildingController.OnClick.SetActive(false);
        warriorBuildingBuildingController.OnClick.SetActive(false);
    }

    public void GenerateTown()
    {
       mainTowerLevelText.text = "Main Tower Level: " + Constants.currentUser.mainTowerLevel.ToString();
        stoneDepositLevelText.text = "Stone Deposit Level: " + Constants.currentUser.stoneDepositLevel.ToString();
        woodDepositLevelText.text = "Wood Deposit Level: " + Constants.currentUser.woodDepositLevel.ToString();
        workerBuildingLevelText.text = "Worker Building Level: " + Constants.currentUser.workerBuildingLevel.ToString();
        warriorBuildingLevelText.text = "Warrior Building Level: " + Constants.currentUser.warriorBuildingLevel.ToString();

        mainTower.SetActive(true);
        stoneDeposit.SetActive(true);
        woodDeposit.SetActive(true);
        if(Constants.currentUser.workerBuildingLevel != 0)
        workerBuilding.SetActive(true);
        if(Constants.currentUser.warriorBuildingLevel != 0)
        warriorBuilding.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
             RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit,50)) 
        {
            if(hit.collider != null)
            {
                ManageColliderHit(hit);
            }
        }
        else
            CloseAllOnClicks();
        }
    }

    public void ManageColliderHit(RaycastHit hit)
    {
        if(hit.collider.gameObject.GetComponent<BuildingController>() != null)
        {
        hit.collider.gameObject.GetComponent<BuildingController>().OnClickCall();
        return;
        }
        
        if(hit.collider.gameObject.tag == "onClickOpen")
        {
            selectedBuilding.OpenMainTab();
            return;
        }
        if(hit.collider.gameObject.tag == "onClickUpgrade")
        {
            selectedBuilding.OpenUpgradeTab();
            return;
        }
        if(hit.collider.gameObject.tag == "onClickInfo")
        {
            selectedBuilding.OpenInfoTab();
            return;
        }
    }
}
