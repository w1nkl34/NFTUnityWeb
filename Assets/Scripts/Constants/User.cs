using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class User 
{
    public int mainTowerLevel;
    public int stoneDepositLevel;
    public int woodDepositLevel;
    public int workerCapacity;
    public int workerBuildingLevel;
    public int warriorBuildingLevel;
    public float peridotShardCount;
    public float stoneCount;
    public float woodCount;
    public List<Workers> workers = new List<Workers>(); 
    public InventoryItems inventoryItems;

    public User(int mainTowerLevel, int stoneDepositLevel,int woodDepositLevel,int workerCapacity,
    int workerBuildingLevel,int warriorBuildingLevel,float peridotShardCount,float stoneCount,
    float woodCount,List<Workers> workers,InventoryItems inventoryItems)
    {
      this.mainTowerLevel = mainTowerLevel;
      this.stoneDepositLevel = stoneDepositLevel;
      this.woodDepositLevel = woodDepositLevel;
      this.workerCapacity = workerCapacity;
      this.workerBuildingLevel = workerBuildingLevel;
      this.warriorBuildingLevel = warriorBuildingLevel;
      this.peridotShardCount = peridotShardCount;
      this.stoneCount = stoneCount;
      this.woodCount = woodCount;
      this.workers = workers;
      this.inventoryItems = inventoryItems;
    }
}
