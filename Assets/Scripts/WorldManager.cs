using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
   public GameObject worldPlaces;

   public MainZoneController selectedMainZone;

   public void ChangeZoneFocus(bool value)
   {
        worldPlaces.SetActive(value);
   }
}
