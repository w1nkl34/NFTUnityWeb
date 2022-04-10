using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine;

public static class Constants
{
      public static User currentUser;

      public static bool onFade = false;
      public static List<Buildings> allBuildings = new List<Buildings>();

      public static bool authenticated = false;

      public static bool onMenu = false;

      public static bool onWorldMap = false;

      public static List<Sprite> workerSprites = new List<Sprite>();

      public static List<Workers> workersData = new List<Workers>();

      public static List<Zones> allZones = new List<Zones>();

      public static bool IsPointerOverUIObject() {
     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
     List<RaycastResult> results = new List<RaycastResult>();
     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
     return results.Count > 0;
    }

}

