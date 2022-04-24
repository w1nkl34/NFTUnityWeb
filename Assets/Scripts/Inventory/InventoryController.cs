using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public ItemHolder itemHolder;
    public GameManager gm;

    public void ResetAllItems()
    {
        int count = 0;
        foreach(Transform child in itemHolder.gameObject.transform.parent)
        {
            if(count != 0)
            Destroy(child.gameObject);
            count++;
        }
    }
    public void GenerateItems()
    {
        ResetAllItems();
        for(int i =0; i<Constants.currentUser.inventoryItems.Count; i++)
        {
            if(Constants.currentUser.inventoryItems[i].amount > 0)
            InstantiateItem(Constants.currentUser.inventoryItems[i].itemName,Constants.currentUser.inventoryItems[i].amount.ToString());
        }
    }

    public void InstantiateItem(string itemName,string itemCount)
    {
        GameObject newItem = Instantiate(itemHolder.gameObject,itemHolder.gameObject.transform.parent);
        newItem.GetComponent<ItemHolder>().itemName = itemName;
        newItem.GetComponent<ItemHolder>().itemCount = itemCount;
         string iconUrl = "";
                foreach(InventoryItems allItem in Constants.allInventoryItems)
                {
                    if(allItem.itemName == itemName)
                    {
                        iconUrl = allItem.iconUrl;
                        break;
                    }
                }
        newItem.gameObject.SetActive(true);
        newItem.GetComponent<Image>().sprite = gm.publicImages.loadingSprite;
        gm.publicImages.StartCoroutine(gm.publicImages.GetTexture(iconUrl,newItem.GetComponent<Image>()));
    }
}
