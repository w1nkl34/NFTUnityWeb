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


    public void OpenInfoPop(string message)
    {
        popUpInfo.messageText.text = message;
        popUpInfo.gameObject.SetActive(true);
    }


    public void CloseAllPops()
    {
        popUpUpgradeBuilding.gameObject.SetActive(false);
        popUpSummonWorker.gameObject.SetActive(false);
        popUpWorkerDetail.gameObject.SetActive(false);
        popUpInfo.gameObject.SetActive(false);
    }


    public void OpenSummonWorker(string title,int index,List<Dictionary<string,object>> input)
    {
        popUpSummonWorker.gameObject.SetActive(true);
        popUpSummonWorker.OpenSummonWorker(title,index,input);

    }

    public void OpenWorkerDetails(Workers worker,Sprite workerSprite)
    {   
       popUpWorkerDetail.OpenWorkerDetails(worker,workerSprite);
       popUpWorkerDetail.gameObject.SetActive(true);
    }


    public void OpenUpgradeBuilding(string buildingName)
    {
        popUpUpgradeBuilding.gameObject.SetActive(true);
        popUpUpgradeBuilding.OpenUpgradeBuilding(buildingName);
    }

}
