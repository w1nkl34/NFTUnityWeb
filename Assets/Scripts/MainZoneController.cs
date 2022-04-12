using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainZoneController : MonoBehaviour
{
   public GameObject zones;
   public bool isLarge = false;
   public Zones zoneData;
   public GameObject fieldsParent;
   public Text zoneName;
    public void GenerateZones()
    {
       zones.gameObject.SetActive(true);
    }
    public void GenerateFields()
    {
        zoneName.text = zoneData.zoneName;
        int index = 0;
        foreach(Transform child in fieldsParent.transform)
        {
            child.GetComponent<FieldController>().field = zoneData.fields[index];
            child.GetComponent<FieldController>().zone = zoneData;
            child.gameObject.SetActive(true);
            index++;
            if(index >= zoneData.fields.Count)
            break;
        }
    }
}
