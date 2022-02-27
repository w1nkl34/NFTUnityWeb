using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public Building buildingType;
    public GameObject OnClick;
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
        townManager.OpenOnClicks(this);
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
