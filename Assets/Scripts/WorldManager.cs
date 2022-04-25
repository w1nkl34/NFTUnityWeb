using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
   public GameObject worldPlaces;

   public Sprite productionImage;

   public Sprite battleImage;

   [HideInInspector]
   public MainZoneController selectedMainZone;

   public List<MainZoneController> mainZoneControllersList = new List<MainZoneController>();

   public void ChangeZoneFocus(bool value)
   {
        worldPlaces.SetActive(value);
   }

   public void HideFields()
   {
        if(selectedMainZone != null)
       selectedMainZone.ChangeFieldsMode(false);
   }
   public void GenerateZones()
   {
         for(int i = 0; i< Constants.allZones.Count; i++)
         {
            int zoneIndex = 0;
            for(int j= 0; j<Constants.allZones.Count; j++)
            {
               if(Constants.allZones[j].zoneIndex == i)
               {
                  zoneIndex = j;
                  break;
               }
            }
            mainZoneControllersList[i].zoneData = Constants.allZones[zoneIndex];
            mainZoneControllersList[i].GenerateFields();
         }
   }

   public void GenerateSpecificZoneNoWorkerWorkDoc(string zoneDocId,string fieldDocId)
   {
       for(int i = 0; i< Constants.allZones.Count; i++)
         {
            int zoneIndex = 0;
            for(int j= 0; j<Constants.allZones.Count; j++)
            {
               if(Constants.allZones[j].zoneIndex == i)
               {
                  zoneIndex = j;
                  break;
               }
            }
            // mainZoneControllersList[i].zoneData = Constants.allZones[zoneIndex];
            if(mainZoneControllersList[i].zoneData.docId == zoneDocId)
            mainZoneControllersList[i].GenerateSpecificFieldNoWorkerWorkDoc(fieldDocId);
         }
   }

      public void GenerateSpecificZone(string workerWorkDocId,string zoneDocId,string fieldDocId)
   {
         for(int i = 0; i< Constants.allZones.Count; i++)
         {
            int zoneIndex = 0;
            for(int j= 0; j<Constants.allZones.Count; j++)
            {
               if(Constants.allZones[j].zoneIndex == i)
               {
                  zoneIndex = j;
                  break;
               }
            }
            // mainZoneControllersList[i].zoneData = Constants.allZones[zoneIndex];
            if(mainZoneControllersList[i].zoneData.docId == zoneDocId)
            mainZoneControllersList[i].GenerateSpecificField(workerWorkDocId,fieldDocId);
         }
   }
}
