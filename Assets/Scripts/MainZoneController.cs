using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainZoneController : MonoBehaviour
{
   public GameObject zones;
   public bool isLarge = false;

   public void GenerateZones()
   {
       zones.gameObject.SetActive(true);
   }
}
