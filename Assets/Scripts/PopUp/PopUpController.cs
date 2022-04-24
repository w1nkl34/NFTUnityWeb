using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopUpController : MonoBehaviour
{

    public PopUpInfo popUpInfo;
    public PopUpUpgradeBuilding popUpUpgradeBuilding;
    public PopUpSummonWorker popUpSummonWorker;
    public PopUpWorkerDetail popUpWorkerDetail;
    public PopUpSellNFT popUpSellNFT;
    public PopUpDestroyWorker popUpDestroyWorker;
    public PopUpWorkerSummonDecline popUpWorkerSummonDecline;
    public PopUpRewardPool popUpRewardPool;
    public PopUpZoneAccessRequiredMainTowerLevel popUpZoneAccessRequiredMainTowerLevel;
    public UIController uIController;
    public PopUpWorkProductionController popUpWorkProductionController;
    public PopUpBattleController popUpBattleController;
    public PopUpWorkIncreaseDecreaseTime popUpWorkIncreaseDecreaseTimeController;
    public GameManager gm;
    public LeanTweenType leanTweenType;

    public void SetActives(GameObject gm)
    {
        gm.transform.GetChild(1).localScale = new Vector3(0,0,0);
        gm.SetActive(true);
        LeanTween.scale( gm.transform.GetChild(1).gameObject,new Vector3(1,1,1),0.15f).setEase(leanTweenType);
    }

    public void OpenInfoPop(string message)
    {
        Constants.onMenu = true;
        popUpInfo.messageText.text = message;
        SetActives(popUpInfo.gameObject);
    }

    public void OpenProductionPop()
    {
        Constants.onMenu = true;
        SetActives(popUpWorkProductionController.gameObject);
    }

    public void OpenBattlePop()
    {
        Constants.onMenu = true;
        SetActives(popUpBattleController.gameObject);

    }

    public void OpenZoneAccessRequiredMainTowerLevelPop()
    {
        Constants.onMenu = true;
        SetActives(popUpZoneAccessRequiredMainTowerLevel.gameObject);

    }

    public void OpenPopUpWorkIncreaseDecreaseTimeController(WorkerWork workerWork,string zoneDocId,string fieldDocId)
    {
        Constants.onMenu = true;
        popUpWorkIncreaseDecreaseTimeController.OpenPop(workerWork,zoneDocId,fieldDocId);
        SetActives(popUpWorkIncreaseDecreaseTimeController.gameObject);
        popUpWorkIncreaseDecreaseTimeController.CheckInteracts();
    }

    public void OpenPopUpRewardPool(Dictionary<string,int> rewards)
    {
        Constants.onMenu = true;
        popUpRewardPool.OpenPop(rewards,gm);
        SetActives(popUpRewardPool.gameObject);

    }

    public void CloseAllPops()
    {
        popUpZoneAccessRequiredMainTowerLevel.gameObject.SetActive(false);
        popUpBattleController.gameObject.SetActive(false);
        popUpUpgradeBuilding.gameObject.SetActive(false);
        popUpSummonWorker.gameObject.SetActive(false);
        popUpWorkerDetail.gameObject.SetActive(false);
        popUpInfo.gameObject.SetActive(false);
        popUpSellNFT.gameObject.SetActive(false);
        popUpRewardPool.gameObject.SetActive(false);
        popUpDestroyWorker.gameObject.SetActive(false);
        popUpWorkerSummonDecline.gameObject.SetActive(false);
        popUpWorkProductionController.gameObject.SetActive(false);
        popUpWorkIncreaseDecreaseTimeController.gameObject.SetActive(false);
        Constants.onMenu = false;
        // uIController.SetOnMenuToFalseCorCall();
    }



    public void OpenSummonWorkerDecline()
    {
        SetActives(popUpWorkerSummonDecline.gameObject);
        popUpWorkerSummonDecline.OpenSummonWorkerDecline();
    }

    public void OpenSellNFT(Workers worker)
    {
        SetActives(popUpSellNFT.gameObject);
        popUpSellNFT.OpenSellNFT(worker);      
    }

    public void OpenDestroyWorker(Workers worker)
    {
        SetActives(popUpDestroyWorker.gameObject);
        popUpDestroyWorker.OpenDestroyWorker(worker);
    }


    public void OpenSummonWorker(string title,int index,List<Dictionary<string,object>> input)
    {
        SetActives(popUpSummonWorker.gameObject);
        popUpSummonWorker.OpenSummonWorker(title,index,input);

    }

    public void OpenWorkerDetails(Workers worker,Sprite workerSprite)
    {   
       popUpWorkerDetail.OpenWorkerDetails(worker,workerSprite);
        SetActives(popUpWorkerDetail.gameObject);
    }


    public void OpenUpgradeBuilding(string buildingName)
    {
        Constants.onMenu = true;
        SetActives(popUpUpgradeBuilding.gameObject);
        popUpUpgradeBuilding.OpenUpgradeBuilding(buildingName);
    }

}
