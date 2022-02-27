using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWorkerDetail : MonoBehaviour
{
     public Workers worker;
    public GameObject NFTTitle;
    public Image workerImage;
    public Text staminaText;
    public Text peridotSpeedText;
    public Text woodSpeedText;
    public Text stoneSpeedText;
    public Text levelText;
    public Text luckText;
    public Text rarityText;
    public GameObject createNFTButton;
    public GameObject sellNFTButton;

    public Text workerNameText;


    public void ClosePop()
    {
        gameObject.SetActive(false);
    }
    public void CreateNFT()
    {
        FindObjectOfType<ReactSend>().CreateNFTCall(worker.docId);
    }

    public void OpenWorkerDetails(Workers worker,Sprite workerSprite)
    {   

        workerImage.sprite = workerSprite;
        this.worker = worker;

        workerNameText.text = worker.type;
         if(worker.isNFT == true)
         {
            NFTTitle.SetActive(true);
            sellNFTButton.SetActive(true);
            createNFTButton.SetActive(false);

         }
        else
        {
            NFTTitle.SetActive(false);
            createNFTButton.SetActive(true);
            sellNFTButton.SetActive(false);
        }

        peridotSpeedText.text = "Peridot Speed(x1) : " + worker.peridotWorkSpeed.ToString();
       
        woodSpeedText.text = "Wood Speed(x1) : " + worker.woodWorkSpeed.ToString();
        
        stoneSpeedText.text = "Stone Speed(x1) : " + worker.stoneWorkSpeed.ToString();

        luckText.text = "Luck : " + worker.luck.ToString();

        staminaText.text = "Stamina : " + worker.currentStamina.ToString() + "/" + worker.stamina.ToString();
        
        levelText.text = "Level : " + worker.level;

        rarityText.text = "Rarity : " + worker.rarity;

        peridotSpeedText.text = "Peridot Speed(x1) : " + worker.peridotWorkSpeed.ToString();
       
        woodSpeedText.text = "Wood Speed(x1) : " + worker.woodWorkSpeed.ToString();
        
        stoneSpeedText.text = "Stone Speed(x1) : " + worker.stoneWorkSpeed.ToString();

        luckText.text = "Luck : " + worker.luck.ToString();

        staminaText.text = "Stamina : " + worker.currentStamina.ToString() + "/" + worker.stamina.ToString();
        
        levelText.text = "Level : " + worker.level;

        rarityText.text = "Rarity : " + worker.rarity;
    }
}
