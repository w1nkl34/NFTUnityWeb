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

    public void ChangeFieldsMode(bool value)
    {
        foreach(Transform child in fieldsParent.transform)
        {
            child.GetComponent<BoxCollider>().enabled = value;
            child.GetComponent<Image>().enabled = value;
            if( child.GetComponent<FieldController>().UpgradeLeft != null)
            {
            if(!value)
            child.GetComponent<FieldController>().UpgradeLeft.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
            else
            child.GetComponent<FieldController>().UpgradeLeft.GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,1);
            }

        }
    }

    public void GenerateZones()
    {
        ChangeFieldsMode(true);
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
            child.GetComponent<FieldController>().StartField();
            index++;
            if(index >= zoneData.fields.Count)
            break;
        }
    }

}
