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

    public Text workerHomeLevelText;
    public GameObject mainTower;
    public GameObject stoneDeposit;
    public GameObject woodDeposit;
    public GameObject workerBuilding;
    public GameObject warriorBuilding;
    public GameObject workerHome;
    public BuildingController mainTowerBuildingController;
    public BuildingController stoneDepositBuildingController;
    public BuildingController woodDepositBuildingController;
    public BuildingController workerBuildingBuildingController;
    public BuildingController warriorBuildingBuildingController;
    public BuildingController workerHomeBuildingController;
    private BuildingController selectedBuilding;
    public Material selectedMaterial;
    public Material unSelectedMaterial;
    
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
        workerHomeBuildingController.OnClick.SetActive(false);
        warriorBuildingBuildingController.OnClick.SetActive(false);
        bc.OnClick.SetActive(true);
        selectedBuilding = bc;
    }


    public void ChangeToUnSelectedMaterial()
    {
            foreach(Transform child in GameObject.FindGameObjectWithTag("warriorBuilding").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }
    
            foreach(Transform child in GameObject.FindGameObjectWithTag("woodDeposit").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }        

             foreach(Transform child in GameObject.FindGameObjectWithTag("workerBuilding").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }

       
             foreach(Transform child in GameObject.FindGameObjectWithTag("stoneDeposit").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }

         foreach(Transform child in GameObject.FindGameObjectWithTag("mainTower").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }

            foreach(Transform child in GameObject.FindGameObjectWithTag("workerHome").transform)
            {
                child.GetComponent<SpriteRenderer>().material = unSelectedMaterial;
            }
    }
    public void CloseAllOnClicks()
    {
        ChangeToUnSelectedMaterial();
        mainTowerBuildingController.OnClick.SetActive(false);
        stoneDepositBuildingController.OnClick.SetActive(false);
        workerHomeBuildingController.OnClick.SetActive(false);
        woodDepositBuildingController.OnClick.SetActive(false);
        workerBuildingBuildingController.OnClick.SetActive(false);
        warriorBuildingBuildingController.OnClick.SetActive(false);
    }

    public void GenerateTown()
    {
        mainTowerLevelText.text = Constants.currentUser.mainTowerLevel.ToString();
        stoneDepositLevelText.text = Constants.currentUser.stoneDepositLevel.ToString();
        woodDepositLevelText.text = Constants.currentUser.woodDepositLevel.ToString();
        workerBuildingLevelText.text = Constants.currentUser.workerBuildingLevel.ToString();
        warriorBuildingLevelText.text = Constants.currentUser.warriorBuildingLevel.ToString();
        workerHomeLevelText.text = Constants.currentUser.workerHomeLevel.ToString();
        mainTower.SetActive(true);
        stoneDeposit.SetActive(true);
        woodDeposit.SetActive(true);
        workerHome.SetActive(true);
        workerBuilding.SetActive(true);
        warriorBuilding.SetActive(true);
    }


    public void ManageColliderHit(RaycastHit hit)
    {
        if(hit.collider.gameObject.GetComponent<BuildingController>() != null)
        {
            ChangeToUnSelectedMaterial();
        hit.collider.gameObject.GetComponent<BuildingController>().OnClickCall(this);
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
