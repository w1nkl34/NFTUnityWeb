using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings 
{
    public string buildingName;
    public int maxLevel;
    public List<BuildingLevels> levels = new List<BuildingLevels>();
}

public class BuildingLevels
{
    public int level;
    public List<Dictionary<string,object>> input;
    public List<Dictionary<string,object>> output;
    public List<Dictionary<string,object>> price;
    public string id;
}
