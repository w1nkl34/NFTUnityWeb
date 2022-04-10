using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpZoneAccessRequiredMainTowerLevel : MonoBehaviour
{

    public Text detailText;

    public void OpenZoneAccess(string level)
    {
        detailText.text = "You Have To Upgrade Your MainTower To Level " +  level + " To Access This Zone!";
    }
    public void ClosePop()
   {
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
       gameObject.SetActive(false);
   }
}
