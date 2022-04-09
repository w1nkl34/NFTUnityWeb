using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDestroyWorker : MonoBehaviour
{

  public Workers worker;
  public Text receiveText;

  
    public void OpenDestroyWorker(Workers worker)
    {
        this.worker = worker;
        if(worker.rarity == "normal")
        receiveText.text = "You Will Receive 5 Summon Crystals";

        if(worker.rarity == "rare")
        receiveText.text = "You Will Receive 1 Blessed Summon Crystals";

        if(worker.rarity == "epic")
        receiveText.text = "You Will Receive 5 Blessed Summon Crystals";

        if(worker.rarity == "legendary")
        receiveText.text = "You Will Receive 1 Legendary Summon Crystals";       
    }
  public void ClosePop()
  {
      gameObject.SetActive(false);
  }

  public void DestroyWorker()
  {
      int summonCrystal = 0;
      int blessedSummonCrystal = 0;
      int legendarySummonCrystal = 0;
        if(worker.rarity == "normal")
        summonCrystal = 5;

        if(worker.rarity == "rare")
        blessedSummonCrystal = 1;

        if(worker.rarity == "epic")
        blessedSummonCrystal = 5;

        if(worker.rarity == "legendary")
        legendarySummonCrystal = 1;

    FindObjectOfType<FirebaseApi>().DestroyWorker(worker.docId,summonCrystal,blessedSummonCrystal,legendarySummonCrystal);
  }
}
