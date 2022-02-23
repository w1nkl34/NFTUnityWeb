using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorkerHolder : MonoBehaviour
{
    public Workers worker;
    public GameObject NFTTitle;
    public GameObject createNFTButton;
    public Image workerImage;
    public Text staminaText;
    public Text peridotSpeedText;
    public Text woodSpeedText;
    public Text stoneSpeedText;
    public Text levelText;
    public Text luckText;
    public Text rarityText;

    void OnEnable()
    {
        if(worker.isNFT == true)
        NFTTitle.SetActive(true);
        else
        createNFTButton.SetActive(true);
       
        peridotSpeedText.text = "Peridot Speed(x1) : " + worker.peridotWorkSpeed.ToString();
       
        woodSpeedText.text = "Wood Speed(x1) : " + worker.woodWorkSpeed.ToString();
        
        stoneSpeedText.text = "Stone Speed(x1) : " + worker.stoneWorkSpeed.ToString();

        luckText.text = "Luck : " + worker.luck.ToString();

        staminaText.text = "Stamina : " + worker.currentStamina.ToString() + "/" + worker.stamina.ToString();
        
        levelText.text = "Level : " + worker.level;

        rarityText.text = "Rarity : " + worker.rarity;

    }
    public void CreateNFT()
    {
        FindObjectOfType<ReactSend>().CreateNFTCall(worker.docId);
    }

}
