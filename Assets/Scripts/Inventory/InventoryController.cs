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
        if(Constants.currentUser.inventoryItems.powerCrystal != 0)
            InstantiateItem("Power Crystal",Constants.currentUser.inventoryItems.powerCrystal.ToString());
        if(Constants.currentUser.inventoryItems.summonCrystal != 0)
            InstantiateItem("Summon Crystal",Constants.currentUser.inventoryItems.summonCrystal.ToString());
        if(Constants.currentUser.inventoryItems.upgradeCrystal != 0)
            InstantiateItem("Upgrade Crystal",Constants.currentUser.inventoryItems.upgradeCrystal.ToString());

        if(Constants.currentUser.inventoryItems.blessedPowerCrystal != 0)
            InstantiateItem("Blessed Power Crystal",Constants.currentUser.inventoryItems.blessedPowerCrystal.ToString());
        if(Constants.currentUser.inventoryItems.blessedSummonCrystal != 0)
            InstantiateItem("Blessed Summon Crystal",Constants.currentUser.inventoryItems.blessedSummonCrystal.ToString());
        if(Constants.currentUser.inventoryItems.blessedUpgradeCrystal != 0)
            InstantiateItem("Blessed Upgrade Crystal",Constants.currentUser.inventoryItems.blessedUpgradeCrystal.ToString());

        if(Constants.currentUser.inventoryItems.legendaryPowerCrystal != 0)
            InstantiateItem("Legendary Power Crystal",Constants.currentUser.inventoryItems.legendaryPowerCrystal.ToString());
        if(Constants.currentUser.inventoryItems.legendarySummonCrystal != 0)
            InstantiateItem("Legendary Summon Crystal",Constants.currentUser.inventoryItems.legendarySummonCrystal.ToString());
        if(Constants.currentUser.inventoryItems.legendaryUpgradeCrystal != 0)
            InstantiateItem("Legendary Upgrade Crystal",Constants.currentUser.inventoryItems.legendaryUpgradeCrystal.ToString());
    }

    public void InstantiateItem(string itemName,string itemCount)
    {
        GameObject newItem = Instantiate(itemHolder.gameObject,itemHolder.gameObject.transform.parent);
        newItem.GetComponent<ItemHolder>().itemName = itemName;
        newItem.GetComponent<ItemHolder>().itemCount = itemCount;
        newItem.gameObject.SetActive(true);
    }
}
