using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public Building buildingType;
    public GameObject OnClick;
    public TownManager townManager;

    public UIController uIController;

    public void Awake()
    {
        uIController = FindObjectOfType<UIController>();
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

    }

}
