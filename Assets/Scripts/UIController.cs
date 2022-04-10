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
    public GameObject registerPage;
    public GameObject signPage;
    public GameObject fade;
    public GameObject bottomLeaveZoneFocus;
    public GameObject bottomNavigationBar;
    public GameObject bottomNavigationBarNoSafeArea;
    public GameManager gm;
    public GameObject bottomBarWorldIcon;
    public Sprite worldIcon;
    public Sprite townIcon;


    public void ChangeWorldMode()
    {
        if (!Constants.onFade)
        {
            if(!Constants.onWorldMap)
        bottomBarWorldIcon.GetComponent<Animator>().enabled = true;
        bottomBarWorldIcon.GetComponent<Animator>().Rebind();
        bottomBarWorldIcon.GetComponent<Animator>().Play("Anim",0);
        Constants.onFade = true;
        FadeStart();
        }
    }

    public void LeaveZoneFocus()
    {
        if(!Constants.onFade)
        {
        Constants.onFade = true;
        gm.cameraController.LeaveFromFocusZone();
        bottomLeaveZoneFocus.SetActive(false);
        bottomNavigationBar.SetActive(true);
        bottomNavigationBarNoSafeArea.SetActive(true);
        gm.wm.ChangeZoneFocus(true);
        if(gm.wm.selectedMainZone != null)
        gm.wm.selectedMainZone.zones.SetActive(false);
        StartCoroutine(FocusWorldZoneFalse());
        }
    }
    public IEnumerator FocusWorldZoneFalse()
    {
        yield return new WaitForSeconds(0.05f);
        Constants.onFade = false;
        gm.cameraController.focusWorldZone = false;
    }

    public void OpenZoneFocus()
    {
        bottomLeaveZoneFocus.SetActive(true);
        bottomNavigationBar.SetActive(false);
        bottomNavigationBarNoSafeArea.SetActive(false);
    }

    public void FadeStart()
    {
        fade.SetActive(true);
        fade.GetComponent<Image>().color = new Color32(0,0,0,0);
         Image r = fade.gameObject.GetComponent<Image>();
             LeanTween.value(fade.gameObject, 0, 1, 0.15f).setOnUpdate((float val) =>
            {
                 Color c = r.color;
                 c.a = val;
                r.color = c;
             }).setOnComplete(FadeEnd);
    }

    public void FadeEnd()
    {
          Constants.onWorldMap = !Constants.onWorldMap;
        FindObjectOfType<CameraController>().ChangeWorldMode();
                    Image r = fade.gameObject.GetComponent<Image>();
             LeanTween.value(fade.gameObject, 1, 0, 0.15f).setOnUpdate((float val) =>
            {
                 Color c = r.color;
                 c.a = val;
                r.color = c;
             }).setOnComplete(FinishFade);
    }
    public void FinishFade()
    {
        fade.gameObject.SetActive(false);
        Constants.onFade = false;
        if(!Constants.onWorldMap)
        bottomBarWorldIcon.GetComponent<Image>().sprite = worldIcon;
        else
        {
        bottomBarWorldIcon.GetComponent<Animator>().enabled = false;
        bottomBarWorldIcon.GetComponent<Image>().sprite = townIcon;
        }

    }

    public void GenerateUserData()
    {
        bottomNavigationBar.SetActive(true);
        bottomNavigationBarNoSafeArea.SetActive(true);
        stoneCountText.text = Constants.currentUser.stoneCount.ToString();
        woodCountText.text = Constants.currentUser.woodCount.ToString();
        peridotShardCountText.text = Constants.currentUser.peridotShardCount.ToString();
    }

    public void ShowAuthenticationScreen(bool what)
    {
        authenticateTab.SetActive(what);
    }

    public void OpenWorkerInventory()
    {
        backgroundNoSafeArea.SetActive(true);
        workerInventory.SetActive(true);
        Constants.onMenu = true;
    }

    public void CloseWorkerInventory()
    {
        backgroundNoSafeArea.SetActive(false);
        workerInventory.SetActive(false);
        SetOnMenuToFalseCorCall();
    }

    public void SetOnMenuToFalseCorCall()
    {
        StartCoroutine(SetOnMenuToFalse());
    }

    public IEnumerator SetOnMenuToFalse()
    {
        yield return new WaitForSeconds(0.05f);
        Constants.onMenu = false;
    }

    public void OpenInventory()
    {
    
        backgroundNoSafeArea.SetActive(true);
        inventory.SetActive(true);
        Constants.onMenu = true;
        

    }

    public void CloseInventory()
    {
         
        backgroundNoSafeArea.SetActive(false);
        inventory.SetActive(false);
        SetOnMenuToFalseCorCall();

    }
    

    public void CloseBuildingMainTab()
    {
      
        backgroundNoSafeArea.SetActive(false);
        currentBuildingMainTab.SetActive(false);
        SetOnMenuToFalseCorCall();

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
        Constants.onMenu = true;
    }

    
}
