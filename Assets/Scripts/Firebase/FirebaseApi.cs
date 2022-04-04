using System.Collections;
using System.Collections.Generic;
using Firebase.Functions;
using UnityEngine;
using Firebase.Extensions;
using Newtonsoft.Json;

public  class FirebaseApi : MonoBehaviour
{
    GameManager gm;
    PopUpController popUpController;


    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        popUpController = FindObjectOfType<PopUpController>();
    }

    public void UpgradeBuilding(string buildingName) {
        gm.OpenCloseLoadingBar(true);
        var functions = FirebaseFunctions.DefaultInstance;
        var data = new Dictionary<string,object>();
        data["buildingName"] = buildingName;
        var function = functions.GetHttpsCallable("upgradeBuilding");
       function.CallAsync(data).ContinueWithOnMainThread( (task) => {
           if(task.IsCompleted)
           {
                 string x =  JsonConvert.SerializeObject(task.Result.Data);
                Debug.Log(x);
               StartCoroutine(GetUserData());
           }
        });
    }

    
    public  IEnumerator GetUserData() {
        var functions = FirebaseFunctions.DefaultInstance;
        var function = functions.GetHttpsCallable("getOnlyUserData");
        var task = function.CallAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
           
        }
        else
        {
            gm.GetUser(task.Result.Data.ToString());
            gm.OpenCloseLoadingBar(false);
            popUpController.OpenInfoPop("Success!");
        }
    }
}
