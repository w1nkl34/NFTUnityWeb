using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUpgradeBuilding : MonoBehaviour
{

    public GameObject upgradeRequiredItems;

    public Text levelToLevelText;

    public GameObject upgradeRequiredItemsTextGameObject;

    public Text buildingName;

    public Button upgradeButton;
    public void ClosePop()
    {
        gameObject.SetActive(false);
    }

    public void ResetUpgradeBuildingRequiredItems()
    {
        foreach(Transform child in upgradeRequiredItems.transform.parent)
        {
            if(child.gameObject != upgradeRequiredItems && child.gameObject != upgradeRequiredItemsTextGameObject && child.gameObject != levelToLevelText.gameObject)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void UpgradeWorkerRequest(string buildingName)
    {
        FindObjectOfType<ReactSend>().UpgradeBuildingCall(buildingName);
        gameObject.SetActive(false);
    }

    public void OpenUpgradeBuilding(string buildingName)
    {
        ResetUpgradeBuildingRequiredItems();
        this.buildingName.text = buildingName;

        int keyIndex = 0;
        bool error = false;
        int currentBuildingLevel = 0;

        if(buildingName == "warriorBuilding")
        currentBuildingLevel = Constants.currentUser.warriorBuildingLevel;
        
        if(buildingName == "woodDeposit")
        currentBuildingLevel = Constants.currentUser.woodDepositLevel;
        
        if(buildingName == "workerBuilding")
        currentBuildingLevel = Constants.currentUser.workerBuildingLevel;

        if(buildingName == "stoneDeposit")
        currentBuildingLevel = Constants.currentUser.stoneDepositLevel;

        if(buildingName == "mainTower")
        currentBuildingLevel = Constants.currentUser.mainTowerLevel;


        List<Dictionary<string,object>> price = new List<Dictionary<string,object>>();
        for(int a = 0; a<Constants.allBuildings.Count; a++)
        {
            if(Constants.allBuildings[a].buildingName == buildingName)
            { 
                for(int i = 0; i<Constants.allBuildings[a].levels.Count; i++)
                {
                    if(Constants.allBuildings[a].levels[i].level == currentBuildingLevel)
                    {
                        price = Constants.allBuildings[a].levels[i].price;
                        break;
                    }
                }
            }
        }
        
        for(int j = 0; j<price.Count; j++)
        {       
            string key = "";
            string value = "";      
            string myValue = "";    
            foreach(KeyValuePair<string, object> entry in price[j])
            {
                if(keyIndex == j)
                {
                     key = entry.Key;
                     value = entry.Value.ToString();
                     if(key == "wood")
                     {
                         myValue = Constants.currentUser.woodCount.ToString();
                     }
                     if(key == "stone")
                     {
                         myValue = Constants.currentUser.stoneCount.ToString();
                     }
                     if(int.Parse(myValue) < int.Parse(value))
                     {
                         error = true;
                     }
                     break;
                }
            }
            keyIndex++;
            GameObject newRequiredText = Instantiate(upgradeRequiredItems,upgradeRequiredItems.transform.parent);
            newRequiredText.GetComponent<Text>().text =  key + ": " + value + "/" + myValue; 
            newRequiredText.SetActive(true);
        }
        levelToLevelText.text = Constants.currentUser.workerBuildingLevel.ToString() + " >> " + (Constants.currentUser.workerBuildingLevel+1).ToString();

        if(error)
        {
        upgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Not Enough Materials!";
        upgradeButton.enabled = false;
        }
        else
        {
        upgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Upgrade!";
        upgradeButton.enabled = true;
        upgradeButton.onClick.AddListener(delegate{UpgradeWorkerRequest(buildingName);});

        }
    }
}
