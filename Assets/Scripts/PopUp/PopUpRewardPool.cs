using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpRewardPool : MonoBehaviour
{

    public Dictionary<string,int> rewards = new Dictionary<string, int>();
    public Transform productsParent;
    public GameManager gm;
    public void ClosePop()
    {
        ResetData();
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
        gameObject.SetActive(false);
    }

    public void OpenPop(Dictionary<string,int> rewards,GameManager gm)
    {
        ResetData();
        this.gm = gm;
        this.rewards = rewards;
        ReceiveItems();
        GenerateItems();
    }

    public void ReceiveItems()
    {
        foreach(KeyValuePair<string,int> item in rewards)
        {
            if(item.Key == "wood")
            {
                Constants.currentUser.woodCount += item.Value;
            }
            else if(item.Key == "stone")
            {
                Constants.currentUser.stoneCount += item.Value;
            }
            else if(item.Key == "peridotShard")
            {
                Constants.currentUser.peridotShardCount += item.Value;
            }
            else
            {
                bool found = false;
                for(int i =0; i<Constants.currentUser.inventoryItems.Count; i++)
                {
                    if(Constants.currentUser.inventoryItems[i].itemName == item.Key)
                    {
                        found = true;
                        Constants.currentUser.inventoryItems[i].amount += item.Value;
                    }
                }
                if(found == false)
                {
                    InventoryItems newItem = new InventoryItems();
                
                        newItem.amount = item.Value;
                        newItem.itemName = item.Key;
                        Constants.currentUser.inventoryItems.Add(newItem);
                }

            }
        }
        gm.UpdateUserLocally();
    }

    public void GenerateItems()
    {
        foreach(KeyValuePair<string,int> item in rewards)
        {
            GameObject newItem = Instantiate(productsParent.transform.GetChild(0).gameObject,productsParent.transform.GetChild(0).gameObject.transform.parent);
            newItem.transform.GetChild(0).GetComponent<Text>().text = "x" + item.Value.ToString();
      
                string iconUrl = "";
                foreach(InventoryItems allItem in Constants.allInventoryItems)
                {
                    if(allItem.itemName == item.Key)
                    {
                        iconUrl = allItem.iconUrl;
                        break;
                    }
                }
               gm.publicImages.StartCoroutine(gm.publicImages.GetTexture(iconUrl,newItem.GetComponent<Image>()));
                newItem.gameObject.SetActive(true);
        }
    }

    public void ResetData()
    {
        rewards = new Dictionary<string, int>();
        int index = 0;
        foreach(Transform child in productsParent)
        {
            if(index != 0)
            {
                Destroy(child.gameObject);
            }
            index++;
        }
    }
}
