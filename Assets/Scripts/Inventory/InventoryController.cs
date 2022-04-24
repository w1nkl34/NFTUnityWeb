using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public ItemHolder itemHolder;

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
            InstantiateItem(Constants.currentUser.inventoryItems[i].itemName,Constants.currentUser.inventoryItems[i].amount.ToString());
        }
    }

    public void InstantiateItem(string itemName,string itemCount)
    {
        GameObject newItem = Instantiate(itemHolder.gameObject,itemHolder.gameObject.transform.parent);
        newItem.GetComponent<ItemHolder>().itemName = itemName;
        newItem.GetComponent<ItemHolder>().itemCount = itemCount;
        newItem.gameObject.SetActive(true);
    }
}
