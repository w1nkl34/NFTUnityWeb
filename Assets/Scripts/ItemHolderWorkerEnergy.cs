using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderWorkerEnergy : MonoBehaviour
{
    public GameObject selectedObj;

    public Image productImage;

    public Text productAmount;

    public InventoryItems inventoryItem;
    
    public PopUpWorkerEnergyIncrease popUpWorkerEnergyIncrease;

    public void ChangeSelectedItem()
    {
        popUpWorkerEnergyIncrease.ChangeSelectedItem(inventoryItem);
    }
}
